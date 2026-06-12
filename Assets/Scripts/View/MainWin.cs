using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;
namespace Main
{
    public partial class UI_MainWin : FairyWindow
    {
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            Msg.Bind(MsgID.OnCardChanged, OnCardChanged);
            Msg.Bind(MsgID.OnCurrCardChanged, OnCurrCardChanged);
            Msg.Bind(MsgID.OnCardListChanged, UpdateCardPreview);
            
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
            m_cont.m_lstCardPreview.onClickItem.Add(() => Msg.Dispatch(MsgID.OnCurrCardChanged));

            // comp功能区
            m_cont.m_btnSetField.onClick.Add(OnClickSetField);
            m_cont.m_btnSetCompSize.onClick.Add(OnClickSetCompSize);
            m_cont.m_btnSetPosition.onClick.Add(OnClickSetCompPos);
            m_cont.m_btnSetPreviewText.onClick.Add(OnClickSetCompText);
            m_cont.m_btnSetPreviewImage.onClick.Add(OnClickSetCompImage);
            m_cont.m_btnLayerUp.onClick.Add(OnClickLayerUp);
            m_cont.m_btnLayerDown.onClick.Add(OnClickLayerDown);
        }

        public override void Dispose()
        {
            base.Dispose();
            Msg.UnBind(MsgID.OnCardChanged, OnCardChanged);
            Msg.UnBind(MsgID.OnCurrCardChanged, OnCurrCardChanged);
            Msg.UnBind(MsgID.OnCardListChanged, UpdateCardPreview);
        }

        public void Init()
        {
            m_cont.m_txtJson.text = Data.work.json;
            m_cont.m_txtCurrFolder.text = Data.work.currFolder;
            m_cont.m_txtExport.text = Data.work.export;

            UpdateCurrCardView();
            Card c = GetCurrCard();
            UI_Card cardUI = m_cont.m_card.component as UI_Card;
            if (c != null)
                cardUI.SetCard(c);
            UpdateCardPreview();
        }

        private void UpdateCardPreview(object[] p = null)
        {
            m_cont.m_lstCardPreview.numItems = Data.work.cards.Count;
        }

        private void UpdateCurrCardView()
        {
            Card c = GetCurrCard();
            m_cont.m_txtSizeHeight.text = c == null ? "" : c.height.ToString();
            m_cont.m_txtSizeWidth.text = c == null ? "" : c.width.ToString();
            m_cont.m_txtCardField.text = c == null ? "" : c.field;
            if (c == null) return;
            UI_Card cardUI = m_cont.m_card.component as UI_Card;
            cardUI.SetCard(c);
            cardUI.m_showCtrlButton.selectedIndex = c.GetCurrComp() != null ? 1 : 0;
            Comp comp = c.GetCurrComp();
            m_cont.m_txtCompPosX.text = c.GetCurrComp() == null ? "" : comp.x.ToString();
            m_cont.m_txtCompPosY.text = c.GetCurrComp() == null ? "" : comp.y.ToString();
            m_cont.m_txtCompSizeWidth.text = c.GetCurrComp() == null ? "" : comp.width.ToString();
            m_cont.m_txtCompSizeHeight.text = c.GetCurrComp() == null ? "" : comp.height.ToString();
            m_cont.m_txtField.text = c.GetCurrComp() == null ? "" : comp.field;
            m_cont.m_txtPreview.text = (c.GetCurrComp() != null && comp.type == CompType.Text) ? comp.text : "";
            if (comp != null)
                m_cont.m_currImage.selectedIndex = (comp.type == CompType.Image) ? 1 : 0;
        }

        private void OnCardChanged(object[] p = null)
        {
            Card c = (Card)p[0];
            if (c != GetCurrCard()) return;
            UpdateCurrCardView();
        }

        private void OnCurrCardChanged(object[] p = null)
        {
            UpdateCurrCardView();
        }

