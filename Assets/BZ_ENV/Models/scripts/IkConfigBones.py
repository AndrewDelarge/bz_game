import bpy
import bpyUtils
import SceneStatusKeeper
import mathutils

collection = bpyUtils.ClassCollection.getCollection()

@collection.add()
class BoneConfig(bpyUtils.PropertyGroup):
    configBoneName: bpy.props.StringProperty(
        name = "Target",
        default = "",
        options = set())
    targetBoneName: bpy.props.StringProperty(
        name = "Copy From",
        options = set())
    copyLocation: bpy.props.BoolProperty(
        name = "Copy Location", 
        options = set(),
        default = False)
    local: bpy.props.BoolProperty(
        name = "Local Location", 
        options = set(),
        default = False)
    
    copyRotation: bpy.props.BoolProperty(
        name = "Copy Location", 
        options = set(),
        default = False)
    offset: bpy.props.FloatVectorProperty(
        options = set()
    )
    def drawInList(self, layout: bpy.types.UILayout, index: int):
        if self.configBoneName == '':
            layout.label(text = 'No Name', icon = 'ERROR')
        else:
            layout.label(text = self.configBoneName, icon = 'BONE_DATA')
    def drawActive(self, layout: bpy.types.UILayout, armature: bpy.types.Armature):            
        #layout.use_property_split = True
        if not armature:
            layout.label(text = "No Armature", icon = 'ERROR')
            return
        layout.prop_search(self, "configBoneName", armature.data, "bones", text="Bone") 

        column = layout.column(align = False)
        column.prop(self, 'targetBoneName')

        row = layout.row(align = True)
        row.label(text = "Copy:")
        row.prop(self, 'copyLocation', icon='CON_LOCLIKE', text = "Location")
        row.prop(self, 'local', icon='ORIENTATION_LOCAL', text = "Local")
        row.prop(self, 'copyRotation', icon='CON_ROTLIKE', text = "Rotation")

        column = layout.column(align = False)
        column.use_property_split = True
        column.enabled = False
        column.prop(self, 'offset', text=" ")

@collection.add()
class Armature(bpyUtils.PropertyGroup):
    armature: bpy.props.PointerProperty(
        type = bpy.types.Object,
        poll = lambda s, o: o.type == 'ARMATURE')

@collection.add()
class IkConfig(bpyUtils.PropertyGroup):
    bone_name: bpy.props.StringProperty()
    configArmature:bpy.types.Armature = None
    armatures: collection.createArray(type = Armature, showActive=False)
    currentArmature: bpy.props.IntProperty(name = "Character", options = set(), default = 0)
    boneConfigs: bpy.props.CollectionProperty(type = BoneConfig)
    activeIndex: bpy.props.IntProperty(default = 0)
    def drawIkConfig(self, layout: bpy.types.UILayout, armatureObj:bpy.types.Object):
        layout.label(text = 'Armatures')
        self.armatures.drawInPanel(layout)
        layout.prop(self, 'currentArmature')
        self.list(layout.row(), "boneConfigs", "activeIndex", list_id = 'IkConfig_boneConfigs')
        row = layout.row(align=True)
        self.buttonFunction(row, icon='ADD', text = "", function = self.add)
        if len(self.boneConfigs)> 0:
            self.buttonFunction(row, icon='REMOVE', text="", function = self.remove)
        if self.activeIndex >= 0 and self.activeIndex < len(self.boneConfigs):
            self.boneConfigs[self.activeIndex].drawActive(layout, armatureObj)
        self.buttonFunction(layout, text="Apply", function=lambda: self.changeArmature(armatureObj))
    def add(self):
        self.boneConfigs.add()
        self.activeIndex = len(self.boneConfigs) - 1
    def remove(self):
        self.boneConfigs.remove(self.activeIndex)
        self.activeIndex = min(self.activeIndex, max(0, len(self.boneConfigs) - 1))
    def changeArmature(self, armatureObj:bpy.types.Object):
        if (armatureObj == None): return
        targetObj = self.armatures.elements[self.currentArmature]
        if (targetObj == None): return
        apply(targetObj.armature, armatureObj, self.boneConfigs)
        
def apply(targetObject: bpy.types.Object, configObject:bpy.types.Object, config:list[BoneConfig]):
    currentStatus = SceneStatusKeeper.SceneStatusKeeper()
    for obj in bpy.context.selected_objects: obj.select_set(False)

    configObject.location = targetObject.location

    fromArmature = targetObject.data
    toArmature = configObject.pose

    fromMatrix = targetObject.matrix_world.to_3x3()
    toMatrix = configObject.matrix_world.to_3x3()
    fromToMatrix = fromMatrix @ toMatrix.inverted()

    bpy.ops.object.mode_set(mode='OBJECT')
    targetObject.select_set(True)
    configObject.select_set(True)
    bpy.ops.object.mode_set(mode='EDIT')
    
    for configData in config:        
        fromEditBone = fromArmature.edit_bones.get(configData.targetBoneName)
        toPoseBone = toArmature.bones.get(configData.configBoneName)
        if fromEditBone == None or toPoseBone == None: 
            continue
        if configData.copyRotation:
            toPoseBone.rotation_quaternion = mathutils.Quaternion(fromEditBone.tail - fromEditBone.head, fromEditBone.roll)
        if configData.copyLocation:
            fromLocation = fromEditBone.head.copy()
            if configData.local and fromEditBone.parent:
                fromLocation -= fromEditBone.parent.head
            (fromLocation.y, fromLocation.z) = (fromLocation.z, -fromLocation.y)
            toPoseBone.location = fromToMatrix @ fromLocation
            print(f"{toPoseBone.name}->{fromEditBone.name} head:{fromEditBone.head} location:{toPoseBone.location}")

        
    currentStatus.reset()