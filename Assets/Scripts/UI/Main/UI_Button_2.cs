/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_Button_2 : GButton
    {
        public Controller m_fixed;
        public const string URL = "ui://5eon00jxu1ucd";

        public static UI_Button_2 CreateInstance()
        {
            return (UI_Button_2)UIPackage.CreateObject("Main", "Button_2");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_fixed = GetControllerAt(1);
        }
    }
}