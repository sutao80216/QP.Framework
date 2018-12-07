
require ("LuaFramework/Global")

local _transform=self.transform
local _gameObject=self.gameObject
function Awake()
   local _button=_transform:Find("Image/Button"):GetComponent(typeof(UI.Button))
   _button.onClick:AddListener(function()
        QP.UIManager.Instance:CloseAll(QP.CanvasType.Top)
   end)
end