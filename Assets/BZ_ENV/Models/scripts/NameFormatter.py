import typing

class CharTable:
    def __init__(self, count:int, chars:typing.List[str]):
        char_table = [False] * count
        for char in chars:
            begin = ord(char[0])
            end = ord(char[2]) + 1 if len(char) == 3 and char[1] == '-' else begin + 1
            for char_index in range(begin, end):
                char_table[char_index] = True
        self.char_table = char_table
        self.pattern = '[' + (''.join(chars)) + ']'
    def check_char(self, ch:str):
        char_code = ord(ch)
        return char_code < len(self.char_table) and self.char_table[char_code]

def _createCharReplaceTable() -> typing.List[str]:
    replace_table: typing.List[str] = [''] * 127
    replace_table[ord('_')] = '_'
    for i in range(ord('A'), ord('Z')):
        replace_table[i] = '_' + chr(i - ord('A') + ord('a'))
    for i in range(ord('a'), ord('z')):
        replace_table[i] = chr(i)
    for i in range(ord('0'), ord('9')):
        replace_table[i] = chr(i)
    return replace_table

_firstCharOfName = CharTable(127, ['_', 'a-z'])
_charOfName = CharTable(127, ['_', '0-9', 'a-z'])
_replaceTable: typing.List[str] = _createCharReplaceTable()


def checkName(name:str, forbidFirstCharToBeANumber = True):
    if forbidFirstCharToBeANumber:
        if not _firstCharOfName.check_char(name[0]):
            raise Exception(f"Bad name #1. Name:'{name}'. Bad first char. Name should be like: [_a-z][_a-z0-9]*")
        for ch in name[1:]:
            if not _charOfName.check_char(ch):
                raise Exception(f"Bad name #2. Name:'{name}'. Bad inner char. Name should be like: [_a-z][_a-z0-9]*")
    else:
        for ch in name:
            if not _charOfName.check_char(ch):
                raise Exception(f"Bad name #3. Name:'{name}' Bad inner char. Name should be like: [_a-z0-9]+")

def nameModification(name: str) -> str:
    pref = '_'
    newName = ''
    for i in name:
        charIndex = ord(i)
        if charIndex > len(_replaceTable):
            continue
        _s = _replaceTable[charIndex]
        if len(_s) == 0:
            continue
        if len(_s) > 1 and _s[0] == '_' and pref == '_':
            _s = _s[1:]
        newName += _s
        pref = _s
    return newName

def generatePanelName(prefix:str, name: str):
    return prefix.upper() + '_PT_' + name

def test():
    line = 68
    test_checkNames_Data = {
        "da123": True,
        "123da": False,
        "Da": False,
        "DA": False,
        "_da123": True,
        "_123da": True,
        "d_a": True,
        "Привет": False,
        "aаcсeе": False,
        "aаcсeеTТ": False,
        "_Привет": False,
        "_aаcсeеTТ": False,
        "aаcс_eеTТ": False,
    }

    for p, expect in test_checkNames_Data.items():  
        try:
            checkName(p)
            if expect == False:
                print(f"_test_checkName1 #{line} \"{p}\" expect Error, get Ok")
        except:
            if expect == True:
                print(f"_test_checkName1 #{line} \"{p}\" expect Ok, get Error")
        line = line + 1

    line = 95
    test_checkNames_Data = {
        "da123": True,
        "123da": True,
        "Da": False,
        "DA": False,
        "_da123": True,
        "_123da": True,
        "d_a": True,
        "Привет": False,
        "aаcсeе": False,
        "aаcсeеTТ": False,
        "_Привет": False,
        "_aаcсeеTТ": False,
        "aаcс_eеTТ": False,
    }

    for param, expect in test_checkNames_Data.items():
        try:
            checkName(param, False)
            if expect == False:
                print(f"_test_checkName2 #{line} \"{param}\" expect Error, get Ok")
        except:
            if expect == True:
                print(f"_test_checkName2 #{line} \"{param}\" expect Ok, get Error")
        line = line + 1

    line = 123
    test_modif_data = {
        "Hello": "hello",
        "hello": "hello",
        "_hello": "_hello",
        "hello_": "hello_",
        "HelloWorld": "hello_world",
        "helloWorld": "hello_world",
        "hello_world": "hello_world",
        "hello_World": "hello_world",
        "_hello_world": "_hello_world",
        "hello_world_": "hello_world_",
        "HelloMyWorld": "hello_my_world",
        "Hello_My_World": "hello_my_world",
        "_HelloMyWorld": "_hello_my_world",
        "Hello_myWorld": "hello_my_world"
    }

    for param, expect in test_modif_data.items():
        s = nameModification(param)
        if s != expect:
            print(f"test_preffix #{line} \"{param}\" expect: \"{expect}\", get: \"{s}\"")
        line = line + 1

if __name__ == "__main__":
    test()