using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Windows.Forms;
using System.IO;

namespace Main
{
    public partial class UI_Image : GComponent
    {
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
        }

        public void SetImage(Comp i, bool justPreview)
        {
            m_preview.selectedIndex = i.preview?1: 0;
            if (!i.preview) return;
            byte[] bytes = File.ReadAllBytes(i.url);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(bytes);
            m_image.texture = new NTexture(tex);
        }
    }
}
