require ("LuaFramework/G_CS")
local JumpSceneMgr = require("LuaFramework/Common/JumpSceneMgr")

local _transform=self.transform
local _button
function Awake()
    local ui=QP.UIManager.Instance;
    _button=UNITY.GameObject.Find("Button"):GetComponent(typeof(UI.Button))
    _button.onClick:AddListener(onClick)

end
function onClick()
    JumpSceneMgr.Over("GameBox")
end