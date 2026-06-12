/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_Text : GComponent
    {
        public Controller m_preview;
        public GTextField m_txtCont;
        public const string URL = "ui://5eon00jxivra6";

        public static UI_Text CreateInstance()
        {
            return (UI_Text)UIPackage.CreateObject("Main", "Text");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_preview = GetControllerAt(0);
            m_txtCont = (GTextField)GetChildAt(1);
        }
    }
}