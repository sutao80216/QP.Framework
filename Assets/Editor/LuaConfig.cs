using QP.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;
/// <summary>
/// 标签配置
/// </summary>
public static class LuaConfig {

    [LuaCallCSharp]
    public static List<Type> luaCallCSharpList = new List<Type>()
    {
        typeof(Module),
        typeof(ResMgr),
        typeof(SceneMgr),
        typeof(UIManager),
        typeof(LuaEnvMgr),
        typeof(ModuleMgr),

        typeof(ShowType),
        typeof(PanelStatus),
        typeof(CanvasType),
    };

    [CSharpCallLua]
    public static List<Type> csharpCallLuaList = new List<Type>()
    {
        typeof(DInt),
        typeof(DVoid),
        typeof(DString),
        typeof(DString_Int),
        typeof(DString_Float),
        typeof(DString_Int_Int),
        typeof(DString_Int_String),
        typeof(DString_Float_Float),
        typeof(DString_SDownloadFileResult),

    };
}
