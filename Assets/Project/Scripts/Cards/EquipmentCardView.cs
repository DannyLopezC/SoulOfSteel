using System;
using UnityEngine;
using Random = UnityEngine.Random;

public interface IEquipmentCardView : ICardView {
}

public class EquipmentCardView : CardView, IEquipmentCardView {
    private IEquipmentCardController _equipmentCardController;

    private void Start() {
        EquipmentCardController.InitEquipmentCard("Cota de espinas",
            "Devuelve el daño",
            Random.Range(3, 7),
            Random.Range(3, 7),
            false,
            null,
            CardType.Armor);
    }

    public override bool GetSelected() {
        return EquipmentCardController.GetSelected();
    }

    public override void Select(bool deselect = false) {
        EquipmentCardController.Select(deselect);
    }

    public override void DoEffect() {
        EquipmentCardController.DoEffect();
    }

    public override void InitCard(string cardName, string cardDescription, int scrapCost, int scrapRecovery,
        bool isCampEffect, Sprite imageSource, int health, BoardView defaultMovement, CardType type) {
        EquipmentCardController.InitEquipmentCard(cardName, cardDescription, scrapCost, scrapRecovery,
            isCampEffect, imageSource, type);
    }

    public IEquipmentCardController EquipmentCardController {
        get { return _equipmentCardController ??= new EquipmentCardController(this); }
    }

    public override void ManageLeftClick() {
        EquipmentCardController.Select();
    }

    public override void ManageRightClick() {
        EquipmentCardController.ManageRightClick();
    }

    public override void SetIsSelecting(bool isSelecting) {
        EquipmentCardController.IsSelecting(isSelecting);
    }

    public override CardType GetCardType() {
        return EquipmentCardController.GetCardType();
    }
}