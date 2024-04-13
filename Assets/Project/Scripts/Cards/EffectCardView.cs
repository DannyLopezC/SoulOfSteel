using UnityEngine;

public enum Effect {
    Damage,
    Heal
}

public interface IEffectCardView : ICardView {
}

public class EffectCardView : CardView, IEffectCardView {
    private IEffectCardController _effectCardController;

    public IEffectCardController EffectCardController {
        get { return _effectCardController ??= new EffectCardController(this); }
    }

    public override void ManageLeftClick() {
        EffectCardController.Select();
    }

    public override void ManageRightClick() {
        EffectCardController.ManageRightClick();
    }

    public override void SetIsSelecting(bool isSelecting) {
        EffectCardController.IsSelecting(isSelecting);
    }

    public override CardType GetCardType() {
        return EffectCardController.GetCardType();
    }

    public override bool GetSelected() {
        return EffectCardController.GetSelected();
    }

    public override void InitCard(string cardName, string cardDescription, int scrapCost, int scrapRecovery,
        bool isCampEffect, Sprite imageSource, int health, BoardView defaultMovement, CardType type) {
    }
}