using System.Collections.Generic;
using UnityEngine;
using XLua;
namespace QP.Framework
{
    [LuaCallCSharp]
    public enum ShowType
    {
        None,
        CloseLastOne,//关闭上一个面板
        CloseLastAll,//关闭上面所有面板
        DestroyOne,
        DestroyAll,
    }
    [LuaCallCSharp]
    public enum CanvasType { Fixed, Normal, Top }
    public enum PanelStatus { None, Show, Close }
    [LuaCallCSharp]
    public class UIManager : MonoBehaviour
    {
        private static UIManager _instance;
        public static UIManager Instance
        {
            get { return Util.GetInstance(ref _instance, "_UIManager",false); }
        }
        private Transform _FixedCanvas;
        private Transform _NormalCanvas;
        private Transform _TopCanvas;
        private Stack<Panel> _FixedStack;
        private Stack<Panel> _NormalStack;
        private Stack<Panel> _TopStack;
        private Dictionary<string, Panel> _CachePanel;
        void Awake()
        {
            this._FixedCanvas = GameObject.Find("_FixedCanvas").transform;
            this._NormalCanvas = GameObject.Find("_NormalCanvas").transform;
            this._TopCanvas = GameObject.Find("_TopCanvas").transform;
            this._FixedStack = new Stack<Panel>();
            this._NormalStack = new Stack<Panel>();
            this._TopStack = new Stack<Panel>();
            this._CachePanel = new Dictionary<string, Panel>();
        }
        public LuaTable ShowPanel(string module, string panelName) { return this.ShowPanel(module, panelName, null, CanvasType.Fixed, ShowType.None); }
        public LuaTable ShowPanel(string module, string panelName,string bundleName) { return this.ShowPanel(module, panelName, bundleName, CanvasType.Fixed, ShowType.None); }
        public LuaTable ShowPanel(string module, string panelName,string bundleName, CanvasType canvasType) { return this.ShowPanel(module, panelName, bundleName, canvasType, ShowType.None); }
        public LuaTable ShowPanel(string module,string panelName,string bundleName, CanvasType canvasType, ShowType showType)
        {
            Panel panel = null;
            if(!this._CachePanel.TryGetValue(panelName,out panel))
            {
                panel = this.CreatePanel(module, panelName, bundleName, canvasType);
                this._CachePanel.Add(panelName, panel);
            }
            Stack<Panel> stack = this.GetStack(canvasType);
            switch (showType)
            {
                case ShowType.CloseLastOne:
                    Panel lastPanel = stack.Peek();
                    lastPanel.Listen(panel);
                    break;
                case ShowType.CloseLastAll:
                    Panel[] ps = stack.ToArray();
                    for (int i = ps.Length-1; i >=0; i--) ps[i].Listen(panel);
                    break;
                case ShowType.DestroyOne:
                    break;
                case ShowType.DestroyAll:
                    break;
            }
            panel.Show();
            stack.Push(panel);
            return panel.luaTable;
        }
        public void CloseTop(CanvasType type)
        {
            Stack<Panel> stack = this.GetStack(type);
            Panel panel = stack.Pop(); 
            panel.Close();
            panel.UnListen();
        }
        public void CloseAll(CanvasType type)
        {
            Stack<Panel> stack = this.GetStack(type);
            Stack<Panel>.Enumerator e = stack.GetEnumerator();
            while (e.MoveNext())
            {
                e.Current.Close();
                e.Current.UnListen();
            }
            e.Dispose();
            stack.Clear();
        }
        private Transform GetParent(CanvasType type)
        {
            switch (type)
            {
                case CanvasType.Fixed:return this._FixedCanvas;
                case CanvasType.Normal: return this._NormalCanvas;
                case CanvasType.Top: return this._TopCanvas;
                default: return this._FixedCanvas;
            }
        }
        private Stack<Panel> GetStack(CanvasType type)
        {
            switch (type)
            {
                case CanvasType.Fixed:return this._FixedStack;
                case CanvasType.Normal:return this._NormalStack;
                case CanvasType.Top:return this._TopStack;
                default:return this._FixedStack;
            }
        }
        private Panel CreatePanel(string module, string panelName,string bundleName,CanvasType type)
        {
            GameObject prefab = ResMgr.Instance.GetPrefab(module, panelName, bundleName);
            prefab = Instantiate(prefab, this.GetParent(type));
            LuaScript script = LuaEnvMgr.Instance.Create(prefab, string.Format("{0}/{1}", module, panelName));
            Panel panel = new Panel(type, PanelStatus.None, prefab, script.Table);
            return panel;
        }
    }
}
