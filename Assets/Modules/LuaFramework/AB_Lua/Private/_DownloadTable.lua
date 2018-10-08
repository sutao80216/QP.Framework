-- [[禁止require]]
require ("LuaFramework/G_CS")
require("LuaFramework/G_Log")
_DownloadTable={}
local this=_DownloadTable

local eventTables={}
function this.AddEvent(moduleName,eventTable)
    eventTables[moduleName]=eventTable;
end
function this.RemEvent(moduleName)
    if eventTables[moduleName]==nil then return end
    eventTables[moduleName]=nil;
end
function this.Befor(moduleName,downloadTotal)
    if eventTables[moduleName]==nil then return end
    if eventTables[moduleName].Befor==nil then return end
    eventTables[moduleName].Befor(moduleName,downloadTotal)
end
function this.Progress(moduleName,result)
    if eventTables[moduleName]==nil then return end
    if eventTables[moduleName].Progress==nil then return end
    eventTables[moduleName].Progress(moduleName,result)
end

function this.OneComplete(moduleName,downloadedCount,downloadTotal)
    if eventTables[moduleName]==nil then return end
    if eventTables[moduleName].OneComplete==nil then return end
    eventTables[moduleName].OneComplete(moduleName,downloadedCount,downloadTotal)
end

function this.AllComplete(moduleName)
    if eventTables[moduleName]==nil then return end
    if eventTables[moduleName].AllComplete==nil then return end
    eventTables[moduleName].AllComplete(moduleName)
    this.RemEvent(moduleName)
end

function this.Error(moduleName)
    if eventTables[moduleName]==nil then return end
    if eventTables[moduleName].Error==nil then return end
    eventTables[moduleName].Error(moduleName)
    this.RemEvent(moduleName)
end