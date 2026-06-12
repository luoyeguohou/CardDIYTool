using LitJson;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Data
{
    public static Work work = new Work();
}

[System.Serializable]
public class Work {
    public string json = "";
    public string currFolder = "c://";
    public string export = "c://";
    public List<Card> cards = new List<Card>();
}

[System.Serializable]
public class Card
{
    public string field = "";
    public List<Comp> comps = new List<Comp>();
    public int height;
    public int width;
    public int selectedIndex = -1;

    public Card Copy()
    {
        Card card = new Card();
        card.height = height;
        card.width = width;
        card.field = field;
        card.comps = new List<Comp>();
        foreach (Comp comp in comps)
        {
            Comp comp1 = new Comp();
            comp1.field = comp.field;
            comp1.height = comp.height;
            comp1.width = comp.width;
            comp1.y = comp.y;
            comp1.x = comp.x;
            comp1.type = comp.type;
            if (comp.type == CompType.Image)
            {
                comp1.url = comp.url;
                comp1.preview = comp1.url != "";
            }
            else
            {
                comp1.text = comp.text;
            }
            card.comps.Add(comp1);
        }
        return card;
    }

    public void SetData(JsonData cardData)
    {
        foreach (Comp comp in comps)
        {
            if (comp.type == CompType.Image)
            {
                comp.url = Data.work.currFolder + "/" + cardData[comp.field].ToString();
                comp.preview = comp.url != "";
            }
            else
            {
                comp.text = cardData[comp.field].ToString();
            }
        }
    }
    public void AddComp(Comp c)
    {
        comps.Add(c);
        if (selectedIndex == -1)
        {
            selectedIndex = 0;
        }
    }

    public void RemoveComp(Comp c)
    {
        if (selectedIndex >= comps.IndexOf(c)) {
            selectedIndex--;
        }
        Debug.Log(selectedIndex);
        comps.Remove(c);
    }

    public Comp GetCurrComp()
    {
        if (comps.Count == 0) { return null; }
        if (selectedIndex == -1) selectedIndex = 0;
        return comps[selectedIndex];
    }

    public static Card NewCard()
    {
        Card c = new Card();
        c.width = 650;
        c.height = 900;
        return c;
    }

    public void LayerUp(Comp c)
    {
        int index = comps.IndexOf(c);
        if (index == -1 || index == comps.Count - 1) return;
        (comps[index + 1], comps[index]) = (comps[index], comps[index + 1]);
        if (selectedIndex == index) selectedIndex++;
        else if (selectedIndex == index + 1) selectedIndex--;
    }

    public void LayerDown(Comp c)
    {
        int index = comps.IndexOf(c);
        if (index == -1 || index == 0) return;
        (comps[index - 1], comps[index]) = (comps[index], comps[index - 1]);
        if (selectedIndex == index) selectedIndex--;
        else if(selectedIndex == index - 1) selectedIndex++;
    }
}
[System.Serializable]
public class Comp {
    public int x;
    public int y;
    public int height;
    public int width;
    public string field = "";
    public CompType type;
    public bool preview;
    public string url = "";
    public string text = "";
    public static Comp NewComp()
    {
        Comp i = new Comp();
        i.x = 0; i.y = 0;
        i.width = 100; i.height = 100;
        return i;
    }

    public static Comp NewImage()
    {
       Comp c = NewComp();
        c.type = CompType.Image;
        return c;
    }

    public static Comp NewText()
    {
        Comp c = NewComp();
        c.type = CompType.Text;
        return c;
    }
}

public enum CompType { 
    Image,
    Text,
}
