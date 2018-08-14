require ("LuaFramework/G_CS")

function Awake()
	local go=CS.UnityEngine.GameObject.Find("LoadingPanel")
	local script = QP.LuaEnvMgr.Instance:Create(go,"Loading/LoadingPanel")
end


function OnDestroy()
end






