require("LuaFramework/Global")

local _transform = self.transform
local _button
function Awake()
    local ui = QP.UIManager.Instance
    _button = UE.GameObject.Find("Button"):GetComponent(typeof(UI.Button))
    _button.onClick:AddListener(onClick)
end
function onClick()
    GameMgr.GetSingle("Module").OverScene("GameBox", "GameBox_Main")
end
