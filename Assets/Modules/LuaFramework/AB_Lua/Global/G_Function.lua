--[[
    如果给定的table不是一个table 
    返回一个空的table
]]
function IsTable(table)
    if type(table)~="table" then 
        table={}
    end
    return table
end
--[[
    获取table的长度
    table的key可以是任意的
    如果value=nil也会计算在内
]]
function GetTableLength(table)
    local length=0
    local t=IsTable(table)
    for k,v in pairs(t) do
        length=length+1
    end
    return length
end
--[[
    删除table中指定key的元素
]]
function RemoveByKey(tab,key)
    local t=IsTable(tab)
    local temp={}
    for k,v in pairs(t) do
        table.insert(temp,k)
    end
    local newTable={}
    local i=1
    while i<#temp do
        local val=temp[i]
        if val==key then
            table.remove( temp,i)
        else
            newTable[val]=t[val]
            i=i+1
        end
    end
    return newTable
end
--[[
    检查一个值是否在table中
]]
function ContainsValue(tab,value)
    local t=IsTable(tab)
    for k,v in pairs(t) do
        if v==value then return true end
    end
    return false
end

--[[
    从table中删除指定value
    table的key必须是连续的1，2，3，4
]]
function RemoveByValue(tab,value)
    local t=IsTable(tab)
    for i,v in ipairs(t) do
        if v==value then 
            table.remove( tab, i)
            return true
        end
    end
    return false
end