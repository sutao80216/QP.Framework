using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using QP.Framework;
public class Package  {
    
    static string ModuleRoot = string.Format("{0}/{1}", Application.dataPath, GameConfig.module_name);
    static string TargetRoot = string.Format("{0}/{1}", Application.streamingAssetsPath, GameConfig.module_name);
    //直接打包到你的wamp64 服务WWW目录中 方便测试
    //static string TargetRoot = string.Format("C:/Users/steven/Desktop/Application/wamp64/www/{0}", GameConfig.module_name);
    static string AB_Lua = "AB_Lua";
    static string AB_Prefab = "AB_Prefab";
    static string AB_Scene = "AB_Scene";
    static string AB_Texture = "AB_Texture";
    static string AB_Material = "AB_Material";
    static string AB_Audio = "AB_Audio";

    [MenuItem("构建/开始打包", false, 1)]
    public static void StartBuild(){
        
        ClearAssetBundlesName();
        //CleanAssetBundleName();
        CreateDirectory(TargetRoot);
        List<string> Modules = GetChilds(ModuleRoot);
        foreach (var module in Modules)
        {
            BuildModule(module, AB_Prefab);
            BuildModule(module, AB_Texture);
            BuildModule(module, AB_Audio);
            BuildModule(module, AB_Scene);
            BuildModule(module, AB_Material);
            BuildModule(module, AB_Lua);

            Debug.Log(Path.GetFileName(module)+ "   ---------> ok!");
        }
        EditorUtility.DisplayProgressBar("Building AssetBundle", "waiting...", 0f);

        Close();
        BuildHandler();
        try
        {
            string outPath = string.Format("{0}/{1}", TargetRoot, "md5file.txt");
            FileStream fs = new FileStream(outPath, FileMode.CreateNew);
            StreamWriter sw = new StreamWriter(fs);
            string file = string.Format("{0}/{1}", TargetRoot, GameConfig.module_name);
            string md5 = Util.Md5File(file);
            string value = file.Replace("\\", "/");
            value = value.Replace(TargetRoot + "/", string.Empty);
            sw.WriteLine(value + "|" + md5);
            fs.Flush();
            sw.Close();
            fs.Close();
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.Message);
            Close();
            return;
        }



