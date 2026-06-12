using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Windows.Forms;
using LitJson;
using System.IO;
public class Test : MonoBehaviour
{
    private void Start()
    {
        OpenFolder();
        OpenFolder();
    }
    public void OpenFolder()
    {
        string path = "C://Users/gaohaochen/Desktop/res/design.json";
        string json = File.ReadAllText(path);

        JsonData data = JsonMapper.ToObject(json);

        List<string> arrayKeys = new List<string>();

        if (data.IsObject)
        {
            // ½« JsonData ×ª³É IDictionary
            IDictionary dict = data as IDictionary;
            if (dict != null)
            {
                foreach (object key in dict.Keys)
                {
                    arrayKeys.Add(key.ToString());
                }
            }
        }

    }
}
