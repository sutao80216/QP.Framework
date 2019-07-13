using System.Collections.Generic;
using UnityEngine;
namespace QP.Framework
{
    /// <summary>
    /// 负责创建||获取模块
    /// </summary>
    public class ModuleMgr : MonoBehaviour
    {
        private static ModuleMgr _instance = null;
        public static ModuleMgr Instance { get { return Util.GetInstance(ref _instance, "_ModuleMgr"); } }

        private Dictionary<string, Module> _moduleDict = new Dictionary<string, Module>();

        public Module GetModule(string moduleName)
        {
            Module module = null;
            if(!_moduleDict.TryGetValue(moduleName,out module))
            {
                module = new GameObject(moduleName).AddComponent<Module>();
                module.transform.SetParent(transform);
                _moduleDict.Add(moduleName,module);
            }
            return module;
        }
    }
}

