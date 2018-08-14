require ("LuaFramework/G_CS")
local DownloadModuleMgr = require("LuaFramework/Common/DownloadModuleMgr")
local JumpSceneMgr = require("LuaFramework/Common/JumpSceneMgr")
--[[
    一切逻辑从这里开始
]]
function Main()
    -- 首先下载必要的模块
    DownloadModuleMgr.DownloadModule("Modules",function(module)
        DownloadModuleMgr.DownloadModule("Common",function(module)
            DownloadModuleMgr.DownloadModule("Loading",function(module)
                -- 跳转
                JumpSceneMgr.Jump(module,nil,nil,true)
            end)
        end)
    end)
end



