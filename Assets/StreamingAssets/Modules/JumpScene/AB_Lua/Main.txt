require ("LuaFramework/G_CS")
require ("LuaFramework/G_Global")
local JumpSceneMgr = require("LuaFramework/Common/JumpSceneMgr")

local _slider
local _text
function Awake()
    _slider = UNITY.GameObject.Find("Slider"):GetComponent(typeof(UI.Slider))
    _text=UNITY.GameObject.Find("Text"):GetComponent(typeof(UI.Text))
    print("Target Module->",__TARGET_MODULE__,"Target Scene->",__TARGET_SCENE__)
    JumpSceneMgr.Jump(__TARGET_MODULE__,__TARGET_SCENE__,OnProgress)
end

function OnProgress(module,progress)
    _slider.value=progress
    _text.text=math.floor(progress*100).."%"

end
