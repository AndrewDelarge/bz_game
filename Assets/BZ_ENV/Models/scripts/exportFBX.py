import os
import bpy
import SceneStatusKeeper
#print("@LOAD EXPORT_FBX")
import bpyUtils
collection = bpyUtils.ClassCollection.getCollection()

@collection.add()
class ExportFbxData(bpyUtils.PropertyGroup):
    animationPrefix: bpy.props.StringProperty(name = "Animation Prefix")
    destination: bpy.props.StringProperty(
        name = "Destination",
        description="Choose a directory:",
        default="",
        maxlen=1024,
        subtype='DIR_PATH')
    needClear: bpy.props.BoolProperty(
        name = "Need Clear", 
        default = False)
    exportMesh: bpy.props.BoolProperty(
        name = "Mesh", 
        default = True)
    exportAnimation: bpy.props.BoolProperty(
        name = "Animation", 
        default = True)
    def drawWithButton(self, layout: bpy.types.UILayout, nlaObject, deformingArmature, collections):
        super().drawInPanel(layout)
        row = layout.row()
        self.buttonFunction(
            layout = row, 
            text = "Export", 
            function = lambda: self.exportFBX(nlaObject, deformingArmature, collections))
        
        self.buttonFunction(
            layout = layout, 
            text = "Select All Animations", 
            function = lambda: self.selectAllAnimation(nlaObject))
        
        self.buttonFunction(
            layout = layout, 
            text = "Deselect All Animations", 
            function = lambda: self.deselectAllAnimation(nlaObject))
        row.enabled = nlaObject != None and deformingArmature != None
    
    def selectAllAnimation(self, nlaObject):
        for i in nlaObject.animation_data.nla_tracks:
            if i.name == 'A Pose' or i.name.startswith('!'):
                continue
            if len(i.strips) == 0: continue
            i.strips[0].extrapolation = 'HOLD_FORWARD'
            if i.name.startswith('#'):
                continue
            i.name = '#' + i.name

    def deselectAllAnimation(self, nlaObject):
        for i in nlaObject.animation_data.nla_tracks:
            if i.name == 'A Pose' or i.name.startswith('!'):
                continue
            if len(i.strips) == 0: continue
            i.strips[0].extrapolation = 'HOLD_FORWARD'
            if not i.name.startswith('#'):
                continue
            i.name = i.name[1:]

    def exportFBX(self, nlaObject, deformingArmature, collections = []):
        import shutil
        import pathlib
        destination = pathlib.Path(bpy.path.abspath(self.destination))
        animationFolder = destination / 'animations'
        meshFolder =  destination / 'meshes'
        if self.needClear and destination.is_dir():
            shutil.rmtree(destination, ignore_errors=False, onerror=None)
        if not destination.is_dir(): os.mkdir(destination)
        if not animationFolder.is_dir(): os.mkdir(animationFolder)
        if not meshFolder.is_dir(): os.mkdir(meshFolder)
        
        animationDestination = str(animationFolder / ('{name}.fbx'))
        meshDestination = str(meshFolder / '{name}_mesh.fbx')

        currentStatus = SceneStatusKeeper.SceneStatusKeeper()
        currentStatus.addObjects(deformingArmature)        
        bpy.ops.object.mode_set(mode = 'OBJECT')
        bpy.context.scene.frame_current = 0
        if self.exportMesh:
            for coll in collections: 
                currentStatus.addCollections(coll)
                currentStatus.addObjects(*coll.objects)
                coll.hide_viewport = False
                for obj in coll.objects: obj.hide_set(False)
        for obj in bpy.context.selected_objects: obj.select_set(False)
        deformingArmature.hide_set(False)
        deformingArmature.hide_viewport = False
        deformingArmature.select_set(True)
                
        tracts = []            
        animationTypes = {} 
        if self.exportAnimation:
            for i in nlaObject.animation_data.nla_tracks:
                trackName:str = i.name
                if trackName[0] == '!':
                    if trackName[1] != '[': continue
                    endIndex = trackName.find(']')
                    if endIndex < 0: continue
                    params = trackName[2:endIndex].split(':')
                    if len(params) != 2: continue
                    destinations = animationTypes.get(params[0])
                    if destinations == None:
                        destinations = {}
                        animationTypes[params[0]] = destinations

                    typeTracks = destinations.get(params[1])
                    if typeTracks == None:
                        typeTracks = []
                        destinations[params[1]] = typeTracks
                    typeTracks.append((i, i.mute))
                    i.mute = True   
                    continue

                if len(i.strips) == 0: continue
                if trackName.startswith('#'):
                    tracts.append((i, i.mute))
                    i.mute = True    
            if False: #for debug
                print(f"len(animationTypes) == {len(animationTypes)}")
                for typeName, destinations in animationTypes.items():
                    print(f"{typeName}")
                    for destinationName, destinationsTracks in destinations.items():
                        print(f"-{destinationName}")
                        for t in destinationsTracks:
                            print(f"--{t[0].name}")
            
            def exportHelper(name):
                typeStart = name.find('[')
                if typeStart < 0:
                    bpy.ops.export_scene.fbx(
                        filepath= animationDestination.format(name = name), 
                        use_selection = True, 
                        apply_scale_options = 'FBX_SCALE_UNITS', 
                        add_leaf_bones = False, 
                        use_armature_deform_only = True, 
                        use_space_transform = True,
                        bake_anim = True, 
                        bake_anim_use_all_bones = False, 
                        bake_anim_use_nla_strips = False, 
                        bake_anim_use_all_actions = False, 
                        bake_anim_simplify_factor = 0.0)
                    return
                typeEnd = name.find(']')
                if typeEnd < 0 or typeStart >= typeEnd:
                    raise Exception()
                destinations = animationTypes[name[typeStart + 1:typeEnd]]
                for destinationName, tracks in destinations.items():
                    newName = name[0:typeStart] + destinationName + name[typeEnd + 1:]
                    for t in tracks: t[0].mute = False
                    exportHelper(newName)
                    for t in tracks: t[0].mute = True
            
            for i in tracts:
                nlaTrack = i[0]
                if len(nlaTrack.strips) == 0: continue
                nlaTrack.mute=False
                bpy.context.scene.frame_start = int(nlaTrack.strips[0].frame_start)
                bpy.context.scene.frame_end = int(nlaTrack.strips[-1].frame_end)
                exportHelper(nlaTrack.name[1:])
                nlaTrack.mute=True
        
        if self.exportMesh:
            for collection in collections:
                for obj in collection.objects: obj.select_set(True)
                bpy.ops.export_scene.fbx(
                    filepath = meshDestination.format(name = collection.name), 
                    use_selection = True, 
                    apply_scale_options = 'FBX_SCALE_UNITS', 
                    use_space_transform = True,
                    add_leaf_bones=False, 
                    use_armature_deform_only = True, 
                    bake_anim = False)
                for obj in collection.objects: obj.select_set(False)
            
        for i in tracts:
            i[0].mute = i[1]
        
        for _, destinations in animationTypes.items():
            for _, destinationsTracks in destinations.items():
                for t in destinationsTracks:
                    t[0].mute = t[1]
        currentStatus.reset()
        
ExportFbxDataPointer = bpyUtils.Pointer(bpy.types.Armature, "object.data.da_export_data", ExportFbxData)

        