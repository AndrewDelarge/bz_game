import bpy
import NameFormatter
def getValueFromContext(path: list[str], type=None):
        ptr = bpy.context
        for p in path:
            ptr = getattr(ptr, p, None)
            if ptr == None:
                return None
        if type != None and not isinstance(ptr, type):
            return None
        return ptr

def createListClass(type):
    class BpyList(PropertyGroup):
        elements: bpy.props.CollectionProperty(type = type)
        activeIndex: bpy.props.IntProperty(default = 0)
        def drawInPanel(self, layout: bpy.types.UILayout):
            self.list(layout.row(), "elements", "activeIndex", list_id = type.__name__)
            row = layout.row(align=True)
            self.buttonFunction(row, icon='ADD', text = "", function = self.add)
            if len(self.elements)> 0:
                self.buttonFunction(row, icon='REMOVE', text="", function = self.remove)
            if self.activeIndex >= 0 and self.activeIndex < len(self.elements):
                self.elements[self.activeIndex].drawInListActive(layout, self.activeIndex)
        def add(self):
            self.elements.add()
            self.activeIndex = len(self.elements) - 1
        def remove(self):
            self.elements.remove(self.activeIndex)
            self.activeIndex = min(self.activeIndex, max(0, len(self.elements) - 1))
    return BpyList

class DA_OT_ButtonFunction(bpy.types.Operator):
    bl_idname = "da.button_function"
    bl_label = "Button"
    classIndex: bpy.props.IntProperty(name = "Class Index")    
    buttonIndex: bpy.props.IntProperty(name = "Button Index")    
    @classmethod
    def poll(cls, context):
        return True
    def execute(self, context): 
        collection = ClassCollection.getCollection()
        if self.classIndex < 0 or self.classIndex >= len(collection.__classes__):
            return {'CANCELLED'}
        panel = collection.__classes__[self.classIndex]
        bpy_utils_execButton =  getattr(panel, 'bpy_utils_execButton', None)
        if bpy_utils_execButton == None or not bpy_utils_execButton(self.buttonIndex):
            return {'CANCELLED'}
        return {'FINISHED'}

from bpy_extras.io_utils import (ImportHelper, ExportHelper)
class DA_OT_ButtonFunctionSave(bpy.types.Operator, ExportHelper):
    bl_idname = "da.button_function_save"
    bl_label = "Button"
    filename_ext = ".da_data"
    def execute(self, context): 
        return {'FINISHED'}

class DA_OT_ButtonFunctionLoad(bpy.types.Operator, ImportHelper):
    bl_idname = "da.button_function_load"
    bl_label = "Button"
    filename_ext = ".da_data"
    def execute(self, context): 
        return {'FINISHED'}
class Pointer:
    def __init__(self, place_class:type, place: str, bpyType:type):
        path = place.split('.')
        self.full_path = place
        self.place_name = path[-1]
        self.place_path = path[:-1]
        self.place_class = place_class
        self.groupType = bpyType
        ClassCollection.getCollection().__pointeres__.append(self)
    def createPanel(self, panelName:str, panel:'Panel', drawInPanel:callable = None):
        def valueDrawInPanel(self, context):
            cls = type(self)
            ClassCollection.getCollection().__curentPanel__ = cls
            cls.bpy_utils_buttons = []
            self.getValue().drawInPanel(self.layout)
        def selfDrawInPanel(self, context):
            cls = type(self)
            ClassCollection.getCollection().__curentPanel__ = cls
            cls.bpy_utils_buttons = []
            self.drawInPanel.__func__(self.getValue(), self.layout)
        def poll(cls, context):
            value = cls.bpy_utils_propertyGroupPointer.getValue()
            return value != None and value.poll()
        def bpy_utils_addButton(cls, method):
            index = len(cls.bpy_utils_buttons)
            cls.bpy_utils_buttons.append(method)
            return index
        def bpy_utils_execButton(cls, index):
            if index < 0 or index >= len(cls.bpy_utils_buttons):
                return False
            cls.bpy_utils_buttons[index]()
            return True
        def getValue(self):
            return self.bpy_utils_propertyGroupPointer.getValue()
        
        collection = ClassCollection.getCollection()
        panelIdName = NameFormatter.generatePanelName(
            collection.__prefix__, 
            self.groupType.__name__ + '_' + NameFormatter.nameModification(panelName))
        label = panelName
        classProperties = {
            'bl_idname': panelIdName,
            'bl_label': label,
            'bl_region_type': panel.bl_region_type,
            'bl_space_type': panel.bl_space_type,
            'bl_context': panel.bl_context,
            'bl_category': panel.bl_category,
            'poll': classmethod(poll),
            'draw': valueDrawInPanel,
            'bpy_utils_propertyGroupPointer': self,
            'bpy_utils_panel_index': len(collection.__classes__),
            'bpy_utils_buttons' : [],
            'bpy_utils_addButton': classmethod(bpy_utils_addButton),
            'bpy_utils_execButton': classmethod(bpy_utils_execButton),
            'getValue': getValue,
        }

        if drawInPanel:
            classProperties['draw'] = selfDrawInPanel
            classProperties['drawInPanel'] = drawInPanel
        
        newClass = type(panelIdName, (bpy.types.Panel,), classProperties)
        collection.__classes__.append(newClass)
        return self
    def getValue(self):
        ptr = getValueFromContext(self.place_path, self.place_class)
        if ptr != None:
            return getattr(ptr, self.place_name)
        return ptr
    def getValueFromContainingObject(self, obj):
        if not isinstance(obj, self.place_class):
            return None
        return getattr(obj, self.place_name)
