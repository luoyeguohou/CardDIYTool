/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_Card : GComponent
    {
        public Controller m_showCtrlButton;
        public GGraph m_bg;
        public GComponent m_cont;
        public GButton m_btn1;
        public GButton m_btn2;
        public GButton m_btn3;
        public GButton m_btn4;
        public const string URL = "ui://5eon00jxivra3";

        public static UI_Card CreateInstance()
        {
            return (UI_Card)UIPackage.CreateObject("Main", "Card");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_showCtrlButton = GetControllerAt(0);
            m_bg = (GGraph)GetChildAt(0);
            m_cont = (GComponent)GetChildAt(1);
            m_btn1 = (GButton)GetChildAt(2);
            m_btn2 = (GButton)GetChildAt(3);
            m_btn3 = (GButton)GetChildAt(4);
            m_btn4 = (GButton)GetChildAt(5);
        }
    }
}