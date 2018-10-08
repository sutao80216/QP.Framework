require ("LuaFramework/G_CS")
local Module = require("LuaFramework/Public/Module")
function Awake()
    local back = UNITY.GameObject.Find("Back"):GetComponent(typeof(UI.Button))
    back.onClick:AddListener(onClick)
end

function onClick()
    Module.OverScene("Game1","Game1_Game")
end
