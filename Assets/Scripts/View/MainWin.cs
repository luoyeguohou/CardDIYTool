using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using UnityEngine;
using LitJson;
using System.Net.Http.Headers;
namespace Main
{
    public partial class UI_MainWin : FairyWindow
    {
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            // 文件功能区
            m_cont.m_btnChooseJson.onClick.Add(OnClickChooseJson);
            m_cont.m_btnChangeCurrFolder.onClick.Add(OnClickChooseCurrFolder);
            m_cont.m_btnChangeExport.onClick.Add(OnClickChooseExport);
            m_cont.m_btnDraw.onClick.Add(OnClickPaint);
            m_cont.m_btnSaveWork.onClick.Add(OnClickSaveWork);
            m_cont.m_btnLoadWork.onClick.Add(OnClickLoadWork);

            // 图层功能区
            m_cont.m_btnAddImage.onClick.Add(OnClickAddImage);
            m_cont.m_btnAddText.onClick.Add(OnClickAddText);
            m_cont.m_btnDeleteComp.onClick.Add(OnClickDeleteComp);
            m_cont.m_btnSetSize.onClick.Add(OnClickChangeCardSize);
            m_cont.m_btnAddCard.onClick.Add(OnClickAddCard);
            m_cont.m_btnDupCard.onClick.Add(OnClickDupCard);
            m_cont.m_btnRemoveCard.onClick.Add(OnClickRemoveCard);
            m_cont.m_btnSetCardField.onClick.Add(OnClickSetCardField);
            m_cont.m_lstCardPreview.itemRenderer = CardPreviewIR;
            m_cont.m_lstCardPreview.onClickItem.Add(UpdateView);

            // comp功能区
            m_cont.m_btnSetField.onClick.Add(OnClickSetField);
            m_cont.m_btnSetCompSize.onClick.Add(OnClickSetCompSize);
            m_cont.m_btnSetPosition.onClick.Add(OnClickSetCompPos);
            m_cont.m_btnSetPreviewText.onClick.Add(OnClickSetCompText);
            m_cont.m_btnSetPreviewImage.onClick.Add(OnClickSetCompImage);
            m_cont.m_btnLayerUp.onClick.Add(OnClickLayerUp);
            m_cont.m_btnLayerDown.onClick.Add(OnClickLayerDown);
        }

        public void Init() {
            UpdateView();
        }

        public void UpdateView() {
            Debug.Log("UpdateView");
            m_cont.m_txtJson.text = Data.work.json;
            m_cont.m_txtCurrFolder.text = Data.work.currFolder;
            m_cont.m_txtExport.text = Data.work.export;
            Card c = GetCurrCard();
            if (c != null)
            {
                m_cont.m_card.SetCard(c);
                m_cont.m_txtSizeHeight.text = c.height.ToString();
                m_cont.m_txtSizeWidth.text = c.width.ToString();
                m_cont.m_txtCardField.text = c.field;
                m_cont.m_card.m_showCtrlButton.selectedIndex = c.GetCurrComp() != null?1:0;
                if (c.GetCurrComp() != null)
                {
                    Comp comp = c.GetCurrComp();
                    m_cont.m_currImage.selectedIndex = (comp.type == CompType.Image) ? 1 : 0;
                    m_cont.m_txtCompPosX.text = comp.x.ToString();
                    m_cont.m_txtCompPosY.text = comp.y.ToString();
                    m_cont.m_txtCompSizeWidth.text = comp.width.ToString();
                    m_cont.m_txtCompSizeHeight.text = comp.height.ToString();
                    m_cont.m_txtField.text = comp.field;
                    m_cont.m_txtPreview.text = (comp.type == CompType.Text) ? comp.text : "";
                }
                else 
                {
                    m_cont.m_txtCompPosX.text = "";
                    m_cont.m_txtCompPosY.text = "";
                    m_cont.m_txtCompSizeWidth.text = "";
                    m_cont.m_txtCompSizeHeight.text = "";
                    m_cont.m_txtField.text = "";
                    m_cont.m_txtPreview.text = "";
                }
            }
            else {
                m_cont.m_txtSizeHeight.text = "";
                m_cont.m_txtSizeWidth.text = "";
                m_cont.m_txtCardField.text = "";
            }
            m_cont.m_lstCardPreview.numItems = Data.work.cards.Count; 
        }

