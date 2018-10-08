require ("LuaFramework/G_CS")
local Module=require("LuaFramework/Public/Module")
--[[
    一切逻辑从这里开始
]]
function Main()
    print("一切逻辑从这里开始")
    -- Modules 必须先下载 主要是为了获取Modules.manifest
    StartUp("Modules",function(moduleName)
        -- 之后下载Loading模块 为了显示进度 
        StartUp("Loading",function(moduleName)
            -- 先跳转到Loading模块主场景Loading_Main 执行Loading模块的Main.lua 由它处理接下来的事情
            Module.JumpScene(moduleName,"Loading_Main",false,true,nil)
        end)
    end)
end
function StartUp(moduleName,complete)
    -- 例1 使用lua封装的方法 全局Table监听
    -- Module.CheckAndDownload(moduleName,{
    --     AllComplete=function(moduleName)
    --         complete(moduleName)
    --     end
    -- })

    -- 例2 调用C#方法 传入对应委托类型的table 
    local module =QP.ModuleMgr.Instance:GetModule(moduleName)
    module:CheckAndDownload({
        AllComplete=function(moduleName)
            complete(moduleName)
        end
    })
end



