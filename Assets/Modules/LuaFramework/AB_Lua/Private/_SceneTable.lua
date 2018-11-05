-- [[禁止require]]
require ("LuaFramework/G_CS")

_SceneTable={}
local this=_SceneTable

local eventTables={}
function this.AddEvent(moduleName,eventTable)
    eventTables[moduleName]=eventTable;
end
function this.RemEvent(moduleName)
    if eventTables[moduleName]==nil then return end
    eventTables[moduleName]=nil;
    Log.Info(moduleName,eventTables)
end

function this.Progress(moduleName,progress)
    if eventTables[moduleName]==nil then return end
    if eventTables[moduleName].Progress==nil then return end
    eventTables[moduleName].Progress(moduleName,progress)
end

function this.Complete(moduleName)
    if eventTables[moduleName]==nil then return end
    if eventTables[moduleName].Complete==nil then return end
    eventTables[moduleName].Complete(moduleName)
    this.RemEvent(moduleName)
end
function this.Error(moduleName)
    if eventTables[moduleName]==nil then return end
    if eventTables[moduleName].Error==nil then return end
    eventTables[moduleName].Error(moduleName)
    this.RemEvent(moduleName)
end
