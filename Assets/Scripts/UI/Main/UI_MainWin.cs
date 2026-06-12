/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_MainWin : FairyWindow
    {
        public GImage m_bg;
        public UI_Main m_cont;
        public const string URL = "ui://5eon00jx8xna8";

        public static UI_MainWin CreateInstance()
        {
            return (UI_MainWin)UIPackage.CreateObject("Main", "MainWin");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_bg = (GImage)GetChildAt(0);
            m_cont = (UI_Main)GetChildAt(1);
        }
    }
}