
require ("LuaFramework/G_CS")

local _transform=self.transform
local _gameObject=self.gameObject
function Awake()
   local _button=_transform:Find("Image/Button"):GetComponent(typeof(UI.Button))
   _button.onClick:AddListener(function()
        local script = QP.UIManager.Instance:ShowPanel(
            "GameBox",
            "Panel3",
            "_Panel",
            QP.CanvasType.Top,
            QP.ShowType.CloseLastAll
        )
   end)
end 