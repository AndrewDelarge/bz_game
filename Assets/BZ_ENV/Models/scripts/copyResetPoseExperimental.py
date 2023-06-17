import bpy
import bpyUtils
from mathutils import Vector
collection = bpyUtils.ClassCollection.getCollection()


@collection.add()
class BoneData(bpyUtils.PropertyGroup):
    calculateRestPose: bpy.props.BoolProperty(
        name = "Calculate Reset Pose", 
        options = set(),
        default = False)    
    boneName: bpy.props.StringProperty(
        name = "Copy From",
        options = set())
    copyHead: bpy.props.BoolProperty(
        name = "Copy Head", 
        options = set(),
        default = True)
    copyTail: bpy.props.BoolProperty(
        name = "Copy Tail", 
        options = set(),
        default = True)
    copyRoll: bpy.props.BoolProperty(
        name = "Copy Roll", 
        options = set(),
        default = True)
    offset: bpy.props.FloatVectorProperty(
        options = set()
    )
    def drawInList(self, layout: bpy.types.UILayout, index: int):
        pass
    def drawInPanel(self, layout: bpy.types.UILayout):
        data = BoneDataPointer.getValue()
        use_connect = bpy.context.object.data.bones.active.use_connect
        layout.use_property_split = True
        layout.prop(self, 'calculateRestPose')

        column = layout.column(align = False)
        column.enabled = data.calculateRestPose
        column.prop(self, 'boneName')


        column = layout.column(align = False)
        column.enabled = data.calculateRestPose and not use_connect
        column.prop(self, 'copyHead')
        column.prop(self, 'copyTail')
        column.prop(self, 'copyRoll')

        column = layout.column(align = False)
        column.enabled = data.calculateRestPose and not use_connect
        if data.copyHead and not data.copyTail:
            column.prop(self, 'offset', text="Tail Offset")
        elif data.copyTail and not data.copyHead:
            column.prop(self, 'offset', text="Head Offset")
        else:
            column.enabled = False
            column.prop(self, 'offset', text=" ")

BoneDataPointer = bpyUtils.Pointer(bpy.types.Bone, "object.data.bones.active.da_bone_data", BoneData)
def copyRestPose_experimental(fromObj, toObj):
    fromArmature = fromObj.data
    toArmature = toObj.data

    fromMatrix = fromObj.matrix_world.to_3x3()
    toMatrix = toObj.matrix_world.to_3x3()
    fromToMatrix = fromMatrix @ toMatrix.inverted()

    currentActiveObject = bpy.context.view_layer.objects.active
    selectedObject = bpy.context.selected_objects

    bpy.ops.object.mode_set(mode='OBJECT')
    bpy.ops.object.select_all(action='DESELECT')
    fromObj.select_set(True)
    toObj.select_set(True)
    bpy.ops.object.mode_set(mode='EDIT')

    for i in range(len(toArmature.bones)):
        toBone = toArmature.bones[i]
        toBoneData: BoneData = BoneDataPointer.getValueFromContainingObject(toBone)

        if not toBoneData.calculateRestPose:
            continue
        print('bones[' + toBone.name + '] copy from ' + toBoneData.boneName)
        if not toBoneData.copyHead and not toBoneData.copyTail:
            print('bones[' + toBone.name + '] not toBoneData.copyHead and not toBoneData.copyTail')
            continue
        
        fromEditBone = fromArmature.edit_bones.get(toBoneData.boneName)
        toEditBone = toArmature.edit_bones.get(toBone.name)
        if fromEditBone == None: 
            print('bones["' + toBone.name, '"] fromEditBone == None')
            continue
        if toEditBone == None: 
            print('bones["' + toBone.name, '"] toEditBone == None')
            continue

        if toBone.use_connect:
            toEditBone.head = fromToMatrix @ fromEditBone.head
            toEditBone.tail = fromToMatrix @ fromEditBone.tail
            toEditBone.roll = fromEditBone.roll
            print('bones[' + toBone.name + '] #1')
            continue
        
        if toBoneData.copyHead:
            toEditBone.head = fromToMatrix @ fromEditBone.head
            if toBoneData.copyTail:
                print('bones[' + toBone.name + '] #2')
                toEditBone.tail = fromToMatrix @ fromEditBone.tail
            else:
                print('bones[' + toBone.name + '] #3')
                toEditBone.tail =  toEditBone.head + Vector(toBoneData.offset)
        else:
            print('bones[' + toBone.name + '] #4')
            toEditBone.tail = fromToMatrix @ fromEditBone.tail
            toEditBone.head = toEditBone.tail + Vector(toBoneData.offset)
        
        if toBoneData.copyRoll:
            toEditBone.roll = fromEditBone.roll

    bpy.ops.object.mode_set(mode='OBJECT')
    bpy.ops.object.select_all(action='DESELECT')
    bpy.context.view_layer.objects.active = currentActiveObject
    for i in selectedObject:
        i.select_set(True)
