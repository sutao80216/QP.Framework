require ("LuaFramework/G_CS")

local _transform=self.transform
local _gameObject=self.gameObject

local _layout 
local modules={
    "Game1",
    "Game2"
}
function Awake()
    _layout=_transform:Find("Layout")
    for i,v in ipairs(modules) do
        local prefab = QP.ResMgr.Instance:GetPrefab("GameBox","GameItem","_Panel") 
        local go =UNITY.GameObject.Instantiate(prefab)
        go.transform:SetParent(_layout.transform)
        go.transform.localPosition=UNITY.Vector3.zero
        go.transform.localScale=UNITY.Vector3.one
        go.transform.localRotation=UNITY.Quaternion.identity
        local script = QP.LuaEnvMgr.Instance:Create(go,"GameBox/GameItem")
        script.Table.Init(v) 
    end

    local _button=_transform:Find("setting"):GetComponent(typeof(UI.Button))
    _button.onClick:AddListener(function()
        local script = QP.UIManager.Instance:ShowPanel("GameBox","Panel1","_Panel",QP.CanvasType.Top)
    end)
end