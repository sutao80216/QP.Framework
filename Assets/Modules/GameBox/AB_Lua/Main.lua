require ("LuaFramework/Global")
local _transform=self.transform
local _checkForUpdateText

function Awake()
   local script = QP.UIManager.Instance:ShowPanel("GameBox","GameBoxPanel","_Panel",QP.CanvasType.Normal)
end

