require ("LuaFramework/Global")

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
        local go =UE.GameObject.Instantiate(prefab)
        go.transform:SetParent(_layout.transform)
        go.transform.localPosition=UE.Vector3.zero
        go.transform.localScale=UE.Vector3.one
        go.transform.localRotation=UE.Quaternion.identity
        local script= GameMgr.AddScript(go,"GameBox/GameItem")
        script.Set(v) 
    end

    local _button=_transform:Find("setting"):GetComponent(typeof(UI.Button))
    _button.onClick:AddListener(function()
        local script = QP.UIManager.Instance:ShowPanel("GameBox","Panel1","_Panel",QP.CanvasType.Top)
    end)
end