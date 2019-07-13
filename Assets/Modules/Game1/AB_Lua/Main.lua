require ("LuaFramework/Global")
function Awake()
    local back = UE.GameObject.Find("Back"):GetComponent(typeof(UI.Button))
    back.onClick:AddListener(onClick)
end

function onClick()
    GameMgr.GetSingle("Module").OverScene("Game1","Game1_Game")
end
