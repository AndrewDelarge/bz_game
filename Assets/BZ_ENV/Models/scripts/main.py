import sys
import bpyUtils
import os
import bpy
class DANla(bpyUtils.Panel):
    bl_space_type = 'NLA_EDITOR'
    bl_region_type = 'UI'
    bl_category = "DA"
class DAView3D(bpyUtils.Panel):
    bl_space_type = "VIEW_3D"
    bl_region_type = "UI"
    bl_category = 'DA'

collection = bpyUtils.ClassCollection.getCollection()

import copyResetPoseExperimental
class  DABone(bpyUtils.Panel):
    bl_space_type = 'PROPERTIES'
    bl_region_type = 'WINDOW'
    bl_context = "bone"
collection.addModule(copyResetPoseExperimental)
copyResetPoseExperimental.BoneDataPointer.createPanel('Bone Data', DABone)

import exportFBX
collection.addModule(exportFBX)
def exportFBX_drawInPanel(self, layout):
    data: ArmatureControllersData = ACDPointer.getValue() 
    collections = []       
    for ch in data.characters.elements:
        if ch.element == None: continue
        collections.append(ch.element)
    self.drawWithButton(layout, data.nlaObject, data.deformArmature, collections)
exportFBX.ExportFbxDataPointer.createPanel('Export Data', DAView3D, exportFBX_drawInPanel)

import copyRestPose
collection.addModule(copyRestPose)

import IkConfigBones
collection.addModule(IkConfigBones)

import renameActionProps
collection.addModule(renameActionProps)

# @collection.add()
# class Track(bpyUtils.PropertyGroup):
#     export: bpy.props.BoolProperty()
# TrackPointer = bpyUtils.Pointer(bpy.types.NlaTrack, "object.animation_data.nla_tracks.active.da_data", Track)
# TrackPointer.createPanel("DA Settings", DANla)

@collection.add()
class WeaponData(bpyUtils.PropertyGroup):
    weaponName: bpy.props.StringProperty(options = set())
    show: bpy.props.BoolProperty(default = True, options = set(), update = lambda self, _: self.updateWeapon())
    anchorSpine: bpy.props.FloatProperty(name = "IK Anchor Spine", default = 1, soft_min = 0, soft_max = 1)
    anchor1: bpy.props.FloatProperty(name = "IK Anchor 1", default = 1, soft_min = 0, soft_max = 1)
    def drawInListActive(self, layout: bpy.types.UILayout, index: int):
        layout.prop(self, 'weaponName', text="")
    def drawInList(self, layout: bpy.types.UILayout, index: int):
        #layout.label(text=str(index) + " " + self.weaponName, icon="HIDE_OFF" if self.show else "HIDE_ON")
        row = layout.row(align=True)
        self.drawInPanel(row)

    def drawInPanel(self, layout: bpy.types.UILayout):
        layout.prop(self, 'show', text="", icon="HIDE_OFF" if self.show else "HIDE_ON")
        layout.label(text=self.weaponName)
        layout.prop(self, 'anchorSpine', text='')
        layout.prop(self, 'anchor1', text='')
    def updateWeapon(self):
        weaponObject = bpy.data.objects.get('Mockup_' + self.weaponName)
        if weaponObject == None:
            return
        weaponObject.hide_set(not self.show)

        self.weapons.remove(self.activeWeapon)
        self.activeWeapon = min(self.activeWeapon, max(0, len(self.weapons) - 1))



@collection.add()
class Character(bpyUtils.PropertyGroup):
    element: bpy.props.PointerProperty(type = bpy.types.Collection)
    def drawInList(self, layout: bpy.types.UILayout, index):
        text = self.element.name if self.element else 'None'
        layout.label(text=f'{index}: {text}')
    def drawInListActive(self, layout: bpy.types.UILayout, index: int):
        layout.prop(self, 'element', text=str(index))

