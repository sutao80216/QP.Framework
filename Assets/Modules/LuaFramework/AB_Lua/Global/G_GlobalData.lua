-- 过度场景
__OVER_MODULE__="JumpScene"
__OVER_SCENE__="JumpScene_Main"

-- 下一个目标场景
__TARGET_MODULE__ =""
__TARGET_SCENE__ =""
function G_SetTargetModule(value)
    __TARGET_MODULE__=value
end
function G_SetTargetScene(value)
    __TARGET_SCENE__=value
end