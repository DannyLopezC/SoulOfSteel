using UnityEngine;

public interface IEquipmentCardController : ICardController {
    void InitCard(int id, string cardName, string cardDescription,
        int scrapCost, int scrapRecovery, Sprite imageSource, CardType type);
}

public class EquipmentCardController : CardController, IEquipmentCardController {
    private readonly IEquipmentCardView _view;

    public EquipmentCardController(IEquipmentCardView view) : base(view) {
        _view = view;
    }

    public void InitCard(int id, string cardName, string cardDescription,
        int scrapCost, int scrapRecovery, Sprite imageSource, CardType type) {
        base.InitCard(id, cardName, cardDescription, scrapCost, scrapRecovery, imageSource, type);
    }

    public override CardType GetCardType() {
        return Type;
    }
}