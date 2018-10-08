#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class QPFrameworkModuleWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(QP.Framework.Module);
			Utils.BeginObjectRegister(type, L, translator, 0, 5, 3, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CheckAndDownload", _m_CheckAndDownload);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CheckUpdate", _m_CheckUpdate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Download", _m_Download);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StopDownload", _m_StopDownload);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "JumpScene", _m_JumpScene);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Name", _g_get_Name);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Size", _g_get_Size);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SizeStr", _g_get_SizeStr);
            
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					QP.Framework.Module gen_ret = new QP.Framework.Module();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to QP.Framework.Module constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CheckAndDownload(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                QP.Framework.Module gen_to_be_invoked = (QP.Framework.Module)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _tableName = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.CheckAndDownload( _tableName );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<QP.Framework.DownloadTable>(L, 2)) 
                {
                    QP.Framework.DownloadTable _table = (QP.Framework.DownloadTable)translator.GetObject(L, 2, typeof(QP.Framework.DownloadTable));
                    
                    gen_to_be_invoked.CheckAndDownload( _table );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to QP.Framework.Module.CheckAndDownload!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CheckUpdate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                QP.Framework.Module gen_to_be_invoked = (QP.Framework.Module)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    string _tableName = LuaAPI.lua_tostring(L, 2);
                    bool _isGetSize = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.CheckUpdate( _tableName, _isGetSize );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<QP.Framework.CheckUpdateTable>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    QP.Framework.CheckUpdateTable _table = (QP.Framework.CheckUpdateTable)translator.GetObject(L, 2, typeof(QP.Framework.CheckUpdateTable));
                    bool _isGetSize = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.CheckUpdate( _table, _isGetSize );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to QP.Framework.Module.CheckUpdate!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Download(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                QP.Framework.Module gen_to_be_invoked = (QP.Framework.Module)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _tableName = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.Download( _tableName );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<QP.Framework.DownloadTable>(L, 2)) 
                {
                    QP.Framework.DownloadTable _table = (QP.Framework.DownloadTable)translator.GetObject(L, 2, typeof(QP.Framework.DownloadTable));
                    
                    gen_to_be_invoked.Download( _table );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to QP.Framework.Module.Download!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StopDownload(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                QP.Framework.Module gen_to_be_invoked = (QP.Framework.Module)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.StopDownload(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_JumpScene(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                QP.Framework.Module gen_to_be_invoked = (QP.Framework.Module)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)&& (LuaAPI.lua_isnil(L, 5) || LuaAPI.lua_type(L, 5) == LuaTypes.LUA_TSTRING)) 
                {
                    string _sceneName = LuaAPI.lua_tostring(L, 2);
                    bool _isAsync = LuaAPI.lua_toboolean(L, 3);
                    bool _isUnloadOtherAssetBundle = LuaAPI.lua_toboolean(L, 4);
                    string _tableName = LuaAPI.lua_tostring(L, 5);
                    
                    gen_to_be_invoked.JumpScene( _sceneName, _isAsync, _isUnloadOtherAssetBundle, _tableName );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 5&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)&& translator.Assignable<QP.Framework.SceneTable>(L, 5)) 
                {
                    string _sceneName = LuaAPI.lua_tostring(L, 2);
                    bool _isAsync = LuaAPI.lua_toboolean(L, 3);
                    bool _isUnloadOtherAssetBundle = LuaAPI.lua_toboolean(L, 4);
                    QP.Framework.SceneTable _table = (QP.Framework.SceneTable)translator.GetObject(L, 5, typeof(QP.Framework.SceneTable));
                    
                    gen_to_be_invoked.JumpScene( _sceneName, _isAsync, _isUnloadOtherAssetBundle, _table );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to QP.Framework.Module.JumpScene!");
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Name(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                QP.Framework.Module gen_to_be_invoked = (QP.Framework.Module)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.Name);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Size(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                QP.Framework.Module gen_to_be_invoked = (QP.Framework.Module)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushint64(L, gen_to_be_invoked.Size);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SizeStr(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                QP.Framework.Module gen_to_be_invoked = (QP.Framework.Module)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.SizeStr);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
