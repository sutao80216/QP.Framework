using System;
using UnityEngine;
using XLua;

namespace QP.Framework
{
    public class Panel
    {
        public Action ShowEvent;
        public Action CloseEvent;
        public CanvasType panelType;
        public PanelStatus panelStatus;
        public GameObject panelObject;
        public LuaTable luaTable;
        private Panel _panel;
        public Panel(CanvasType type, PanelStatus status, GameObject obj, LuaTable table)
        {
            panelType = type;
            panelStatus = status;
            panelObject = obj;
            luaTable = table;
        }
        public void Show()
        {
            panelStatus = PanelStatus.Show;
            if (!panelObject.activeSelf) panelObject.SetActive(true);
            if (_panel != null) _panel.CloseEvent -= Show;
            if (ShowEvent != null) ShowEvent();
            panelObject.transform.SetAsLastSibling();
        }
        public void Close()
        {
            panelStatus = PanelStatus.Close;
            if (panelObject.activeSelf) panelObject.SetActive(false);
            if (_panel != null) _panel.ShowEvent -= Close;
            if (CloseEvent != null) CloseEvent();
        }
        public void Listen(Panel panel)
        {
            UnListen();
            _panel = panel;
            _panel.ShowEvent += Close;
            _panel.CloseEvent += Show;
        }
        public void UnListen()
        {
            if (_panel != null)
            {
                _panel.CloseEvent -= Show;
                _panel.ShowEvent -= Close;
            }
        }
    }

}
