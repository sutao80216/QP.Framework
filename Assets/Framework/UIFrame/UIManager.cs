using System.Collections.Generic;
using UnityEngine;
using XLua;
namespace QP.Framework
{
    public enum ShowType
    {
        None,
        /// <summary>
        /// 打开当前面板时关闭上一个面板
        /// 当关闭当前面板时 会重新显示
        /// </summary>
        CloseLastOne,
        /// <summary>
        /// 打开当前面板时关闭上面所有面板
        /// 当关闭当前面板时 会重新显示
        /// </summary>
        CloseLastAll,

        DestroyOne, //删除上一个面板（暂未实现）

        DestroyAll, //删除所有面板（暂未实现）
    }
    public enum CanvasType { Fixed, Normal, Top }
    public enum PanelStatus { None, Show, Close }
    /// <summary>
    /// 简单的UI框架
    /// </summary>
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
            _FixedCanvas = GameObject.Find("_FixedCanvas").transform;
            _NormalCanvas = GameObject.Find("_NormalCanvas").transform;
            _TopCanvas = GameObject.Find("_TopCanvas").transform;
            _FixedStack = new Stack<Panel>();
            _NormalStack = new Stack<Panel>();
            _TopStack = new Stack<Panel>();
            _CachePanel = new Dictionary<string, Panel>();
        }
        public LuaTable ShowPanel(string module, string panelName) { return ShowPanel(module, panelName, null, CanvasType.Fixed, ShowType.None); }
        public LuaTable ShowPanel(string module, string panelName,string bundleName) { return ShowPanel(module, panelName, bundleName, CanvasType.Fixed, ShowType.None); }
        public LuaTable ShowPanel(string module, string panelName,string bundleName, CanvasType canvasType) { return ShowPanel(module, panelName, bundleName, canvasType, ShowType.None); }
        public LuaTable ShowPanel(string module,string panelName,string bundleName, CanvasType canvasType, ShowType showType)
        {
            Panel panel = null;
            if(!_CachePanel.TryGetValue(panelName,out panel))
            {
                panel = CreatePanel(module, panelName, bundleName, canvasType);
                _CachePanel.Add(panelName, panel);
            }
            Stack<Panel> stack = GetStack(canvasType);
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
        /// <summary>
        /// 关闭最上层面板
        /// </summary>
        public void CloseTop(CanvasType type)
        {
            Stack<Panel> stack = GetStack(type);
            Panel panel = stack.Pop(); 
            panel.Close();
            panel.UnListen();
        }
        /// <summary>
        /// 关闭所有面板
        /// </summary>
        public void CloseAll(CanvasType type)
        {
            Stack<Panel> stack = GetStack(type);
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
                case CanvasType.Fixed:return _FixedCanvas;
                case CanvasType.Normal: return _NormalCanvas;
                case CanvasType.Top: return _TopCanvas;
                default: return _FixedCanvas;
            }
        }
        private Stack<Panel> GetStack(CanvasType type)
        {
            switch (type)
            {
                case CanvasType.Fixed:return _FixedStack;
                case CanvasType.Normal:return _NormalStack;
                case CanvasType.Top:return _TopStack;
                default:return _FixedStack;
            }
        }
        private Panel CreatePanel(string module, string panelName,string bundleName,CanvasType type)
        {
            GameObject prefab = ResMgr.Instance.GetPrefab(module, panelName, bundleName);
            prefab = Instantiate(prefab, GetParent(type));
            LuaScript script = LuaEnvMgr.Instance.Create(prefab, string.Format("{0}/{1}", module, panelName));
            Panel panel = new Panel(type, PanelStatus.None, prefab, script.Table);
            return panel;
        }
    }
}
