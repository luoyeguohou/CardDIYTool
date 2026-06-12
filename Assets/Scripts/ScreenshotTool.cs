using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Main;

public class ScreenshotTool : MonoBehaviour
{
    public static ScreenshotTool inst;
    private void Awake()
    {
        inst = this;
    }
    public Rect captureRect = new Rect(0, 0, 1920, 1080); // 要截的区域（屏幕坐标）

    public void Capture(Rect captureRect,string name,Card c)
    {
        this.captureRect = captureRect;
        StartCoroutine(CaptureCoroutine(name,c));
    }

    IEnumerator CaptureCoroutine(string name, Card c)
    {
        UIManager.GetType<UI_MainWin>().m_cont.m_card.SetCard(c,true);
        // 等待当前帧渲染完成（非常关键）
        yield return new WaitForEndOfFrame();

        Texture2D tex = new Texture2D(
            (int)captureRect.width,
            (int)captureRect.height,
            TextureFormat.RGB24,
            false
        );

        tex.ReadPixels(captureRect, 0, 0);
        tex.Apply();

        byte[] png = tex.EncodeToPNG();

        string path = Path.Combine(Data.work.export, name);
        File.WriteAllBytes(path, png);

        Debug.Log("保存成功: " + path);
    }
}