@collection.add()
class ArmatureControllersData(bpyUtils.PropertyGroup):    
    nlaObject: bpy.props.PointerProperty(
        name = "Object with NLA", 
        type = bpy.types.Object,
        poll = lambda s, o: o.type == 'ARMATURE')
    deformArmature: bpy.props.PointerProperty(
        name = "Deform Armature", 
        type = bpy.types.Object,
        poll = lambda s, o: o.type == 'ARMATURE')

    showInside: bpy.props.BoolProperty(
        name = "Hide Inside", 
        default = False)

    #Melee
    ikMeleeShow: bpy.props.BoolProperty(default = True, options = set(), update = lambda self, _: self.updateWeapon('Melee'))
    ikMeleeAnchorSpine: bpy.props.FloatProperty(name = "IK Melee Anchor Spine", default = 1, soft_min = 0, soft_max = 1)
    ikMeleeAnchor1: bpy.props.FloatProperty(name = "IK Melee Anchor 1", default = 1, soft_min = 0, soft_max = 1)

    #Pistol
    ikPistolShow: bpy.props.BoolProperty(default = True, options = set(), update = lambda self, _: self.updateWeapon('Pistol'))
    ikPistolAnchorSpine: bpy.props.FloatProperty(name = "IK Pistol Anchor Spine", default = 1, soft_min = 0, soft_max = 1)
    ikPistolAnchor1: bpy.props.FloatProperty(name = "IK Pistol Anchor 1", default = 1, soft_min = 0, soft_max = 1)
    
    #SMG
    ikSMGShow: bpy.props.BoolProperty(default = True, options = set(), update = lambda self, _: self.updateWeapon('SMG'))
    ikSMGAnchorSpine: bpy.props.FloatProperty(name = "IK SMG Anchor Spine", default = 1, soft_min = 0, soft_max = 1)
    ikSMGAnchor1: bpy.props.FloatProperty(name = "IK SMG Anchor 1", default = 1, soft_min = 0, soft_max = 1)
    
    #Shotgun
    ikShotgunShow: bpy.props.BoolProperty(default = True, options = set(), update = lambda self, _: self.updateWeapon('Shotgun'))
    ikShotgunAnchorSpine: bpy.props.FloatProperty(name = "IK Shotgun Anchor Spine", default = 1, soft_min = 0, soft_max = 1)
    ikShotgunAnchor1: bpy.props.FloatProperty(name = "IK Shotgun Anchor 1", default = 1, soft_min = 0, soft_max = 1)

    #Rifle
    ikRifleShow: bpy.props.BoolProperty(default = True, options = set(), update = lambda self, _: self.updateWeapon('Rifle'))
    ikRifleAnchorSpine: bpy.props.FloatProperty(name = "IK Rifle Anchor Spine", default = 1, soft_min = 0, soft_max = 1)
    ikRifleAnchor1: bpy.props.FloatProperty(name = "IK Rifle Anchor 1", default = 1, soft_min = 0, soft_max = 1)

    ikHandLAnchor1: bpy.props.FloatProperty(name = "Hand.L Anchor 1", default = 0, soft_min = 0, soft_max = 1)
    ikHandRAnchor1: bpy.props.FloatProperty(name = "Hand.R Anchor 1", default = 0, soft_min = 0, soft_max = 1)

    characters: collection.createListProperty(Character)

    characterIndex: bpy.props.IntProperty(name = "Character", options = set(), default = 0, update=lambda self, _: self.showCharacter())

    weapons: collection.createListProperty(WeaponData)
    ikConfig: bpy.props.PointerProperty(type = IkConfigBones.IkConfig)
    renameActionProps: bpy.props.PointerProperty(type = renameActionProps.DataForRenameProperties)

    def poll(self):
        return bpy.context.object \
            and isinstance(bpy.context.object.data, bpy.types.Armature)
        
    def drawInPanel(self, layout: bpy.types.UILayout):
        #layout.use_property_split = True
        
        layout.prop(self, 'nlaObject')
        layout.prop(self, 'deformArmature')
        layout.prop(self, 'ikHandLAnchor1')
        layout.prop(self, 'ikHandRAnchor1')
        weapons = ['Melee', 'Pistol', 'SMG', 'Shotgun', 'Rifle']
        row = layout.row(align=True)
        row.label(text='↓Weapon')
        row.label(text='↓Anchor Spine')
        row.label(text='↓Anchor 1')
        def prop(weapon: str):
            row = layout.row(align=True)
            #row.label(text=weapon)
            status = getattr(self, 'ik' + weapon + 'Show', False)
            row.prop(self, 'ik' + weapon + 'Show', text=weapon, icon="HIDE_OFF" if status else "HIDE_ON")
            row.prop(self, 'ik' + weapon + 'AnchorSpine', text='')
            row.prop(self, 'ik' + weapon + 'Anchor1', text='')
        for weapon in weapons:
            prop(weapon)
        self.buttonFunction(layout, lambda: copyRestPose.copyRestPose(self.deformArmature, self.nlaObject), text="Copy Reset Pose")
        
    
    def updateWeapon(self, weaponName:str):
        status = getattr(self, 'ik' + weaponName + 'Show', False)
        weaponObject = bpy.data.objects.get('Mockup_' + weaponName)
        if weaponObject == None:
            print(f'No object "Mockup_{weaponName}"')
        weaponObject.hide_set(not status)

    def hideAllCharacters(self):
        ch: 'Character' = None
        for ch in self.characters.elements:
            ch.element.hide_viewport = True
    
    def showCharacter(self):
        index = self.characterIndex
        count = len(self.characters.elements)
        if count == 0:
            return
        while index < 0:
            index = index + count
        if index >= count:
            index = index % count
        self.characterIndex = index
        currentCharacter = self.characters.elements[index].element
        if currentCharacter == None:
            return
        self.hideAllCharacters()
        currentCharacter.hide_viewport = False


ACDPointer = bpyUtils.Pointer(bpy.types.Object, "object.da_armature_controllers_data", ArmatureControllersData)
ACDPointer.createPanel('Armature Controllers Data', DAView3D())
ACDPointer.createPanel('Weapons', DAView3D, lambda self, layout: self.weapons.drawInPanel(layout))
ACDPointer.createPanel('Characters', DAView3D, lambda self, layout: \
    layout.prop(self, 'characterIndex') or \
    self.characters.drawInPanel(layout))
# ACDPointer.createPanel('Ik Config', DAView3D, lambda self, layout: self.ikConfig.drawIkConfig(layout, self.nlaObject))

ACDPointer.createPanel('Rename Property in Action ', DAView3D, lambda self, layout: self.renameActionProps.drawWithButtons(layout, self.nlaObject))


#if sys.platform == 'win32':    
import copyUV
class DAImageEditor(bpyUtils.Panel):
    bl_space_type = "IMAGE_EDITOR"
    bl_region_type = "UI"
    bl_category = "DA"
collection.addModule(copyUV)
copyUV.CopyUvDataPointer.createPanel('Copy UV', DAImageEditor)
