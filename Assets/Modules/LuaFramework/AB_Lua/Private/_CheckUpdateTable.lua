-- [[禁止require]]
require ("LuaFramework/G_CS")
_CheckUpdateTable={}
local this=_CheckUpdateTable

local eventTables={}

function this.AddEvent(moduleName,eventTable)
    eventTables[moduleName]=eventTable
end
function this.RemEvent(moduleName)
    if eventTables[moduleName]==nil then return end
    eventTables[moduleName]=nil
end


function this.Complete(moduleName,downloadCount,size)
    if eventTables[moduleName]==nil then return end
    if eventTables[moduleName].Complete==nil then return end
    eventTables[moduleName].Complete(moduleName,downloadCount,size)
    this.RemEvent(moduleName)
end

function this.Error(moduleName)
    if eventTables[moduleName]==nil then return end
    if eventTables[moduleName].Error==nil then return end
    eventTables[moduleName].Error(moduleName)
    this.RemEvent(moduleName)
end