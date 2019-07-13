using UnityEngine;
using XLua;

namespace QP.Framework
{
    public delegate void DVoid();
    public delegate void DInt(int i);
    public delegate void DString(string s);
    public delegate void DString_Int(string s, int i);
    public delegate void DString_Float(string s, float f);
    public delegate void DString_Int_Int(string s,int i1, int i2);
    public delegate void DString_Int_String(string s1,int i, string s2);
    public delegate void DString_Float_Float(string s,float f1, float f2);
    public delegate void DString_SDownloadFileResult(string s, SDownloadFileResult f1);
    /// <summary>
    /// 检查更新回调
    /// </summary>
    public class CheckUpdateTable
    {
        public DString Error;
        public DString_Int_String Complete;
    }
    /// <summary>
    /// 下载回调
    /// </summary>
    public class DownloadTable
    {
        public DString Error;
        public DString_Int Befor;
        public DString AllComplete;
        public DString_Int_Int OneComplete;
        public DString_SDownloadFileResult Progress;
    }
    /// <summary>
    /// 跳转场景回调
    /// </summary>
    public class SceneTable
    {
        public DString Error;
        public DString Complete;
        public DString_Float Progress;
    }
}


