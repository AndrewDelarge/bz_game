
import bpy
import bpyUtils

def changeProperty(action: bpy.types.Action, oldProperty:str, newProperty:str):
    for i in action.fcurves:
        if i.data_path == oldProperty:
            i.data_path = newProperty
            return
def chagePrefix(action: bpy.types.Action, oldPrefix:str, newPrefix:str):
    for i in action.fcurves:
        if i.data_path.startswith(oldPrefix):
            i.data_path = newPrefix + i.data_path[len(oldPrefix):]

collection = bpyUtils.ClassCollection.getCollection()

@collection.add()
class DataForRenameProperty(bpyUtils.PropertyGroup):
    oldProperty: bpy.props.StringProperty(options = set())
    newProperty: bpy.props.StringProperty(options = set())
    isPrefix: bpy.props.BoolProperty(default = False, options = set())
    fixDrivers: bpy.props.BoolProperty(default = False, options = set())
    def drawInList(self, layout: bpy.types.UILayout, index: int):
        if self.oldProperty == '':
            layout.label(text = 'None', icon = 'ERROR')
        else:
            layout.label(text = self.oldProperty)


@collection.add()
class DataForRenameProperties(bpyUtils.createListClass(DataForRenameProperty)):
    def drawWithButtons(self, layout: bpy.types.UILayout, nlaObj:bpy.types.Object):
        self.list(layout.row(), "elements", "activeIndex", list_id = type.__name__)
        row = layout.row(align=True)
        self.buttonFunction(row, icon='ADD', text = "", function = self.add)
        self.buttonFunction(row, icon='ADD', text = "Property", function = self.addProps)
        self.buttonFunction(row, icon='ADD', text = "Bone", function = self.addBones)
        if len(self.elements)> 0:
            self.buttonFunction(row, icon='REMOVE', text="", function = self.remove)
        if self.activeIndex >= 0 and self.activeIndex < len(self.elements):
            self.elements[self.activeIndex].drawInListActive(layout, self.activeIndex)
        self.buttonFunction(layout, lambda: self.renameAllProperties(nlaObj), text="Apply")
    def addProps(self):
        boneProps = DataForRenameProperties.getNameOfActiveProperties()
        for p in boneProps:
            el = self.elements.add()
            el.oldProperty = p
            el.newProperty = p
            el.isPrefix = False
        self.activeIndex = len(self.elements) - 1

    def addBones(self):
        boneProps = []
        for p in DataForRenameProperties.getNameOfActiveProperties():
            if p.startswith('pose'):
                prop = p[:p.index(']') + 1]
                if prop in boneProps: continue
                boneProps.append(prop)
        for p in boneProps:
            el = self.elements.add()
            el.oldProperty = p
            el.newProperty = p
            el.isPrefix = True
        self.activeIndex = len(self.elements) - 1
    def save(self):
        pass
    def load(self):
        pass

    @staticmethod
    def getNameOfActiveProperties():
        if not bpy.context.object or not bpy.context.object.animation_data or not bpy.context.object.animation_data.action:
            return []
        return [i.data_path for i in bpy.context.object.animation_data.action.fcurves if i.select]
    def renameAllProperties(self, nlaObj:bpy.types.Object):
        for track in nlaObj.animation_data.nla_tracks:
            for strip in track.strips:
                action = strip.action
                for property in self.elements:
                    if property.isPrefix:
                        chagePrefix(action, property.oldProperty, property.newProperty)
                    else:
                        changeProperty(action, property.oldProperty, property.newProperty)
        for property in self.elements:
            if not property.fixDrivers: continue
            if property.isPrefix: continue # DO TO Fix it
            for fcurve in nlaObj.animation_data.drivers:
                for variable in fcurve.driver.variables:
                    for target in variable.targets:
                        if target.data_path == property.oldProperty:
                            target.data_path = property.newProperty
import bpy_extras
class ExportSomeData(bpy.types.Operator, bpy_extras.io_utils.ExportHelper):
    bl_idname = "da.save_data_of_rename_properties"  # important since its how bpy.ops.import_test.some_data is constructed
    bl_label = "Export Rename Properties"

    filename_ext = ".txt"

    filter_glob: bpy.props.StringProperty(
        default="*.txt",
        options={'HIDDEN'},
        maxlen=255,  # Max internal buffer length, longer would be clamped.
    )

    def execute(self, context):
        f = open(self.filepath, 'w', encoding='utf-8')
        f.write(self.use_setting)
        f.close()
        return {'FINISHED'}
