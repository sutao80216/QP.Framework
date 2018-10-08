require ("LuaFramework/G_CS")
local Module = require("LuaFramework/Public/Module")
function Awake()
    _button=UNITY.GameObject.Find("Button"):GetComponent(typeof(UI.Button))

    _button.onClick:AddListener(function()
        Module.OverScene("GameBox","GameBox_Main")
    end)
end