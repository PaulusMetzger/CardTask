using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardManager : MonoBehaviour
{
    [SerializeField, Min(0), Header("ставится 0 для случайного числа карт")] private int cardQuantaty;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private RectTransform cardAnchor;
    [SerializeField, Header ("максимальный угол поворота крайней карты")] private float maximumAngle;
    [SerializeField, Header("максимальный сдвиг карты по вертикали")] private float maximumVeticalOffset;

    private GameObject newCard;
    private List<Card> cardsList = new List<Card>();
    private const float LOCATION_COEFFICENT = 0.042f;
    private int currentActiveCard;

    private void Start()
    {
        int quantity = Random.Range(4, 7);
        if (cardQuantaty == 0)
        {
            cardQuantaty = quantity;
        }        

        for (int i=0; i < cardQuantaty; i++)
        {
            newCard = Instantiate(cardPrefab, cardAnchor);
            Card card = newCard.GetComponent<Card>();
            card.Init();
            cardsList.Add(card);
            float offset = GetOffset(i);    
            float verticalOffset = GetVerticalOffset(offset);
            Vector3 cardLocalPosition = newCard.transform.localPosition;
            newCard.transform.localPosition = new Vector2(cardLocalPosition.x + offset,
                cardLocalPosition.y + verticalOffset);

            float angle = GetAngle(offset);
            newCard.transform.eulerAngles = new Vector3(newCard.transform.eulerAngles.x, newCard.transform.eulerAngles.y, angle);
        }
    }

    public void ChangeActiveCardValue()
    {
        if (currentActiveCard > cardsList.Count - 1)
        {
            currentActiveCard = 0;
        }
        Card currentCard = cardsList[currentActiveCard];
        currentCard.ChangeFieldsValues();
        if (currentCard.HP <= 0)
        {
            cardsList.Remove(currentCard);
            Destroy(currentCard.gameObject);
            ChangeCurrentCardsPosition();
            return;
        }
        currentActiveCard++;
    }

    private float GetOffset(int i)
    {
        int steps = cardQuantaty - 1;
        if (steps == 0)
        {
            return 0;
        }        

        float stepSize = Screen.width * LOCATION_COEFFICENT;
        float stepLenght = stepSize * steps;
        float anchorOffset = -stepLenght / 2 + stepSize * i;
        return anchorOffset;
    }

    private float GetAngle(float offset)
    {
        float angle = - offset * maximumAngle / (Screen.width * 0.5f);
        return angle;
    }

    private float GetVerticalOffset(float offset)
    {
        float verticalOffset = 0;
        verticalOffset = offset < 0 ? offset * maximumVeticalOffset / (Screen.width * 0.5f)
            : verticalOffset = -offset * maximumVeticalOffset / (Screen.width * 0.5f);
        return verticalOffset;
    }

    private void ChangeCurrentCardsPosition()
    {
        for(int i = 0; i < cardsList.Count; i++)
        {
            Transform cardTransform = cardsList[i].gameObject.transform;
            Vector3 currentPosition = cardTransform.localPosition;
            cardQuantaty= cardsList.Count;
            float horizontalOffset = GetOffset(i);
            float verticalOffset = GetVerticalOffset(horizontalOffset);
            Vector3 newCardPosition = new Vector3(horizontalOffset, verticalOffset, currentPosition.z);
            DOTween.To(() => cardTransform.localPosition, x => cardTransform.localPosition = x, newCardPosition, 0.5f);

            float currentAngle = cardTransform.eulerAngles.z;
            float newAngle = GetAngle(horizontalOffset);
            Vector3 newEulerAngles = new Vector3(cardTransform.eulerAngles.x, cardTransform.eulerAngles.y, newAngle);
            cardTransform.DORotate(newEulerAngles, 1f);
        }
    }    
}
