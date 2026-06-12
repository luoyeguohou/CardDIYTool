/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_CardPreview : GButton
    {
        public UI_Card m_card;
        public GButton m_btnSetCardField;
        public const string URL = "ui://5eon00jx8xna9";

        public static UI_CardPreview CreateInstance()
        {
            return (UI_CardPreview)UIPackage.CreateObject("Main", "CardPreview");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_card = (UI_Card)GetChildAt(1);
            m_btnSetCardField = (GButton)GetChildAt(2);
        }
    }
}