using System;
using UnityEngine;
using Random = UnityEngine.Random;

public interface IEquipmentCardView : ICardView {
}

public class EquipmentCardView : CardView, IEquipmentCardView {
    private IEquipmentCardController _equipmentCardController;

    private void Start() {
        EquipmentCardController.InitEquipmentCard(0, "Cota de espinas",
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

    public override void DoEffect(int originId) {
        EquipmentCardController.DoEffect(originId);
    }

    public override void InitCard(int id, string cardName, string cardDescription, int scrapCost, int scrapRecovery,
        bool isCampEffect, Sprite imageSource, int health, BoardView defaultMovement, CardType type) {
        EquipmentCardController.InitEquipmentCard(id, cardName, cardDescription, scrapCost, scrapRecovery,
            isCampEffect, imageSource, type);
    }

    public IEquipmentCardController EquipmentCardController {
        get { return _equipmentCardController ??= new EquipmentCardController(this); }
    }

    public override void ManageLeftClick() {
        EquipmentCardController.Select(false);
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