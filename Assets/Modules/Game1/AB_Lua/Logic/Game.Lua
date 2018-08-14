require ("LuaFramework/G_CS")
local JumpSceneMgr = require("LuaFramework/Common/JumpSceneMgr")
function Awake()
    _button=UNITY.GameObject.Find("Button"):GetComponent(typeof(UI.Button))

    _button.onClick:AddListener(function()
        JumpSceneMgr.Over("GameBox")
    end)
end