import bpy
class CollectionStatus:
    def __init__(self, collection) -> None:
        self.hideViewport = collection.hide_viewport
        self.collection = collection
    def reset(self):
        self.collection.hide_viewport = self.hideViewport
class ObjectStatus:
    def __init__(self, obj: bpy.types.Object) -> None:
        self.selected = obj.select_get()
        self.hide  = obj.hide_get()
        self.hideViewport = obj.hide_viewport
        self.object = obj
    def reset(self):
        obj = self.object
        obj.hide_viewport = self.hideViewport
        obj.hide_set(self.hide)
        obj.select_set(self.selected)
class SceneStatusKeeper:
    def __init__(self) -> None:
        self.currentActiveObject: bpy.types.Object = bpy.context.view_layer.objects.active
        self.selectedObjects = bpy.context.selected_objects
        self.frame_start = bpy.context.scene.frame_start
        self.frame_end = bpy.context.scene.frame_end
        self.frame_current = bpy.context.scene.frame_current
        self.mode = bpy.context.object.mode
        self.stats = []
    def addObjects(self, *objs):
        for obj in objs:
            if obj == None: continue
            self.stats.append(ObjectStatus(obj))
    def addCollections(self, *coll):
        for c in coll:
            if c == None: continue
            self.stats.append(CollectionStatus(c))
    def reset(self):
        bpy.ops.object.mode_set(mode=bpy.context.object.mode)
        for obj in bpy.context.selected_objects: obj.select_set(False)
        bpy.context.view_layer.objects.active = self.currentActiveObject
        for i in self.selectedObjects: i.select_set(True)
        bpy.context.scene.frame_start = self.frame_start
        bpy.context.scene.frame_end = self.frame_end
        bpy.context.scene.frame_current = self.frame_current
        bpy.ops.object.mode_set ( mode = self.mode )
        for stat in self.stats: stat.reset()