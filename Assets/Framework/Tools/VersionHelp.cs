using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
namespace QP.Framework
{
    public class Version
    {
        public string version;
        public string app_download_url;
        public string res_download_url;
    }
    public class VersionHelp
    {
        private static Version _version;
        public static Version version
        {
            get
            {
                if (_version == null)
                {
                    _version = GetLocalVersionForApp();
                }
                return _version;
            }
        }
        public static Version GetLocalVersionForModule(string module)
        {
            string version_path = string.Format("{0}/{1}/{2}", Util.DeviceResPath, module, "version.txt");
            if (!File.Exists(version_path)) return null;
            try
            {
                string text = File.ReadAllText(version_path);
                return StringForVersion(text);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
                return null;
            }
        }
        public static Version JsonForVersion(string json)
        {
            try
            {
                Version version = JsonUtility.FromJson<Version>(json);
                return version;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
                return null;
            }
        }
        public static Version StringForVersion(string text)
        {
            Version version = new Version();
            version.version = text.Trim();
            version.res_download_url = BuildConfig.res_download_url;
            return version;
        }
        public static Version GetLocalVersionForApp()
        {
            string version_path = string.Format("{0}/{1}", Util.DeviceResPath, "version.txt");
            if (!File.Exists(version_path)) return null;
            string text = File.ReadAllText(version_path);
            if (string.IsNullOrEmpty(text)) return null;
            return JsonForVersion(text);
        }

        public static void WriteLocalVersionFile(Version version)
        {
            string version_path = string.Format("{0}/{1}", Util.DeviceResPath, "version.txt");
            if (!Directory.Exists(Path.GetDirectoryName(version_path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(version_path));
            }
            if (File.Exists(version_path)) File.Delete(version_path);
            string json = JsonUtility.ToJson(version);
            try
            {
                File.WriteAllText(version_path, json);
                 _version = version;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
                throw;
            }
            
        }


    }
}

