/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_Main : GComponent
    {
        public Controller m_currImage;
        public Controller m_useFixedExportSize;
        public GTextField m_txtExport;
        public GButton m_btnAddImage;
        public GButton m_btnAddText;
        public GButton m_btnLayerUp;
        public GButton m_btnLayerDown;
        public GTextField m_txtCurrFolder;
        public GButton m_btnChooseJson;
        public GTextField m_txtJson;
        public GButton m_btnDraw;
        public GButton m_btnSetSize;
        public GTextInput m_txtSizeWidth;
        public GTextInput m_txtSizeHeight;
        public GTextInput m_txtField;
        public GButton m_btnSetField;
        public GTextInput m_txtCompPosX;
        public GTextInput m_txtCompPosY;
        public GTextInput m_txtCompSizeWidth;
        public GTextInput m_txtCompSizeHeight;
        public GButton m_btnSetPosition;
        public GButton m_btnSetCompSize;
        public GButton m_btnSetPreviewImage;
        public GButton m_btnSetPreviewText;
        public GTextInput m_txtPreview;
        public GButton m_btnChangeCurrFolder;
        public GButton m_btnChangeExport;
        public GButton m_btnAddCard;
        public GButton m_btnRemoveCard;
        public GTextInput m_txtCardField;
        public GButton m_btnSetCardField;
        public GList m_lstCardPreview;
        public GButton m_btnSaveWork;
        public GButton m_btnLoadWork;
        public GButton m_btnDupCard;
        public GButton m_btnDeleteComp;
        public GLoader m_card;
        public GTextInput m_txtExportSizeWidth;
        public GTextInput m_txtExportSizeHeight;
        public const string URL = "ui://5eon00jxvgvm0";

        public static UI_Main CreateInstance()
        {
            return (UI_Main)UIPackage.CreateObject("Main", "Main");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_currImage = GetControllerAt(0);
            m_useFixedExportSize = GetControllerAt(1);
            m_txtExport = (GTextField)GetChildAt(5);
            m_btnAddImage = (GButton)GetChildAt(6);
            m_btnAddText = (GButton)GetChildAt(7);
            m_btnLayerUp = (GButton)GetChildAt(8);
            m_btnLayerDown = (GButton)GetChildAt(9);
            m_txtCurrFolder = (GTextField)GetChildAt(12);
            m_btnChooseJson = (GButton)GetChildAt(13);
            m_txtJson = (GTextField)GetChildAt(14);
            m_btnDraw = (GButton)GetChildAt(15);
            m_btnSetSize = (GButton)GetChildAt(16);
            m_txtSizeWidth = (GTextInput)GetChildAt(18);
            m_txtSizeHeight = (GTextInput)GetChildAt(20);
            m_txtField = (GTextInput)GetChildAt(22);
            m_btnSetField = (GButton)GetChildAt(23);
            m_txtCompPosX = (GTextInput)GetChildAt(25);
            m_txtCompPosY = (GTextInput)GetChildAt(27);
            m_txtCompSizeWidth = (GTextInput)GetChildAt(29);
            m_txtCompSizeHeight = (GTextInput)GetChildAt(31);
            m_btnSetPosition = (GButton)GetChildAt(32);
            m_btnSetCompSize = (GButton)GetChildAt(33);
            m_btnSetPreviewImage = (GButton)GetChildAt(34);
            m_btnSetPreviewText = (GButton)GetChildAt(35);
            m_txtPreview = (GTextInput)GetChildAt(37);
            m_btnChangeCurrFolder = (GButton)GetChildAt(38);
            m_btnChangeExport = (GButton)GetChildAt(39);
            m_btnAddCard = (GButton)GetChildAt(42);
            m_btnRemoveCard = (GButton)GetChildAt(43);
            m_txtCardField = (GTextInput)GetChildAt(44);
            m_btnSetCardField = (GButton)GetChildAt(45);
            m_lstCardPreview = (GList)GetChildAt(46);
            m_btnSaveWork = (GButton)GetChildAt(47);
            m_btnLoadWork = (GButton)GetChildAt(48);
            m_btnDupCard = (GButton)GetChildAt(49);
            m_btnDeleteComp = (GButton)GetChildAt(50);
            m_card = (GLoader)GetChildAt(51);
            m_txtExportSizeWidth = (GTextInput)GetChildAt(53);
            m_txtExportSizeHeight = (GTextInput)GetChildAt(55);
        }
    }
}