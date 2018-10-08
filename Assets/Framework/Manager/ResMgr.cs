using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;

namespace QP.Framework
{
    public class ModuleAssetBundle
    {
        public Dictionary<string, AssetBundle> bundles;
    }
    /// <summary>
    /// 资源管理类
    /// </summary>
    public class ResMgr : MonoBehaviour
    {
        private Dictionary<string, ModuleAssetBundle> ModuleAssetBundles;
        public Dictionary<string, AssetBundle> Dependencies;
        private static ResMgr _instance;
        public static ResMgr Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject("_ResMgr", typeof(ResMgr)).GetComponent<ResMgr>();
                    DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
        }
        void Awake()
        {
            ModuleAssetBundles = new Dictionary<string, ModuleAssetBundle>();
            Dependencies = new Dictionary<string, AssetBundle>();
        }
        public void InitAssetBundle(string module,Action<float>progress, Action complete)
        {
            if (complete == null) complete = () => { };
            if (GameConfig.gameModel== GameModel.Editor)
            {
                complete();
                return;
            }
            ModuleAssetBundle moduleAssetBundle = null;
            if (ModuleAssetBundles.TryGetValue(module,out moduleAssetBundle))
            {
                if(moduleAssetBundle.bundles!=null){
                    complete();
                    return;
                }
            }
            StartCoroutine(Init(module, progress,complete));
        }

        private IEnumerator Init(string module,Action<float>progress, Action complete)
        {
            //string url = string.Format("{0}/{1}/{2}", Util.WWWDeviceResPath, module, module);
            string url = string.Format("{0}/{1}", Util.WWWDeviceResPath, GameConfig.module_name);
            WWW www = new WWW(url);
            yield return www;
            if (www.error != null)
            {
                Debug.LogError(www.error);
                yield break;
            }
            AssetBundle manifestBundle = www.assetBundle;
            AssetBundleManifest manifest = (AssetBundleManifest)manifestBundle.LoadAsset("AssetBundleManifest");
            manifestBundle.Unload(false);
            //获取依赖文件列表
            string[] allBundles = manifest.GetAllAssetBundles();
            ModuleAssetBundle moduleAssetBundle = new ModuleAssetBundle();
            moduleAssetBundle.bundles = new Dictionary<string, AssetBundle>();
            float count = allBundles.Length;
            float curr = 0;
            for (int i = 0; i < allBundles.Length; i++)
            {
                curr++;
                if (progress != null) progress(curr / count);
                string bundleName = allBundles[i];
                if (Path.GetDirectoryName(bundleName) != module.Trim().ToLower()) continue;
                string bundleUrl = string.Format("{0}/{1}", Util.WWWDeviceResPath, bundleName);
                WWW bw = new WWW(bundleUrl);
                yield return bw;
                yield return StartCoroutine(FindDependencies(manifest, bundleName,
                    (string key) => moduleAssetBundle.bundles.ContainsKey(key),
                    (string key, AssetBundle value) => moduleAssetBundle.bundles.Add(key, value)));
                if (bw.error != null)
                {
                    Debug.LogError("请检查  "+ module+"  是否已经下载成功？"+ bw.error);
                }
                else
                {
                    moduleAssetBundle.bundles.Add(Path.GetFileName(bundleName), bw.assetBundle);
                }
            }

            if (ModuleAssetBundles.ContainsKey(module)){
                ModuleAssetBundles[module] = moduleAssetBundle;
            }else{
                ModuleAssetBundles.Add(module, moduleAssetBundle);
            }
            complete();
        }

        private IEnumerator FindDependencies(AssetBundleManifest manifest,string bundleName, Func<string,bool> IsContains,Action<string,AssetBundle> Add)
        {
            string[] deps = manifest.GetAllDependencies(bundleName);
            if (deps.Length > 0)
            {
                for (int j = 0; j < deps.Length; j++)
                {
                    string dep = deps[j];
                    string depModule = Path.GetDirectoryName(dep);
                    if (depModule == Path.GetDirectoryName(bundleName)) continue;
                    if (IsContains(Path.GetFileName(dep))) continue;
                    //if (Dependencies.ContainsKey(Path.GetFileName(dep))) continue;
                    string url = string.Format("{0}/{1}", Util.WWWDeviceResPath, dep);
                    //Debug.Log("添加依赖资源：" + url);

                    WWW www = new WWW(url);
                    yield return www;
                    Add(Path.GetFileName(dep), www.assetBundle);
                    //Dependencies.Add(Path.GetFileName(dep), www.assetBundle);
                }
            }
            yield return new WaitForEndOfFrame();
        }