        // 文件功能区
        private void OnClickSaveWork()
        {
            string json = JsonUtility.ToJson(Data.work, true);
            FileUtil.SaveJson(json);
        }
        private void OnClickLoadWork() {
            FileUtil.LoadJson();
            UpdateView();
        }

        private void OnClickChooseJson()
        {
            Data.work.json = FileUtil.ChooseAJsonFile();
            m_cont.m_txtJson.text = Data.work.json;
        }
        private void OnClickChooseCurrFolder()
        {
            Data.work.currFolder = FileUtil.ChooseAFolder();
            m_cont.m_txtCurrFolder.text = Data.work.currFolder;
        }
        private void OnClickChooseExport()
        {
            Data.work.export = FileUtil.ChooseAFolder();
            m_cont.m_txtExport.text = Data.work.export;
        }
        private void OnClickPaint()
        {
            string json = File.ReadAllText(Data.work.json);
            JsonData data = JsonMapper.ToObject(json);
            foreach (Card c in Data.work.cards)
            {
                CaptureCards(c, ToArray(data[c.field]), c.field);
            }
        }

        private bool ContainsKey(JsonData data,string key)
        {
            bool hasKey;
            try
            {
                var temp = data[key];
                hasKey = true;
            }
            catch
            {
                hasKey = false;
            }

            return hasKey;
        }

        private void CaptureCards(Card c, List<JsonData> ary, string field)
        {
            if (!ContainsKey(ary[0], "set"))
            {
                Capture1ImageSet(c, ary, field);
            }
            else 
            {
                Dictionary<string, List<JsonData>> dic = new Dictionary<string, List<JsonData>>();
                foreach (var item in ary)
                {
                    string key = item["set"].ToString();
                    if (!dic.ContainsKey(key)) {
                        dic.Add(key,new List<JsonData>());   
                    }
                    dic[key].Add(item);
                }
                foreach (var item in dic)
                {
                    Capture1ImageSet(c, item.Value, field+item.Key);
                }
            }
        }

        private void Capture1ImageSet(Card c,List<JsonData> ary, string field) {
            Vector2 v = FGUIUtil.GetWorldPos(m_cont.m_card);
            Rect cardRect = new Rect(v.x, UnityEngine.Screen.height - v.y - m_cont.m_card.height, m_cont.m_card.width, m_cont.m_card.height);
            int howManyInRow = Mathf.CeilToInt(Mathf.Sqrt(ary.Count));
            int imgWidth = howManyInRow * c.width;
            int imgHeight = howManyInRow * c.height;
            Texture2D tex = new Texture2D(imgWidth, imgHeight, TextureFormat.RGB24, false);

            for (int i = 0; i < ary.Count; i++)
            {
                int x = i % howManyInRow;
                int y = i / howManyInRow;
                JsonData cardData = ary[i];
                c.SetData(cardData);
                Rect destRect = new Rect(x * c.width, (howManyInRow - y - 1) * c.height, c.width, c.height);
                CoroutineQueue.inst.Enqueue(CaptureCard(tex, cardRect, destRect, c.Copy()));
            }
            CoroutineQueue.inst.Enqueue(SavePicture(tex, field + ".png"));
        }

        private IEnumerator CaptureCard(Texture2D tex, Rect cardRect, Rect destRect, Card c) {
            UIManager.GetType<UI_MainWin>().m_cont.m_card.SetCard(c, true);
            yield return new WaitForEndOfFrame();
            tex.ReadPixels(cardRect,(int)destRect.x, (int)destRect.y);
            tex.Apply();
        }

        private IEnumerator SavePicture(Texture2D tex,string name)
        {
            yield return new WaitForEndOfFrame();
            byte[] png = tex.EncodeToPNG();
            string path = Path.Combine(Data.work.export, name);
            File.WriteAllBytes(path, png);
        }


