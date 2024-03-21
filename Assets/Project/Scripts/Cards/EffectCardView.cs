﻿using UnityEngine;

public interface IEffectCardView : ICardView {
}

public class EffectCardView : CardView, IEffectCardView {
    private IEffectCardController _effectCardController;

    public IEffectCardController EffectCardController {
        get { return _effectCardController ??= new EffectCardController(this); }
    }

    public override void ManageRightClick() {
        EffectCardController.ManageRightClick();
    }
}