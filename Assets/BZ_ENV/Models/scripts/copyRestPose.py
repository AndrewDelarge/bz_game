import bpy
import bpyUtils
#print("@LOAD COPY_RESET_POSE")
collection = bpyUtils.ClassCollection.getCollection()

def copyRestPose(fromObj, toObj):
    fromArmature = fromObj.data
    toArmature = toObj.data

    fromMatrix = fromObj.matrix_world.to_3x3()
    toMatrix = toObj.matrix_world.to_3x3()
    fromToMatrix = fromMatrix @ toMatrix.inverted()
    
    bonesNames = [
        'Hips',
        'Spine0',
        'Spine1',
        'Neck',
        'Head',

        'Shoulder.L',
        'Arm.L',
        'Forearm.L',
        'Hand.L',
        'Thigh.L',
        'Leg.L',
        'Foot.L',
        'Toe.L',

        'Shoulder.R',
        'Arm.R',
        'Forearm.R',
        'Hand.R',
        'Thigh.R',
        'Leg.R',
        'Foot.R',
        'Toe.R'
    ]
    
    currentActiveObject = bpy.context.view_layer.objects.active
    selectedObject = bpy.context.selected_objects

    bpy.ops.object.mode_set(mode='OBJECT')
    bpy.ops.object.select_all(action='DESELECT')
    fromObj.select_set(True)
    toObj.select_set(True)
    bpy.ops.object.mode_set(mode='EDIT')

    for boneName in bonesNames:
        fromBone = fromArmature.edit_bones.get(boneName)
        toBone = toArmature.edit_bones.get(boneName)
        if fromBone == None or toBone == None: 
            continue
        toBone.head = fromToMatrix @ fromBone.head
        toBone.tail = fromToMatrix @ fromBone.tail
        toBone.roll = fromBone.roll

    bpy.ops.object.mode_set(mode='OBJECT')
    bpy.ops.object.select_all(action='DESELECT')
    bpy.context.view_layer.objects.active = currentActiveObject
    for i in selectedObject:
        i.select_set(True)