def isPossibleToShowPropertyGroup(g):
    return g.bpy_utils_propertyGroupPointer.getValue() != None

class PropertyGroup(bpy.types.PropertyGroup):
    def poll(self): return True
    def drawInPanel(self, layout: bpy.types.UILayout):
        layout.use_property_split = True
        for k, _ in self.__annotations__.items():
            layout.prop(self, k)
    def drawInListActive(self, layout: bpy.types.UILayout, index: int):
        self.drawInPanel(layout)
    def drawInList(self, layout: bpy.types.UILayout, index: int):
        self.drawInPanel(layout)
    def drawInGrid(self, layout: bpy.types.UILayout, index: int):
        self.drawInPanel(layout)
    def buttonFunction(self, layout: bpy.types.UILayout, function, poll=None, text=None, text_ctxt=None, translate=True, icon='NONE', emboss=True, depress=False, icon_value=0):
        panel = ClassCollection.getCollection().__curentPanel__
        prop = layout.operator(
            DA_OT_ButtonFunction.bl_idname, 
            text=text, 
            text_ctxt=text_ctxt, 
            translate=translate, 
            icon=icon, 
            emboss=emboss, 
            depress=depress, 
            icon_value=icon_value)
        prop.classIndex = panel.bpy_utils_panel_index
        prop.buttonIndex = panel.bpy_utils_addButton(function)
    def list(self, layout: bpy.types.UILayout, elementsName:str, indexName:str, rows:int = 3, list_id: str = "DA_list"):
        layout.template_list(
            DA_UL_List.__name__, list_id, 
            self, elementsName, 
            self, indexName, rows = rows)

class Panel:
    bl_label:str = None
    bl_region_type:str = 'WINDOW'
    bl_space_type = 'EMPTY'
    bl_context:str = ''
    bl_category:str = ''
    def __init__(self, label:str = None, regionType: str = None, spaceType: str = None, context: str = None, category: str = None) -> None:
        if label: self.bl_label = label
        if regionType: self.bl_region_type = regionType
        if spaceType: self.bl_space_type = spaceType
        if context: self.bl_context = context
        if category: self.bl_category = category


class DA_UL_List(bpy.types.UIList):
    def draw_item(self, context, layout, data, item, icon, active_data,
                  active_propname, index):
        if self.layout_type in {'DEFAULT', 'COMPACT'}:
            item.drawInList(layout, index)
        elif self.layout_type in {'GRID'}:
            item.drawInGrid(layout, index)

