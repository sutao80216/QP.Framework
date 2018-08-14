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
    public class QPFrameworkResMgrWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(QP.Framework.ResMgr);
			Utils.BeginObjectRegister(type, L, translator, 0, 5, 1, 1);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InitAssetBundle", _m_InitAssetBundle);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ClearOtherModule", _m_ClearOtherModule);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetPrefab", _m_GetPrefab);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetTexture", _m_GetTexture);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAudio", _m_GetAudio);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Dependencies", _g_get_Dependencies);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "Dependencies", _s_set_Dependencies);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 1, 0);
			
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "Instance", _g_get_Instance);
            
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					QP.Framework.ResMgr gen_ret = new QP.Framework.ResMgr();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to QP.Framework.ResMgr constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitAssetBundle(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                QP.Framework.ResMgr gen_to_be_invoked = (QP.Framework.ResMgr)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _module = LuaAPI.lua_tostring(L, 2);
                    System.Action<float> _progress = translator.GetDelegate<System.Action<float>>(L, 3);
                    System.Action _complete = translator.GetDelegate<System.Action>(L, 4);
                    
                    gen_to_be_invoked.InitAssetBundle( _module, _progress, _complete );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClearOtherModule(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                QP.Framework.ResMgr gen_to_be_invoked = (QP.Framework.ResMgr)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _module = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.ClearOtherModule( _module );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetPrefab(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                QP.Framework.ResMgr gen_to_be_invoked = (QP.Framework.ResMgr)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TSTRING)) 
                {
                    string _module = LuaAPI.lua_tostring(L, 2);
                    string _prefabName = LuaAPI.lua_tostring(L, 3);
                    string _bundleName = LuaAPI.lua_tostring(L, 4);
                    
                        UnityEngine.GameObject gen_ret = gen_to_be_invoked.GetPrefab( _module, _prefabName, _bundleName );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)) 
                {
                    string _module = LuaAPI.lua_tostring(L, 2);
                    string _prefabName = LuaAPI.lua_tostring(L, 3);
                    
                        UnityEngine.GameObject gen_ret = gen_to_be_invoked.GetPrefab( _module, _prefabName );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to QP.Framework.ResMgr.GetPrefab!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetTexture(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                QP.Framework.ResMgr gen_to_be_invoked = (QP.Framework.ResMgr)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TSTRING)) 
                {
                    string _module = LuaAPI.lua_tostring(L, 2);
                    string _prefabName = LuaAPI.lua_tostring(L, 3);
                    string _bundleName = LuaAPI.lua_tostring(L, 4);
                    
                        UnityEngine.Texture2D gen_ret = gen_to_be_invoked.GetTexture( _module, _prefabName, _bundleName );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)) 
                {
                    string _module = LuaAPI.lua_tostring(L, 2);
                    string _prefabName = LuaAPI.lua_tostring(L, 3);
                    
                        UnityEngine.Texture2D gen_ret = gen_to_be_invoked.GetTexture( _module, _prefabName );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to QP.Framework.ResMgr.GetTexture!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAudio(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                QP.Framework.ResMgr gen_to_be_invoked = (QP.Framework.ResMgr)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TSTRING)) 
                {
                    string _module = LuaAPI.lua_tostring(L, 2);
                    string _prefabName = LuaAPI.lua_tostring(L, 3);
                    string _bundleName = LuaAPI.lua_tostring(L, 4);
                    
                        UnityEngine.AudioClip gen_ret = gen_to_be_invoked.GetAudio( _module, _prefabName, _bundleName );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)) 
                {
                    string _module = LuaAPI.lua_tostring(L, 2);
                    string _prefabName = LuaAPI.lua_tostring(L, 3);
                    
                        UnityEngine.AudioClip gen_ret = gen_to_be_invoked.GetAudio( _module, _prefabName );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to QP.Framework.ResMgr.GetAudio!");
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Instance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, QP.Framework.ResMgr.Instance);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Dependencies(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                QP.Framework.ResMgr gen_to_be_invoked = (QP.Framework.ResMgr)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Dependencies);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Dependencies(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                QP.Framework.ResMgr gen_to_be_invoked = (QP.Framework.ResMgr)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Dependencies = (System.Collections.Generic.Dictionary<string, UnityEngine.AssetBundle>)translator.GetObject(L, 2, typeof(System.Collections.Generic.Dictionary<string, UnityEngine.AssetBundle>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
