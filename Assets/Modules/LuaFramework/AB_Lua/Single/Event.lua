--[[
    事件管理器
    负责发送网络消息事件和UI事件
]]
local _events = {}
function Init()
    _events = {}
end
function AddEvent(eventTable, code, callback)
    local tableKey = tostring(eventTable)
    if _events[tableKey] == nil then
        _events[tableKey] = {}
    end
    if _events[tableKey][code] == nil then
        _events[tableKey][code] = {}
    end
    table.insert(_events[tableKey][code], callback)
end
function RemoveEvent(eventTable, code, callback)
    local tableKey = tostring(eventTable)
    if _events[tableKey] == nil then
        return
    end
    if _events[tableKey][code] == nil then
        return
    end

    for i, v in ipairs(_events[tableKey][code]) do
        if callback == v then
            table.remove(_events[tableKey][code], i)
            break
        end
    end
end
function Execute(eventTable, code, data)
    local tableKey = tostring(eventTable)
    if _events[tableKey] == nil then
        return
    end
    if _events[tableKey][code] == nil then
        return
    end
    for i, v in ipairs(_events[tableKey][code]) do
        if v ~= nil then
            v(data)
        end
    end
end

function OnDestroy()
    _events = {}
end
