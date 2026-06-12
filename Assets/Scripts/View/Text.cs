using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_Text : GComponent
    {
        public void SetText(Comp t,bool justPreview) 
        { 
            if(t.text != null)
                m_txtCont.text = t.text;

            m_preview.selectedIndex = justPreview ? 1 : 0;
        }
    }
}