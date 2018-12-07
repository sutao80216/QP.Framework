require ("LuaFramework/Global")

function Awake()
	local go=CS.UnityEngine.GameObject.Find("LoadingPanel")
	local script=GameMgr.AddScript(go,"Loading/LoadingPanel")
end
function OnDestroy()
end