using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Card : MonoBehaviour
{
    [SerializeField] private TMP_Text manaText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text attackText;
    [SerializeField] private Image cardLoadedImage;

    private const string IMAGE_PATH = "https://picsum.photos/";
    private int imageHeight;
    private int imageWidth;
    private int previousValue;
    private int newValue;
    private TMP_Text currentText;
    private Coroutine countingCoroutine;
    private WaitForSeconds wait = new WaitForSeconds(0.1f);

    public int Mana { get; private set; }
    public int HP { get; private set; }
    public int Attack { get; private set; }

    public void Init()
    {
        Vector2 imageSize = cardLoadedImage.rectTransform.sizeDelta;
        imageWidth = (int)imageSize.x;
        imageHeight = (int)imageSize.y;        
        string fullURL = Path.Combine(IMAGE_PATH, imageWidth.ToString(), imageHeight.ToString());
        StartCoroutine(LoadImageFromURL(fullURL));
        InitialChangeValues(1);
    }

    public void ChangeFieldsValues()
    {
        int valueForChange = Random.Range(1, 3);
        switch (valueForChange)
        {
            case 1:
                ChangeManaValue();
                break;
            case 2:
                ChangeHPValue();
                break;
            case 3:
                ChangeAttakValue();
                break;
        }
    }

    private void UpdateText(int previousValue, int newValue)
    {
        if (countingCoroutine != null)
        {
            StopCoroutine(countingCoroutine);
        }
        countingCoroutine = StartCoroutine(CountAnimation(previousValue, newValue));
    }

    private void InitialChangeValues(int minValue)
    {
        Mana = GetRandomValue(minValue);
        HP = GetRandomValue(minValue);
        Attack = GetRandomValue(minValue);
        manaText.text = Mana.ToString();
        healthText.text = HP.ToString();
        attackText.text = Attack.ToString();
    }

    private void ChangeManaValue()
    {
        previousValue = Mana;
        Mana = GetRandomValue();
        currentText = manaText;
        newValue = Mana;
        UpdateText(previousValue, newValue);
    }

    private void ChangeHPValue()
    {
        previousValue = HP;
        HP = GetRandomValue();
        currentText = healthText;
        newValue = HP;
        UpdateText(previousValue, newValue);
    }

    private void ChangeAttakValue()
    {
        previousValue = Attack;
        Attack = GetRandomValue();
        currentText = attackText;
        newValue = Attack;
        UpdateText(previousValue, newValue);
    }

    private int GetRandomValue( int minValue = -2)
    {
        int newValue = Random.Range(minValue, 9);
        return newValue;
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

    private IEnumerator CountAnimation(int previousValue, int newValue)
    {
        if (previousValue < newValue)
        {
            while (previousValue < newValue)
            {
                previousValue ++;
                if (previousValue > newValue)
                {
                    previousValue = newValue;
                }
                currentText.SetText(previousValue.ToString());
                yield return wait;
            }
        }
        else
        {
            while (previousValue > newValue)
            {
                previousValue --;
                if (previousValue < newValue)
                {
                    previousValue = newValue;
                }
                currentText.SetText(previousValue.ToString());
                yield return wait;
            }
        }
    }
}
