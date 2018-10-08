local Debug=CS.UnityEngine.Debug
local openLog=true
Log={}
local this=Log
function this.Info(...)
    if openLog~=true then return end
    local msg=this.GetMessage(...)
    Debug.Log("Info : "..msg)
end
function this.Warning(...)
    if openLog~=true then return end
    local msg=this.GetMessage(...)
    Debug.LogWarning("Warning : "..msg)
end
function this.Error(...)
    if openLog~=true then return end
    local msg=this.GetMessage(...)
    Debug.LogError("Error : "..msg)
end
function this.GetMessage(...)
    local msg=""
    for k,v in ipairs({...}) do
        if v==nil then
            msg=msg.."<nil>" .."\n"
        elseif type(v)=="table" then
            msg=msg..this.GetTableMsg(v,0).."\n"
        elseif type(v)=="function" then
            msg=msg.."[function]" .."\n"
        elseif type(v)=="userdata" then
            msg=msg.."[userdata]" .."\n"
        else
            msg=msg..v.."\n"
        end
    end
    return msg
end
function this.GetTableMsg(tab,space)
    local space1=""
    local space2=""
    for i=1,space do
        space1=space1.."    "
    end
    space2=space1.."    "
    local msg="{"
    for k,v in pairs(tab) do
        if v==nil then
            msg=msg.."<nil>" .."\n"
        elseif type(v)=="table" then
            msg=msg.."\n"..space2..k.." : "..this.GetTableMsg(v,space+1)
        elseif type(v)=="function" then
            msg=msg.."\n"..space2..k.." : [function]"
        elseif type(v)=="userdata" then
            msg=msg.."\n"..space2..k.." : {userdata}"
        else
            Debug.LogError(type(v))
            msg=msg.."\n"..space2..k.." : "..v
        end
    end
    msg=msg.."\n"..space1.."}"
    return msg
end