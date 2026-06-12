/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_Image : GComponent
    {
        public Controller m_preview;
        public GLoader m_image;
        public const string URL = "ui://5eon00jxivra4";

        public static UI_Image CreateInstance()
        {
            return (UI_Image)UIPackage.CreateObject("Main", "Image");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_preview = GetControllerAt(0);
            m_image = (GLoader)GetChildAt(0);
        }
    }
}