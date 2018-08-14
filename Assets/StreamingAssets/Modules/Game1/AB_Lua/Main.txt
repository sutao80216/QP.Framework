require ("LuaFramework/G_CS")
local JumpSceneMgr = require("LuaFramework/Common/JumpSceneMgr")
function Awake()
    local back = UNITY.GameObject.Find("Back"):GetComponent(typeof(UI.Button))
    back.onClick:AddListener(onClick)
end

function onClick()
    JumpSceneMgr.Over("Game1","Game")
end
