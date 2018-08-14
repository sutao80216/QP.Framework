using System;
using System.Collections;
using System.Collections.Generic;
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
            this.panelType = type;
            this.panelStatus = status;
            this.panelObject = obj;
            this.luaTable = table;
        }
        public void Show()
        {
            this.panelStatus = PanelStatus.Show;
            if (!this.panelObject.activeSelf) this.panelObject.SetActive(true);
            if (this._panel != null) this._panel.CloseEvent -= this.Show;
            if (this.ShowEvent != null) this.ShowEvent();
            this.panelObject.transform.SetAsLastSibling();
        }
        public void Close()
        {
            this.panelStatus = PanelStatus.Close;
            if (this.panelObject.activeSelf) this.panelObject.SetActive(false);
            if (this._panel != null) this._panel.ShowEvent -= this.Close;
            if (this.CloseEvent != null) this.CloseEvent();
        }
        public void Listen(Panel panel)
        {
            this.UnListen();
            this._panel = panel;
            this._panel.ShowEvent += this.Close;
            this._panel.CloseEvent += this.Show;
        }
        public void UnListen()
        {
            if (this._panel != null)
            {
                this._panel.CloseEvent -= this.Show;
                this._panel.ShowEvent -= this.Close;
            }
        }
    }

}
