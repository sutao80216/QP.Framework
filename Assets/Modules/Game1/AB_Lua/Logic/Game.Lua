require ("LuaFramework/Global")
function Awake()
    _button=UE.GameObject.Find("Button"):GetComponent(typeof(UI.Button))

    _button.onClick:AddListener(function()
        GameMgr.GetSingle("Module").OverScene("GameBox","GameBox_Main")
    end)
end