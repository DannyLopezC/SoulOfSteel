using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public interface ICardView {
    void SetCardUI(string cardName, string cardDescription, int scrapCost, Sprite imageSource);
}

[Serializable]
public class CardView : MonoBehaviour, ICardView, IPointerClickHandler {
    [SerializeField, BoxGroup("Card UI Components")]
    private TMP_Text nameTMP;
    [SerializeField, BoxGroup("Card UI Components")]
    private TMP_Text descriptionTMP;
    [SerializeField, BoxGroup("Card UI Components")]
    private TMP_Text scrapCostTMP;
    [SerializeField, BoxGroup("Card UI Components")]
    private Image imageSourceIMG;
    
    private ICardController _cardController;

    public ICardController CardController {
        get { return _cardController ??= new CardController(this); }
    }
    
    public virtual void SetCardUI(string cardName, string cardDescription, int scrapCost, Sprite imageSource) {
        nameTMP.text = cardName;
        descriptionTMP.text = cardDescription;
        scrapCostTMP.text = $"{scrapCost}";
        imageSourceIMG.sprite = imageSource;
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Right) {
            CardController.ManageRightClick();
        }
    }
}