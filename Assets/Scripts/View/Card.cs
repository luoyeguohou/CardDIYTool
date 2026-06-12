using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using System.CodeDom;
namespace Main
{
    public partial class UI_Card : GComponent
    {
        private readonly List<GComponent> comps = new List<GComponent>();

        private Card c;

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
        }

        // 小图/大图导出   不显示拖拽按钮 不显示文字背景
        // 大图编辑     
        public void SetCard(Card c, bool justPreview = false)
        {
            this.c = c;
            width = c.width;
            height = c.height;
            m_btn1.dragBounds = TransformRect(new Rect(0, 0, width, height), GRoot.inst);
            m_btn2.dragBounds = TransformRect(new Rect(0, 0, width, height), GRoot.inst);
            m_btn3.dragBounds = TransformRect(new Rect(0, 0, width, height), GRoot.inst);
            m_btn4.dragBounds = TransformRect(new Rect(0, 0, width, height), GRoot.inst);
            foreach (var item in comps)
            {
                item.Dispose();
            }
            comps.Clear();
            foreach (var item in c.comps)
            {
                GComponent ui = UIPackage.CreateObject("Main", item.type == CompType.Image ? "Image" : "Text").asCom;
                m_cont.AddChild(ui);
                ui.SetXY(item.x, item.y);
                ui.width = item.width;
                ui.height = item.height;
                ui.onClick.Add(() => OnClickComp(c, item));
                comps.Add(ui);
                ui.draggable = true;
                ui.dragBounds = TransformRect(new Rect(0, 0, width, height), GRoot.inst);
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

        private void OnClickComp(Card c, Comp comp)
        {
            c.selectedIndex = c.comps.IndexOf(comp);
            UIManager.GetType<UI_MainWin>().UpdateView();
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
            UIManager.GetType<UI_MainWin>().UpdateView();
        }
    }
}
