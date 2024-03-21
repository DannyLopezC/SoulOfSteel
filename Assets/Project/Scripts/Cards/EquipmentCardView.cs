using UnityEngine;

public interface IEquipmentCardView : ICardView {
}

public class EquipmentCardView : CardView, IEquipmentCardView {
    private IEquipmentCardController _equipmentCardController;

    public IEquipmentCardController EquipmentCardController {
        get { return _equipmentCardController ??= new EquipmentCardController(this); }
    }

    public override void ManageRightClick() {
        EquipmentCardController.ManageRightClick();
    }
}