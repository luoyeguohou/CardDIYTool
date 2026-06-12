using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using System.CodeDom;
using System;
namespace Main
{
    public partial class UI_Card : GComponent
    {
        private readonly List<GComponent> comps = new List<GComponent>();
        private static ObjectPool<GComponent> _imgPool;
        private static ObjectPool<GComponent> _textPool;
        private static ObjectPool<GComponent> imgPool
        {
            get
            {
                if (_imgPool == null)
                    _imgPool = new ObjectPool<GComponent>(
                        () => UIPackage.CreateObject("Main", "Image").asCom
                        , (GComponent g) => g.visible = true,
                        (GComponent g) => g.visible = false
                        );
                return _imgPool;
            }
        }
        private static ObjectPool<GComponent> textPool
        {
            get
            {
                if (_textPool == null)
                    _textPool = new ObjectPool<GComponent>(
                        () => UIPackage.CreateObject("Main", "Text").asCom
                        , (GComponent g) => g.visible = true,
                        (GComponent g) => g.visible = false);
                return _textPool;
            }
        }

        private Card c;
        private bool justPreview;

        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_btn1.draggable = true;
            m_btn2.draggable = true;
            m_btn3.draggable = true;
            m_btn4.draggable = true;
          
            m_btn1.onDragEnd.Add(OnDragEnd1);
            m_btn2.onDragEnd.Add(OnDragEnd2);
            m_btn3.onDragEnd.Add(OnDragEnd3);
            m_btn4.onDragEnd.Add(OnDragEnd4);
            Msg.Bind(MsgID.OnCardChanged, OnCardChanged);
        }

        public override void Dispose()
        {
            base.Dispose();
            Msg.UnBind(MsgID.OnCardChanged, OnCardChanged);
        }

        private void OnCardChanged(object[] p =null) {
            Card c = (Card)p[0];
            if (c != this.c) return;
            // todo update view
        }

        private void InitView() 
        { 
        
        }

        private void UpdateView() 
        {
            width = c.width;
            height = c.height;

            foreach (var item in comps)
            {
                if (item.GetType() == typeof(UI_Image))
                    imgPool.Release(item);
                else
                    textPool.Release(item);
            }
            comps.Clear();
            foreach (var item in c.comps)
            {
                GComponent ui = (item.type == CompType.Image ? imgPool : textPool).Get();
                m_cont.AddChild(ui);
                ui.SetXY(item.x, item.y);
                ui.width = item.width;
                ui.height = item.height;
                ui.onClick.Clear();
                ui.onClick.Add(() => OnClickComp(c, item));
                comps.Add(ui);
                ui.draggable = true;
                ui.dragBounds = TransformRect(new Rect(0, 0, width, height), GRoot.inst);
                ui.onDragEnd.Clear();
                ui.onDragEnd.Add(() => OnDragEndComp(item, ui));

                if (item.type == CompType.Image)
                {
                    (ui as UI_Image).SetImage(item, justPreview);
                }
                else
                {
                    (ui as UI_Text).SetText(item, justPreview);
                }

                m_showCtrlButton.selectedIndex = c.selectedIndex == c.comps.IndexOf(item) && !justPreview ? 1 : 0;
                if (c.selectedIndex == c.comps.IndexOf(item) && !justPreview)
                {
                    //加载出拖拽按钮
                    m_btn1.position = ui.position;
                    m_btn2.position = ui.position + new Vector3(ui.width - m_btn2.width, 0);
                    m_btn3.position = ui.position + new Vector3(0, ui.height - m_btn3.height);
                    m_btn4.position = ui.position + new Vector3(ui.width - m_btn4.width, ui.height - m_btn4.height);
                }
            }
        }

        // 小图/大图导出   不显示拖拽按钮 不显示文字背景
        // 大图编辑     
        public void SetCard(Card c, bool justPreview = false)
        {
            this.c = c;
            this.justPreview = justPreview;
            m_btn1.dragBounds = TransformRect(new Rect(0, 0, width, height), GRoot.inst);
            m_btn2.dragBounds = TransformRect(new Rect(0, 0, width, height), GRoot.inst);
            m_btn3.dragBounds = TransformRect(new Rect(0, 0, width, height), GRoot.inst);
            m_btn4.dragBounds = TransformRect(new Rect(0, 0, width, height), GRoot.inst);
            UpdateView();
        }

        private void OnClickComp(Card c, Comp comp)
        {
            c.selectedIndex = c.comps.IndexOf(comp);
            Msg.Dispatch(MsgID.OnCardChanged,new object[1]{c });
        }

        private void OnDragEndComp(Comp c, GComponent ui)
        {
            UpdateData(c, ui);
        }

        // top left
        private void OnDragEnd1() {
            Comp comp = c.GetCurrComp();
            if (comp == null) return;
            GComponent ui = comps[c.selectedIndex];
            ui.width += ui.x - m_btn1.x;
            ui.height += ui.y - m_btn1.y;
            ui.position = m_btn1.position;
            UpdateData(comp,ui);
        }
        // top right
        private void OnDragEnd2() {
            Comp comp = c.GetCurrComp();
            if (comp == null) return;
            GComponent ui = comps[c.selectedIndex];
            ui.width = m_btn2.x-ui.x+m_btn2.width;
            ui.height += ui.y - m_btn2.y;
            ui.position = new Vector3(ui.position.x, m_btn2.position.y);
            UpdateData(comp, ui);
        }
        // bot left
        private void OnDragEnd3() {
            Comp comp = c.GetCurrComp();
            if (comp == null) return;
            GComponent ui = comps[c.selectedIndex];
            ui.width += ui.x - m_btn3.x;
            ui.height =  m_btn3.y - ui.y + m_btn3.height;
            ui.position = new Vector3(m_btn3.position.x,ui.y);
            UpdateData(comp, ui);
        }
        // bot right
        private void OnDragEnd4() {
            Comp comp = c.GetCurrComp();
            if (comp == null) return;
            GComponent ui = comps[c.selectedIndex];
            ui.width = m_btn4.x - ui.x+ m_btn4.width;
            ui.height = m_btn4.y - ui.y + m_btn4.height;
            UpdateData(comp, ui);
        }

        private void UpdateData(Comp comp,GComponent ui) {
            comp.x = (int)ui.x;
            comp.y = (int)ui.y;
            comp.width = (int)ui.width;
            comp.height = (int)ui.height;
            // todo
            //UIManager.GetType<UI_MainWin>().UpdateView();
            UpdateView();
        }
    }
}