class ClassCollection:
    __collection__ = None
    __modules__ = []
    def __init__(self, prefix: str):
        self.__classes__ = [
            DA_UL_List,
            DA_OT_ButtonFunction
        ]
        self.__pointeres__ = []     
        self.__prefix__ = prefix   
        self.__hasBeenRegistered__ = []
        self.__alreadyCreatedPointers__ = []
        self.__curentPanel__ = None
        if ClassCollection.__collection__ != None:
            raise Exception()
        ClassCollection.__collection__ = self
    @classmethod
    def getCollection(_):
        if  ClassCollection.__collection__ == None:
            return ClassCollection('DA')
        return ClassCollection.__collection__
        
    def add(self):
        def inner(cls: type):
            self.__classes__.append(cls)
            return cls
        return inner
    def __unsafeCreateGeneralPanel__(self, name, draw, poll, panel:Panel, extraProperties = {}):
        panelIdName = NameFormatter.generatePanelName(self.__prefix__, name)
        label = panel.bl_label
        if label == None: label = name
        classProperties = {
            'bl_idname': panelIdName,
            'bl_label': label,
            'bl_region_type': panel.bl_region_type,
            'bl_space_type': panel.bl_space_type,
            'bl_context': panel.bl_context,
            'bl_category': panel.bl_category,
            'poll': poll,
            'draw': draw
        }
        classProperties.update(extraProperties)
        newClass = type(panelIdName, (bpy.types.Panel,), classProperties)
        self.__classes__.append(newClass)

    def createArray(self, type, name: str = '', needExpanded: bool = False, showActive: bool = True):
        class BpyArray(PropertyGroup):
            elements: bpy.props.CollectionProperty(type = type)
            index: bpy.props.IntProperty(default = 0)
            expanded: bpy.props.BoolProperty(default=True)
            def drawInPanel(self, layout: bpy.types.UILayout):
                if needExpanded:
                    row = layout.row()
                    row.alignment = 'LEFT'
                    row.prop(self, "expanded",
                        icon="TRIA_DOWN" if self.expanded else "TRIA_RIGHT",
                        text = name,
                        icon_only = True,
                        emboss = False
                    )
                if self.expanded or not needExpanded:
                    row = layout.row()
                    row.template_list(
                        DA_UL_List.__name__, "DA_list", 
                        self, "elements", 
                        self, "index", rows = 3)
                    col = row.column(align=True)
                    self.buttonFunction(col, icon='ADD', text="", function = self.elements.add)
                    self.buttonFunction(col, icon='REMOVE', text="", function = lambda: self.elements.remove(self.index))
                if showActive and self.index >= 0 and self.index < len(self.elements):
                    active = self.elements[self.index]
                    active.drawInPanel(layout)
            def getAt(index:int) -> type:
                return self.elements[index]
        self.__classes__.append(BpyArray)
        return bpy.props.PointerProperty(type = BpyArray)
    def createListProperty(self, type):
        cls = createListClass(type)
        self.__classes__.append(cls)
        return bpy.props.PointerProperty(type = cls)
    def addModule(self, module):
        if module in self.__modules__: 
            # print ('# addModule x', module.__name__)
            return
        # print ('# addModule v', module.__name__)
        self.__modules__.append(module)
    
    def getModules(self):
        return self.__modules__[:]

    def register(self):
        def process(fromList, toList, message, getNameFunction, mainFunction):
            for index, e in enumerate(fromList):
                name = getNameFunction(index, e)
                if e in toList:
                    # print(message, name, '[Pass]')
                    continue

                # print(message, name, '[Start]')
                try:
                    mainFunction(e)
                    toList.append(e)
                    # print(message, name, '[Done ]')
                except Exception as err:
                    print(err)
                    # print(message, name, '[Fail]')
                    
        process(fromList = self.__classes__, 
                toList  = self.__hasBeenRegistered__, 
                message = '# @ClassCollection.register Try register class',
                getNameFunction = lambda index, cls: f"[{index}] {cls.__name__}",
                mainFunction = lambda cls: bpy.utils.register_class(cls))
                
        process(fromList = self.__pointeres__, 
                toList  = self.__alreadyCreatedPointers__, 
                message = '# @ClassCollection.register Try make pointer',
                getNameFunction = lambda index, g:f'[{index}] {g.place_class.__name__}::{g.place_name} -> {g.groupType.__name__}',
                mainFunction = lambda g: setattr(g.place_class, g.place_name, bpy.props.PointerProperty(type = g.groupType)))       
        
    def unregister(self):
        def process(fromList, message, getNameFunction, mainFunctin):     
            hasProlem = False       
            for e in fromList:
                name = getNameFunction(e)
                # print(message, name, '[Start]')
                try:
                    mainFunctin(e)
                    # print(message, name, '[Done ]')
                except Exception as err:
                    print(err)
                    hasProlem = True
                    # print(message, name, '[Fail]')
            return hasProlem
        hasProlem = process(
            fromList = self.__alreadyCreatedPointers__,
            message = '# @ClassCollection.unregister Try delete pointer',
            getNameFunction = lambda ptr: f'{ptr.place_class.__name__}::{ptr.place_name}',
            mainFunctin = lambda ptr: delattr(ptr.place_class, ptr.place_name)
        ) and \
        process(
            fromList = self.__hasBeenRegistered__,
            message = '# @ClassCollection.unregister Try unregister class',
            getNameFunction = lambda cls: cls.__name__,
            mainFunctin = lambda cls: bpy.utils.unregister_class(cls)
        )

        self.__alreadyCreatedPointers__.clear()
        self.__hasBeenRegistered__.clear()
        if hasProlem:
            print("Need restart blender")
    
    def totalClear(self):
        print('# totalClear begin')
        self.unregister()
        ClassCollection.__collection__ = None
        self.__modules__.clear()
        print('# totalClear end')