        public void ClearOtherModule(string module)
        {
            Dictionary<string, ModuleAssetBundle>.Enumerator e = ModuleAssetBundles.GetEnumerator();
            while (e.MoveNext())
            {
                if (e.Current.Key == module) continue;
                if (e.Current.Value.bundles == null) continue;
                //如果内存允许的话可以打开这个 可以让过度场景无延迟跳转
                if (e.Current.Key != module)
                {
                    //Debug.Log(e.Current.Key + " 资源被释放！！！"+ module);
                    ModuleAssetBundle moduleAssetBundle = e.Current.Value;
                    Dictionary<string, AssetBundle>.Enumerator m = moduleAssetBundle.bundles.GetEnumerator();
                    while (m.MoveNext())
                    {
                        m.Current.Value.Unload(true);
                    }
                    moduleAssetBundle.bundles.Clear();
                    moduleAssetBundle.bundles = null;
                    m.Dispose();
                }
            }
            e.Dispose();

            Debug.Log("---------------------当前模块：" + module + "的资源--------------------------");
            foreach (var item in ModuleAssetBundles)
            {
                if (item.Value.bundles != null)
                {
                    Debug.Log(item.Key);
                    foreach (var item2 in item.Value.bundles)
                    {
                        Debug.Log("\t->> " + item2.Key);
                    }
                }
            }

        }
        public GameObject GetPrefab(string module, string prefabName, string bundleName = null) 
        {
            return GetObject<GameObject>(module, prefabName, bundleName,"AB_Prefab");
        }
        public Texture2D GetTexture(string module, string prefabName, string bundleName = null)
        {
            return GetObject<Texture2D>(module, prefabName, bundleName, "AB_Texture");
        }
        public AudioClip GetAudio(string module, string prefabName, string bundleName = null)
        {
            return GetObject<AudioClip>(module, prefabName, bundleName, "AB_Audio");
        }

        private T GetObject<T>(string module, string prefabName, string bundleName,string dir) where T:UnityEngine.Object
        {
            if (GameConfig.gameModel == GameModel.Editor)
            {
                return DeepFind<T>(module, prefabName, dir);
            }
            ModuleAssetBundle moduleAssetBundle = null;
            if (ModuleAssetBundles.TryGetValue(module, out moduleAssetBundle))
            {
                bundleName = bundleName ?? prefabName;
                AssetBundle bundle = null;
                if (moduleAssetBundle.bundles.TryGetValue(bundleName.ToLower(), out bundle))
                {
                    return bundle.LoadAsset<T>(prefabName);
                }
            }
            return null;
        }

        private T DeepFind<T>(string module, string prefabName,string dir ) where T : UnityEngine.Object
        {
            string path = string.Format("{0}/{1}/{2}", Util.DeviceResPath, module, dir);
            string url = Recursive(path, prefabName);
            if (url == null) return null;
            url = url.Replace("\\", "/");
            url = url.Replace(Application.dataPath, string.Empty);
            url = string.Format("Assets{0}", url);
#if UNITY_EDITOR
            return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(url) as T;
#endif
            return null;
        }
       
        private string Recursive(string path,string name ){
            if (!Directory.Exists(path)) return null;
            DirectoryInfo direction = new DirectoryInfo(path);  
            FileInfo[] files = direction.GetFiles("*",SearchOption.AllDirectories);

            for(int i=0;i<files.Length;i++){
                FileInfo file = files[i];
                if (file.Name.EndsWith(".meta")||file.Name.EndsWith(".json")){
                    continue;
                }
                if (Path.GetFileNameWithoutExtension(file.Name) == name)
                {
                    return file.FullName;
                }
            }
            return null;
        }
    }

}
