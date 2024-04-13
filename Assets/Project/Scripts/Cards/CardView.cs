using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public interface ICardView {
    void SetCardUI(string cardName, string cardDescription, int scrapCost, Sprite imageSource);

    GameObject GetGameObject();
}

[Serializable]
public abstract class CardView : MonoBehaviour, ICardView, IPointerClickHandler {
    [SerializeField, BoxGroup("Card UI Components")]
    private TMP_Text nameTMP;

    [SerializeField, BoxGroup("Card UI Components")]
    private TMP_Text descriptionTMP;

    [SerializeField, BoxGroup("Card UI Components")]
    private TMP_Text scrapCostTMP;

    [SerializeField, BoxGroup("Card UI Components")]
    private Image imageSourceIMG;

    public virtual void SetCardUI(string cardName, string cardDescription, int scrapCost, Sprite imageSource) {
        nameTMP.text = cardName;
        descriptionTMP.text = cardDescription;
        scrapCostTMP.text = $"{scrapCost}";
        imageSourceIMG.sprite = imageSource;
    }

    public GameObject GetGameObject() {
        return gameObject;
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Right) {
            ManageRightClick();
        }

        if (eventData.button == PointerEventData.InputButton.Left) {
            ManageLeftClick();
        }
    }

    public abstract void ManageLeftClick();
    public abstract void ManageRightClick();
    public abstract void SetIsSelecting(bool isSelecting);
    public abstract CardType GetCardType();
    public abstract bool GetSelected();

    public abstract void InitCard(string cardName, string cardDescription, int scrapCost, int scrapRecovery,
        bool isCampEffect, Sprite imageSource, int health, BoardView defaultMovement, CardType type);
}