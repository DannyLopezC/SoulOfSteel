using UnityEngine;

// public enum CardType {
//     Pilot,
//     Weapon,
//     CampEffect
// }

public interface ICardController {
    // CardType GetCardType();
    void ManageRightClick();
}

public class CardController : ICardController {
    private readonly ICardView _view;

    private int _scrapRecovery;
    private bool _isCampEffect;
    
    protected string CardName { get; set; }
    protected string CardDescription { get; set; }
    protected int ScrapCost { get; set; }
    protected Sprite ImageSource { get; set; }

    public CardController(ICardView view) {
        _view = view;
    }

    public void InitCard(string cardName, string cardDescription, int scrapCost, int scrapRecovery, bool isCampEffect,
        Sprite imageSource) {
        CardName = cardName;
        CardDescription = cardDescription;
        ScrapCost = scrapCost;
        _scrapRecovery = scrapRecovery;
        _isCampEffect = isCampEffect;
        ImageSource = imageSource;

        SetCardUI();
    }

    protected virtual void SetCardUI() {
        _view.SetCardUI(CardName, CardDescription, ScrapCost, ImageSource);
    }

    private void ShowCard() {
        UIManager.Instance.ShowCard(true, CardName, CardDescription, ScrapCost, ImageSource);
    }

    public void ManageRightClick() {
        ShowCard();
    }

    // public abstract CardType GetCardType();
}