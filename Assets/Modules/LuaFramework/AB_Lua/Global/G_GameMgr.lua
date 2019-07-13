--[[
    管理单例 添加/获取
    创建预设体并添加Lua脚本
]]
GameMgr = {}
local _singles = {}
local this=GameMgr
function this.Init()
    _singles = {}
end
--[[
    添加单例
]]
function this.AddSingle(path, name,notDestroy)
    if _singles[name] ~= nil then
        return
    end
    local go=UE.GameObject(path)
    _singles[name] = QP.LuaEnvMgr.Instance:CreateSingle(go, path).Table
    if notDestroy==true then 
        UE.Object.DontDestroyOnLoad(go)
    end
    if _singles[name].Init ~= nil then
        _singles[name].Init()
    end
end
--[[
    获取单例
]]
function this.GetSingle(name)
    return _singles[name]
end

--[[
    给预设体添加lua脚本
    返回luaTable
]]
function this.AddScript(obj, path)
    if obj == nil then
        Log.Error("GameMgr.AddScript 参数obj=nil",path)
        return
    end
    if path == nil or path == "" then
        Log.Error("GameMgr.AddScript 参数path=nil",obj)
        return
    end
    local tab = QP.LuaEnvMgr.Instance:CreateScript(obj, path).Table
    if tab.Init ~= nil then
        tab.Init()
    end
    return tab
end
function this.CreateScript(prefab, path)
    if prefab == nil then
        Log.Error("GameMgr.CreateScript 参数prefab=nil",path)
        return
    end
    if path == nil or path == "" then
        Log.Error("GameMgr.CreateScript 参数path=nil",prefab)
        return
    end
    local obj = UE.GameObject.Instantiate(prefab)
    return this.AddScript(obj, path)
end
--[[
    退出游戏时必须要手动调用OnDestroy!!!!!!
]]
function this.OnDestroy()
    _singles = {}
end