        private List<JsonData> ToArray(JsonData data) {
            List < JsonData > ret = new List < JsonData >();
            if (data.IsArray)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    JsonData element = data[i];
                    ret.Add(element);
                }
            }
            return ret;
        }

        // 图层功能区
        private void OnClickAddImage()
        {
            Card c = GetCurrCard();
            if (c == null) return;
            c.AddComp(Comp.NewImage());
            UpdateView();
        }
        private void OnClickAddText()
        {
            Card c = GetCurrCard();
            if (c == null) return;
            c.AddComp(Comp.NewText());
            UpdateView();
        }

        private void OnClickDeleteComp()
        {
            Card c = GetCurrCard();
            if (c == null) return;
            if (c.GetCurrComp() == null) return;
            c.RemoveComp(c.GetCurrComp());
            UpdateView();
        }

        private void OnClickChangeCardSize()
        {
            Card c = GetCurrCard();
            if (c == null) return;
            c.width = int.Parse(m_cont.m_txtSizeWidth.text);
            c.height = int.Parse(m_cont.m_txtSizeHeight.text);
            UpdateView();
        }

        private void OnClickAddCard()
        {
            Data.work.cards.Add(Card.NewCard());
            UpdateView();
            if (Data.work.cards.Count == 1)
                m_cont.m_lstCardPreview.selectedIndex = 0;
        }

        private void OnClickDupCard()
        {
            if (GetCurrCard() == null) return;
            Data.work.cards.Add(GetCurrCard().Copy());
            UpdateView();
            if (Data.work.cards.Count == 1)
                m_cont.m_lstCardPreview.selectedIndex = 0;
        }
        private void OnClickRemoveCard()
        {
            Card c = GetCurrCard();
            if (c == null) return;
            Data.work.cards.Remove(c);
            if (m_cont.m_lstCardPreview.selectedIndex >= Data.work.cards.Count)
                m_cont.m_lstCardPreview.selectedIndex = Data.work.cards.Count - 1;
            UpdateView();
        }
        private void OnClickSetCardField()
        {
            Card c = GetCurrCard();
            if (c == null) return;
            c.field = m_cont.m_txtCardField.text;
            UpdateView();
        }

        private void CardPreviewIR(int index, GObject g)
        {
            UI_CardPreview c = g as UI_CardPreview;
            c.SetCard(Data.work.cards[index]);
        }

        // comp功能区
        private void OnClickSetField()
        {
            Card c = GetCurrCard();
            if (c == null || c.GetCurrComp() == null) return;
            c.GetCurrComp().field = m_cont.m_txtField.text;
        }
        private void OnClickSetCompSize()
        {
            Card c = GetCurrCard();
            if (c == null || c.GetCurrComp() == null) return;
            c.GetCurrComp().width = int.Parse(m_cont.m_txtCompSizeWidth.text);
            c.GetCurrComp().height = int.Parse(m_cont.m_txtCompSizeHeight.text);
            UpdateView();
        }
        private void OnClickSetCompPos()
        {
            Card c = GetCurrCard();
            if (c == null|| c.GetCurrComp() == null) return;
            c.GetCurrComp().x = int.Parse(m_cont.m_txtCompPosX.text);
            c.GetCurrComp().y = int.Parse(m_cont.m_txtCompPosY.text);
            UpdateView();
        }
        private void OnClickSetCompText()
        {
            Card c = GetCurrCard();
            if (c == null || c.GetCurrComp() == null) return;
            c.GetCurrComp().text = m_cont.m_txtPreview.text;
            UpdateView();
        }
        private void OnClickSetCompImage()
        {
            Card c = GetCurrCard();
            if (c == null || c.GetCurrComp() == null) return;
            Comp i = c.GetCurrComp();
            i.url = FileUtil.ChooseAImage();
            i.preview = true;
            UpdateView();
        }
        private void OnClickLayerUp()
        {
            Card c = GetCurrCard();
            if (c == null || c.GetCurrComp() == null) return;
            c.LayerUp(c.GetCurrComp());
            UpdateView();
        }
        private void OnClickLayerDown()
        {
            Card c = GetCurrCard();
            if (c == null || c.GetCurrComp() == null) return;
            c.LayerDown(c.GetCurrComp());
            UpdateView();
        }

        //util
        private Card GetCurrCard()
        {
            if(m_cont.m_lstCardPreview.selectedIndex == -1) return null;
            return Data.work.cards[m_cont.m_lstCardPreview.selectedIndex];
        }
    }
}
