using UnityEngine;

public interface IPilotCardController : ICardController {
    void InitializePilotCard(string cardName, string cardDescription, int scrapCost, int scrapRecovery,
        bool isCampEffect, Sprite imageSource, int health, Board defaultMovement, int defaultDamage = 0);
}

public class PilotCardController: CardController, IPilotCardController {
    private readonly IPilotCardView _view;
    
    [Header("Pilot Properties")]
    private int _health;
    private int _defaultDamage;
    private Board _defaultMovement;
    [Space(20)]public Vector2 position;
    
    public PilotCardController(PilotCardView view) : base(view) {
        _view = view;
    }
    
    public void InitializePilotCard(string cardName, string cardDescription, int scrapCost, int scrapRecovery,
        bool isCampEffect, Sprite imageSource, int health, Board defaultMovement, int defaultDamage = 0) {
        _health = health;
        _defaultMovement = defaultMovement;
        _defaultDamage = defaultDamage;
        
        /* Init card method called at the end because I am calling SetCardUI from it,
           and in this class I am modifying the SetCardUI*/
        InitCard(cardName, cardDescription, scrapCost, scrapRecovery, isCampEffect, imageSource);
    }

    protected override void SetCardUI()
    {
        _view.SetCardUI(CardName, CardDescription, ScrapCost, ImageSource, _health);
    }
}