import bpy
import SceneStatusKeeper
#print("@LOAD COPY_UV")
def get_uv_polygons(mesh: bpy.types.Mesh, layer_index: int):
    layer = mesh.uv_layers[layer_index].data
    polygons = [None] * len(mesh.polygons)
    for polygon_index, polygon_origin in enumerate(mesh.polygons):
        polygon_new = [None] * polygon_origin.loop_total
        for i in range(0, polygon_origin.loop_total):
            uv = layer[polygon_origin.loop_start + i].uv
            polygon_new[i] = (uv[0], uv[1])
        polygons[polygon_index] = polygon_new
    return polygons
def copy_uv(objects, layer_index: int = 0, width: int = 1024, height: int = 1024, color = [255, 255, 255], background = [0, 0, 0]):
    import uv_copy
    currentStatus = SceneStatusKeeper.SceneStatusKeeper()
    bpy.ops.object.mode_set(mode = 'OBJECT')
    polygons = []
    for obj in  objects:
        if not obj.data or not isinstance(obj.data, bpy.types.Mesh):
            continue
        mesh = obj.data
        polygons = polygons + get_uv_polygons(mesh, layer_index)
    uv_copy.draw_uv(width, height, color, background, polygons)
    currentStatus.reset()


import bpyUtils
collection = bpyUtils.ClassCollection.getCollection()
@collection.add()
class CopyUvData(bpyUtils.PropertyGroup):
    layer: bpy.props.IntProperty(name = "Layer", options = set(), min = 0)
    width: bpy.props.IntProperty(name = "Width", options = set(), subtype ='PIXEL', default = 1024)
    height: bpy.props.IntProperty(name = "Height", options = set(), subtype ='PIXEL', default = 1024)
    color: bpy.props.FloatVectorProperty(name = "Color", subtype='COLOR', size = 3, options = set(), min = 0, max = 1, default = (0.0, 0.6, 1.0))
    background: bpy.props.FloatVectorProperty(name = "Background", subtype='COLOR', size = 3, options = set(), min = 0, max = 1, default = (0.0, 0.0, 0.0))
    def drawInPanel(self, layout: bpy.types.UILayout):
        super().drawInPanel(layout)
        params = layout.operator(DA_OT_CopyUV.bl_idname)
        params.layer = self.layer
        params.width = self.width
        params.height = self.height
        params.color = self.color
        params.background = self.background    
CopyUvDataPointer = bpyUtils.Pointer(bpy.types.Scene, "scene.da_copyUv_data", CopyUvData)


@collection.add()
class DA_OT_CopyUV(bpy.types.Operator):
    bl_idname = "da.copy_uv"
    bl_label = "Copy UV"
    layer: bpy.props.IntProperty(name = "Layer", default = 0)
    width: bpy.props.IntProperty(name = "Width", default = 1024)
    height: bpy.props.IntProperty(name = "Height", default = 1024)
    color: bpy.props.FloatVectorProperty(name = "Color", size = 3, min = 0, max = 1, default = (1.0, 0.5, 0.25))
    background: bpy.props.FloatVectorProperty(name = "Background", size = 3, min = 0, max = 1, default = (0.0, 0.0, 0.0))
    
    @classmethod
    def poll(cls, context):
        meshes = filter(
            lambda obj: obj.data and isinstance(obj.data, bpy.types.Mesh), 
            bpy.context.selected_objects)
        return len(list(meshes)) > 0

    def execute(self, context):
        def convert(color):
            c = [0] * len(color)
            for index, value in enumerate(color):
                c[index] = int(255 * value)
            return c
        copy_uv(bpy.context.selected_objects, 
            self.layer, self.width, self.height, 
            convert(self.color), convert(self.background))
        return {'FINISHED'}