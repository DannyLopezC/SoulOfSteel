using UnityEngine;

public interface IEquipmentCardController : ICardController {
}

public class EquipmentCardController : CardController, IEquipmentCardController {
    private readonly IEquipmentCardView _view;

    public EquipmentCardController(IEquipmentCardView view) : base(view) {
        _view = view;
    }

    public override CardType GetCardType() {
        return Type;
    }
}