        foreach (var module in Modules)
        {
            CreateMd5File(module);
            CreateVersionFile(module);
        }
        CreateAppVersionFile();
        Debug.Log("--------- 打包完毕！---------");
        AssetDatabase.Refresh();
        Close();
    }

    [MenuItem("构建/清理 AssetBundleName", false, 2)]
    private static void ClearAssetBundlesName()
    {
        int length = AssetDatabase.GetAllAssetBundleNames().Length;
        string[] oldAssetBundleNames = new string[length];
        for (int i = 0; i < length; i++)
        {
            oldAssetBundleNames[i] = AssetDatabase.GetAllAssetBundleNames()[i];
            EditorUtility.DisplayProgressBar("Building AssetBundle", oldAssetBundleNames[i],(float)i / (float)(length-1));
        }
        for (int i = 0; i < oldAssetBundleNames.Length; i++)
        {
            AssetDatabase.RemoveAssetBundleName(oldAssetBundleNames[i], true);
            EditorUtility.DisplayProgressBar("Building AssetBundle", oldAssetBundleNames[i], (float)i / (float)(length - 1));
        }
        EditorUtility.ClearProgressBar();
    }
    static void BuildLua(List<string> fileList){
        
        for (int i = 0; i < fileList.Count; i++)
        {
            string file = fileList[i];
            file = file.Replace("\\", "/");
            string outPath = file.Replace(ModuleRoot, TargetRoot);
            string dir = Path.GetDirectoryName(outPath);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            
            outPath=outPath.Replace(".lua", ".txt");
            outPath=outPath.Replace(".Lua", ".txt");
            File.Copy(file, outPath, true);
            EditorUtility.DisplayProgressBar("Building AssetBundle", file, (float)i / (float)(fileList.Count - 1));

        }

    }
    static void CreateAppVersionFile()
    {
        try
        {
            Version version = new Version();
            version.version = BuildConfig.version;
            version.root_module = BuildConfig.root_module;
            version.app_download_url = BuildConfig.app_download_url;
            version.res_download_url = BuildConfig.res_download_url;
            version.download_fail_retry = BuildConfig.download_fail_retry;
            version.preTamperLua = BuildConfig.preTamperLua;
            string json = JsonUtility.ToJson(version);
            string target = string.Format("{0}/{1}", TargetRoot, "version.txt");
            if (File.Exists(target)) File.Delete(target);
            FileStream fs = new FileStream(target, FileMode.CreateNew);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(json);
            fs.Flush();
            sw.Close();
            fs.Close();
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.Message);
            Close();
        }

    }
    static void CreateVersionFile(string module)
    {
        try
        {
            module = module.Replace("\\", "/");
            string version = string.Format("{0}/{1}", module, "version.txt");
            if (!File.Exists(version)) return;
            string target = version.Replace(ModuleRoot, TargetRoot);
            File.Copy(version, target, true);
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.Message);
            Close();
        }
    }
    static void CreateMd5File(string module){
        try
        {
            module = module.Replace("\\", "/");
            module = module.Replace(ModuleRoot, TargetRoot);
            List<string> list = new List<string>();
            Recursive(module, ref list);

            string outPath = string.Format("{0}/{1}", module, "md5file.txt");
            FileStream fs = new FileStream(outPath, FileMode.CreateNew);
            StreamWriter sw = new StreamWriter(fs);
            for (int i = 0; i < list.Count; i++)
            {
                string file = list[i];
                if (file.EndsWith(".manifest")) continue;
                string md5 = Util.Md5File(file);
                string value = file.Replace("\\", "/");
                value = value.Replace(TargetRoot + "/", string.Empty);
                sw.WriteLine(value + "|" + md5);
                EditorUtility.DisplayProgressBar("Building AssetBundle", value + "|" + md5, (float)i / (float)(list.Count - 1));
            }
            fs.Flush();
            sw.Close();
            fs.Close();
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.Message);
            Close();
        }

    }

    static void BuildModule(string module,string child){
        List<string> fileList = new List<string>();
        string path = string.Format("{0}/{1}", module, child);
        Recursive(path, ref fileList);
        if (child == AB_Lua){
            BuildLua(fileList);
        }else{
            SetAssetBundleName(fileList, module, path);
        }

    }

    static void Recursive(string path,ref List<string>list){
        if (Directory.Exists(path)){  
            
            DirectoryInfo direction = new DirectoryInfo(path);  
            FileInfo[] files = direction.GetFiles("*",SearchOption.AllDirectories);

            path = path.Replace(Application.dataPath, string.Empty);
            for(int i=0;i<files.Length;i++){

                EditorUtility.DisplayProgressBar("Building AssetBundle", files[i].FullName, (float)i / (float)(files.Length - 1));

                if (files[i].Name.EndsWith(".cs")||files[i].Name.EndsWith(".meta")||files[i].Name.EndsWith(".json"))
                {  
                    continue;
                }
                list.Add(files[i].FullName);
            }  
        }  
    }
    static void SetAssetBundleName(List<string> list,string module, string rootPath){
        
        for (int i = 0; i < list.Count; i++)
        {
            string file = list[i];
            file = file.Replace("\\", "/");
            rootPath = rootPath.Replace("\\", "/");
            string name = GetParentDirectoryName(file.Replace(rootPath, string.Empty));
            DirectoryInfo dirInfo = new DirectoryInfo(file);
            string filePath = dirInfo.FullName.Replace("\\", "/");
            filePath = filePath.Replace(Application.dataPath, "Assets");
            AssetImporter ai = AssetImporter.GetAtPath(filePath);
            if(ai!=null){
                if (name != null)
                {
                    ai.assetBundleName = string.Format("{0}/{1}", Path.GetFileName(module), name);
                    //ai.assetBundleName = name;
                }
                else
                {
                    ai.assetBundleName = string.Format("{0}/{1}", Path.GetFileName(module), Path.GetFileNameWithoutExtension(file));
                    //ai.assetBundleName = Path.GetFileNameWithoutExtension(file);
                }
            }


            EditorUtility.DisplayProgressBar("Building AssetBundle", file, (float)i / (float)(list.Count - 1));

        }
    }

    static string GetParentDirectoryName(string path){
        string dir = Path.GetFileName(Path.GetDirectoryName(path));
        if (string.IsNullOrEmpty(dir)) return null;
        if(dir.IndexOf('_')!=0)
        {
            path = path.Replace("/" + Path.GetFileName(path), string.Empty);
            if (Path.GetDirectoryName(path) == "/") return null;
            return GetParentDirectoryName(path);
        }
        return dir;  
    }

    static List<string> GetChilds(string path){
        DirectoryInfo dirInfo = new DirectoryInfo(path);
        FileSystemInfo[] files = dirInfo.GetFileSystemInfos();
        List<string> list = new List<string>();
        foreach (FileSystemInfo file in files)
        {
            if (Path.GetFileName(file.FullName) == ".vscode") continue;
            if (Directory.Exists(file.FullName))
            {
                list.Add(file.FullName);
            }
        }
        return list;
    }
    static void CreateDirectory(string path){
        if (Directory.Exists(path)) Directory.Delete(path,true);
        Directory.CreateDirectory(path);
    }
    static void BuildHandler(string module){
        string path = string.Format("{0}/{1}", TargetRoot, Path.GetFileName(module));
        if (!Directory.Exists(path)) 
            Directory.CreateDirectory(path);
        
        BuildPipeline.BuildAssetBundles(path,
                                         BuildAssetBundleOptions.ChunkBasedCompression,
                                         buildTarget());
       
    }
    static void BuildHandler()
    {
        if (!Directory.Exists(TargetRoot))
            Directory.CreateDirectory(TargetRoot);
        BuildPipeline.BuildAssetBundles(TargetRoot,
                                         BuildAssetBundleOptions.ChunkBasedCompression,
                                         buildTarget());
    }
    static BuildTarget buildTarget()
    {

#if UNITY_IOS
       return BuildTarget.iOS;
#endif

#if UNITY_ANDROID
        return BuildTarget.Android;
#endif

#if UNITY_EDITOR
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            return BuildTarget.StandaloneWindows64;
        }
        else if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return BuildTarget.StandaloneOSXIntel64;
        }
#endif
        return BuildTarget.StandaloneWindows64;
    }

    static void Close(){
        EditorUtility.ClearProgressBar();
    }





}


//#if UNITY_IOS || UNITY_ANDROID
////这里的代码在IOS和Android平台都会编译
//#endif

//#if UNITY_ANDROID && UNITY_EDITOR
////这里的代码只有在发布设置设置的是Android，且在编辑器里运行时才会编译
//#endif