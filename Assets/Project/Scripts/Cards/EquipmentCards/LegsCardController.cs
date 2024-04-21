using System.Collections.Generic;
using UnityEngine;

public interface ILegsCardController : IEquipmentCardController {
    void InitCard(int id, string cardName, string cardDescription,
        int scrapCost, int scrapRecovery, List<Movement> movements,
        Sprite imageSource, CardType type);
}

public class LegsCardController : EquipmentCardController, ILegsCardController {
    private readonly ILegsCardView _view;

    private List<Movement> _movements;

    public LegsCardController(ILegsCardView view) : base(view) {
        _view = view;
    }

    public void InitCard(int id, string cardName, string cardDescription,
        int scrapCost, int scrapRecovery, List<Movement> movements,
        Sprite imageSource, CardType type) {
        _movements = movements;

        base.InitCard(id, cardName, cardDescription, scrapCost, scrapRecovery, imageSource, type);
    }
}