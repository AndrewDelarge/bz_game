import sys
import os
import bpy
from pathlib import Path

def findGitRotDir():
    path = Path(bpy.data.filepath)
    while path != path.parent:
        gitDir = path / '.git'
        if gitDir.is_dir():
            return path
        path = path.parent
    return None
 
def findScripts():
    gitRootDir = findGitRotDir()
    if gitRootDir == None:
        return
    scripts_dir = gitRootDir / 'scripts'
    if not scripts_dir.is_dir():
        return
    scripts_dir = str(scripts_dir)
    if scripts_dir not in sys.path:
       sys.path.append(scripts_dir)

def loadMain():
    try:
        import loader
        loader.reload()
    except ImportError:
        findScripts()
        import loader
        loader.firstLoad()
        
if __name__ == '__main__':
    loadMain()