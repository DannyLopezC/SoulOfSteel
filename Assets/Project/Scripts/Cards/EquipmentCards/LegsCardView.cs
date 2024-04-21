using System.Collections.Generic;
using UnityEngine;

public interface ILegsCardView : IEquipmentCardView {
}

public class LegsCardView : EquipmentCardView, ILegsCardView {
    private ILegsCardController _legsCardController;

    public ILegsCardController LegsCardController {
        get { return _legsCardController ??= new LegsCardController(this); }
    }

    public void InitCard(int id, string cardName, string cardDescription, int scrapCost, int scrapRecovery,
        List<Movement> movements, Sprite imageSource, CardType type) {
        LegsCardController.InitCard(id, cardName, cardDescription, scrapCost,
            scrapRecovery, movements, imageSource, type);
    }

    public override void ManageLeftClick() {
        LegsCardController.Select(false);
    }

    public override void ManageRightClick() {
        LegsCardController.ManageRightClick();
    }

    public override void SetIsSelecting(bool isSelecting) {
        LegsCardController.IsSelecting(isSelecting);
    }

    public override CardType GetCardType() {
        return LegsCardController.GetCardType();
    }

    public override bool GetSelected() {
        return LegsCardController.GetSelected();
    }

    public override void Select(bool deselect = false) {
        LegsCardController.Select(deselect);
    }

    public override void DoEffect(int originId) {
        LegsCardController.DoEffect(originId);
    }
}