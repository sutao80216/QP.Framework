require ("LuaFramework/G_CS")
local Module = require("LuaFramework/Public/Module")

local _transform=self.transform
local _button
function Awake()
    print("新修改")
    local ui=QP.UIManager.Instance;
    _button=UNITY.GameObject.Find("Button"):GetComponent(typeof(UI.Button))
    _button.onClick:AddListener(onClick)

end
function onClick()
    Module.OverScene("GameBox","GameBox_Main")
end