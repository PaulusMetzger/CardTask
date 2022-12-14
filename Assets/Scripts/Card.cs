using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Card : MonoBehaviour
{
    [SerializeField] private TMP_Text mana;
    [SerializeField] private TMP_Text health;
    [SerializeField] private TMP_Text attack;
    [SerializeField] private Image cardLoadedImage;

    private const string IMAGE_PATH = "https://picsum.photos/";
    private int imageHeight;
    private int imageWidth;

    public void Init()
    {
        Vector2 imageSize = cardLoadedImage.rectTransform.sizeDelta;
        imageWidth = (int)imageSize.x;
        imageHeight = (int)imageSize.y;        
        string fullURL = Path.Combine(IMAGE_PATH, imageWidth.ToString(), imageHeight.ToString());
        StartCoroutine(LoadImageFromURL(fullURL));
    }   

    public void ChangeFieldsValues()
    {        
        mana.text = GetRandomValue();
        health.text = GetRandomValue();
        attack.text = GetRandomValue();
    }

    private string GetRandomValue()
    {
        int newValue = Random.Range(-2, 9);
        return newValue.ToString();
    }

    private IEnumerator LoadImageFromURL(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError(www.error);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2());
            cardLoadedImage.sprite = sprite;
        }
    }
}
