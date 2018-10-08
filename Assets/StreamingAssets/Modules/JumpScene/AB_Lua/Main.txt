require ("LuaFramework/G_CS")
require ("LuaFramework/G_Global")
local Module = require("LuaFramework/Public/Module")

local _slider
local _text
function Awake()
    _slider = UNITY.GameObject.Find("Slider"):GetComponent(typeof(UI.Slider))
    _text=UNITY.GameObject.Find("Text"):GetComponent(typeof(UI.Text))
end
function Start()
    print("目标模块->",__TARGET_MODULE__," 目标场景->",__TARGET_SCENE__)
    Module.JumpScene(__TARGET_MODULE__,__TARGET_SCENE__,true,true,{
        Progress=OnProgress,
        Complete=OnComplete,
        Error=OnError
    })
end
function OnProgress(module,progress)
    _slider.value=progress
    _text.text=math.floor(progress*100).."%"
end
function OnComplete(module)
    -- print(module,"过度场景完成！")
end
function OnError(module)
    -- print(module,"过度场景失败！！")
end