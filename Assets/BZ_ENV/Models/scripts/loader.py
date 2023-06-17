
def firstLoad():
    print("[firstLoad]--------------------")
    import bpyUtils
    collection = bpyUtils.ClassCollection.getCollection()
    import main
    collection.addModule(main)
    collection.register()
    print("--------------------")

def reload():
    print("[reload]--------------------")
    import bpyUtils
    import importlib
    collection = bpyUtils.ClassCollection.getCollection()
    modules = collection.getModules()
    collection.totalClear()

    importlib.reload(bpyUtils)
    collection = bpyUtils.ClassCollection.getCollection()
    for i in modules:
        print("# reload module", i.__name__)
        importlib.reload(i)
    import main
    collection.addModule(main)
    collection.register()
    print("--------------------")