        // 文件功能区
        private void OnClickSaveWork()
        {
            string json = JsonUtility.ToJson(Data.work, true);
            FileUtil.SaveJson(json);
        }
        private void OnClickLoadWork()
        {
            FileUtil.LoadJson();
            Init();
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

        private bool ContainsKey(JsonData data, string key)
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
                    if (!dic.ContainsKey(key))
                    {
                        dic.Add(key, new List<JsonData>());
                    }
                    dic[key].Add(item);
                }
                foreach (var item in dic)
                {
                    Capture1ImageSet(c, item.Value, field + item.Key);
                }
            }
        }

        private void Capture1ImageSet(Card c, List<JsonData> ary, string field)
        {
            Vector2 v = FGUIUtil.GetWorldPos(m_cont.m_card);
            Rect cardRect = new Rect(v.x, UnityEngine.Screen.height - v.y - m_cont.m_card.height, m_cont.m_card.width, m_cont.m_card.height);
            int howManyInRow = Mathf.CeilToInt(Mathf.Sqrt(ary.Count));
            int imgWidth = m_cont.m_useFixedExportSize.selectedIndex == 1? int.Parse( m_cont.m_txtExportSizeWidth.text):  howManyInRow * c.width;
            int imgHeight = m_cont.m_useFixedExportSize.selectedIndex == 1 ? int.Parse(m_cont.m_txtExportSizeHeight.text) : howManyInRow * c.height;
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

        private IEnumerator CaptureCard(Texture2D tex, Rect cardRect, Rect destRect, Card c)
        {
            UI_Card cardUI = UIManager.GetType<UI_MainWin>().m_cont.m_card.component as UI_Card;
            cardUI.SetCard(c, true);
            yield return new WaitForEndOfFrame();
            tex.ReadPixels(cardRect, (int)destRect.x, (int)destRect.y);
            tex.Apply();
        }

        private IEnumerator SavePicture(Texture2D tex, string name)
        {
            yield return new WaitForEndOfFrame();
            byte[] png = tex.EncodeToPNG();
            string path = Path.Combine(Data.work.export, name);
            File.WriteAllBytes(path, png);
        }


        private List<JsonData> ToArray(JsonData data)
        {
            List<JsonData> ret = new List<JsonData>();
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
            Msg.Dispatch(MsgID.OnCardChanged, new object[1] { c });
        }
        private void OnClickAddText()
        {
            Card c = GetCurrCard();
            if (c == null) return;
            c.AddComp(Comp.NewText());
            Msg.Dispatch(MsgID.OnCardChanged, new object[1] { c });
        }

        private void OnClickDeleteComp()
        {
            Card c = GetCurrCard();
            if (c == null) return;
            if (c.GetCurrComp() == null) return;
            c.RemoveComp(c.GetCurrComp());
            Msg.Dispatch(MsgID.OnCardChanged, new object[1] { c });
        }

        private void OnClickChangeCardSize()
        {
            Card c = GetCurrCard();
            if (c == null) return;
            c.width = int.Parse(m_cont.m_txtSizeWidth.text);
            c.height = int.Parse(m_cont.m_txtSizeHeight.text);
            Msg.Dispatch(MsgID.OnCardChanged, new object[1] { c });
        }

        private void OnClickAddCard()
        {
            Card c = Card.NewCard();
            Data.work.cards.Add(c);
            if (Data.work.cards.Count == 1)
                m_cont.m_lstCardPreview.selectedIndex = 0;
            Msg.Dispatch(MsgID.OnCurrCardChanged);
            Msg.Dispatch(MsgID.OnCardListChanged);

        }

        private void OnClickDupCard()
        {
            if (GetCurrCard() == null) return;
            Card c = GetCurrCard().Copy();
            Data.work.cards.Add(c);
            Msg.Dispatch(MsgID.OnCardListChanged);
        }
        private void OnClickRemoveCard()
        {
            Card c = GetCurrCard();
            if (c == null) return;
            Data.work.cards.Remove(c);
            if (m_cont.m_lstCardPreview.selectedIndex >= Data.work.cards.Count)
                m_cont.m_lstCardPreview.selectedIndex = Data.work.cards.Count - 1;
            Msg.Dispatch(MsgID.OnCurrCardChanged);
            Msg.Dispatch(MsgID.OnCardListChanged);
        }
        private void OnClickSetCardField()
        {
            Card c = GetCurrCard();
            if (c == null) return;
            c.field = m_cont.m_txtCardField.text;
            Msg.Dispatch(MsgID.OnCardChanged, new object[1] { c });
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
            Msg.Dispatch(MsgID.OnCardChanged, new object[1] { c });
        }
        private void OnClickSetCompPos()
        {
            Card c = GetCurrCard();
            if (c == null || c.GetCurrComp() == null) return;
            c.GetCurrComp().x = int.Parse(m_cont.m_txtCompPosX.text);
            c.GetCurrComp().y = int.Parse(m_cont.m_txtCompPosY.text);
            Msg.Dispatch(MsgID.OnCardChanged, new object[1] { c });
        }
        private void OnClickSetCompText()
        {
            Card c = GetCurrCard();
            if (c == null || c.GetCurrComp() == null) return;
            c.GetCurrComp().text = m_cont.m_txtPreview.text;
            Msg.Dispatch(MsgID.OnCardChanged, new object[1] { c });
        }
        private void OnClickSetCompImage()
        {
            Card c = GetCurrCard();
            if (c == null || c.GetCurrComp() == null) return;
            Comp i = c.GetCurrComp();
            i.url = FileUtil.ChooseAImage();
            i.preview = true;
            Msg.Dispatch(MsgID.OnCardChanged, new object[1] { c });
        }
        private void OnClickLayerUp()
        {
            Card c = GetCurrCard();
            if (c == null || c.GetCurrComp() == null) return;
            c.LayerUp(c.GetCurrComp());
            Msg.Dispatch(MsgID.OnCardChanged, new object[1] { c });
        }
        private void OnClickLayerDown()
        {
            Card c = GetCurrCard();
            if (c == null || c.GetCurrComp() == null) return;
            c.LayerDown(c.GetCurrComp());
            Msg.Dispatch(MsgID.OnCardChanged, new object[1] { c });
        }

        //util
        private Card GetCurrCard()
        {
            if (m_cont.m_lstCardPreview.selectedIndex == -1) return null;
            return Data.work.cards[m_cont.m_lstCardPreview.selectedIndex];
        }
    }
}
