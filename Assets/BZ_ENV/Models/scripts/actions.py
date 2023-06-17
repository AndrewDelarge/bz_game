import bpy
import json

class Keyframe:
    def scan(self, keyframe: bpy.types.Keyframe):
        pass
    def generate(self, keyframe: bpy.types.Keyframe):
        pass

class FCurve:
    def __init__(self) -> None:
        self.data_path: str = ''
        self.index: int = -1
        self.keyframes: list[Keyframe] = [] 
    def scan(self, fcurve: bpy.types.FCurve):
            self.data_path = fcurve.data_path
            self.index = fcurve.array_index
    def generate(self, action: bpy.types.Action):
        count = len(self.keyframes)
        fcurve = action.fcurves.new(self.data_path, index=self.index)
        fcurve.keyframe_points.add(count=count)
        index = 0
        while index < count:
            self.keyframes[index].generate(fcurve.keyframe_points[index])
            index += 1

class ActionData:
    def __init__(self) -> None:
        self.name: str = ''
        self.fcurves: list[FCurve] = []
    def load(self, fromData: dict):
        pass
    def save(self, toData: dict):
        pass
    def scan(self, action: bpy.types.Action):
        self.name: str = action.name
        for fc in action.fcurves:
            nFc = FCurve()
            nFc.scan(fc)
            self.fcurves.append(nFc)
        pass
    def generate(self):
        action = bpy.data.actions.new(self.name)
        count = len(self.fcurves)
        index = 0
        while index < count:
            self.fcurves[index].generate(action)
            index += 1
        return action

