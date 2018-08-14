using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
namespace QP.Framework
{
    public class Util
    {
        /// <summary>
        /// 查找子物体
        /// </summary>
        public static T FindObject<T>(Transform parent, string name) where T : UnityEngine.Object
        {
            Transform obj = GetChild(parent, name);
            if (obj != null)
            {
                if (typeof(T).Equals(typeof(UnityEngine.GameObject)))
                    return obj.gameObject as T;
                if (typeof(T).Equals(typeof(UnityEngine.Transform)))
                    return obj as T;
                return obj.gameObject.GetComponent<T>();
            }
            return null;
        }
        static Transform GetChild(Transform parent, string name)
        {
            if (parent.gameObject.name == name)
                return parent;
            for (int i = 0; i < parent.childCount; i++)
            {
                Transform curr = GetChild(parent.GetChild(i), name);
                if (curr != null && curr.gameObject.name == name)
                    return curr;
            }
            return null;
        }
        /// <summary>
        /// 创建单例
        /// </summary>
        public static T GetInstance<T>(ref T instance,string name, bool isDontDestroy=true) where T : UnityEngine.Object
        {
            if (instance != null) return instance;
            GameObject go = new GameObject(name, typeof(T));
            if (isDontDestroy) UnityEngine.Object.DontDestroyOnLoad(go);
            instance= go.GetComponent(typeof(T)) as T;
            return instance;
        }
        public static string DeviceResPath
        {
            get
            {
                switch (GameConfig.gameModel)
                {
                    case GameModel.Editor:
                        return string.Format("{0}/{1}", Application.dataPath, GameConfig.module_name);
                    case GameModel.Local:
                        return string.Format("{0}/{1}", Application.streamingAssetsPath, GameConfig.module_name);
                    case GameModel.Remote:
                        return string.Format("{0}/{1}", Application.persistentDataPath, GameConfig.module_name);
                }
                return string.Format("{0}/{1}", Application.dataPath, GameConfig.module_name);
            }
        }
        public static string WWWDeviceResPath
        {
            get
            {
                string path = string.Empty;
                switch (Application.platform)
                {
                    case RuntimePlatform.Android:
                        path= string.Format("{0}{1}/{2}", "file://", Application.persistentDataPath, GameConfig.module_name);
                        //path = string.Format("{0}{1}/{2}", "jar:file:///", Application.persistentDataPath, GameConfig.module_name);
                        //"jar:file://" + Application.persistentDataPath + "!/assets"; 
                        break;
                    case RuntimePlatform.IPhonePlayer:
                        path = Application.persistentDataPath + "/Raw";
                        break;
                    default:
                        path=  "file://" + DeviceResPath.Replace("\\", "/");
                        break;
                }
                return path;
            }
        }
        public static string GetLuaPath(string path)
        {
            if (path.IndexOf("AB_Lua") == -1)
            {
                StringBuilder sb = new StringBuilder(path);
                path = sb.Replace("/", "/AB_Lua/", path.IndexOf('/'), 1).ToString();
            }
            path = string.Format("{0}/{1}{2}", DeviceResPath, path, GetLuaExtension());
            return path;
        }
        public static string GetLuaExtension()
        {
            if (GameConfig.gameModel == GameModel.Editor)
            {
                return ".lua";
            }
            return ".txt";
        }
        public static string Md5String(string str)
        {
            if (null == str) return null;
            byte[] result = ((System.Security.Cryptography.HashAlgorithm)System.Security.Cryptography.CryptoConfig.CreateFromName("MD5")).ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder output = new StringBuilder(16);
            for (int i = 0; i < result.Length; i++)
            {
                output.Append((result[i]).ToString("x2", System.Globalization.CultureInfo.InvariantCulture));
            }
            return output.ToString();
        }

        public static string Md5File(string file)
        {
            if (null == file) return null;
            try
            {
                FileStream fs = new FileStream(file, FileMode.Open);
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(fs);
                fs.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("md5file() fail, error:" + ex.Message);
            }
        }
    }
}

