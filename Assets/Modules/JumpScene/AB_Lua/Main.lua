require ("LuaFramework/Global")
local _slider
local _text
function Awake()
    _slider = UE.GameObject.Find("Slider"):GetComponent(typeof(UI.Slider))
    _text=UE.GameObject.Find("Text"):GetComponent(typeof(UI.Text))
    GameMgr.GetSingle("Event").AddEvent(SceneCmd,SceneCmd.Progress,Progress)
end
function Start()
    -- print("目标模块->",__TARGET_MODULE__," 目标场景->",__TARGET_SCENE__)
    GameMgr.GetSingle("Module").JumpScene(__TARGET_MODULE__,__TARGET_SCENE__,true,true)
end
function Progress(data)
    _slider.value=data.progress
    _text.text=math.floor(data.progress*100).."%"
end
function OnDestroy()
    GameMgr.GetSingle("Event").RemoveEvent(SceneCmd,SceneCmd.Progress,Progress)
end