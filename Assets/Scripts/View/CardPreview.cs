using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_CardPreview : GButton
    {
        public void SetCard(Card c)
        {
            m_card.SetCard(c, true);
            m_btnSetCardField.title = c.field;
        }
    }
}