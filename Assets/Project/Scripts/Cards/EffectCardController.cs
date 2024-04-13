﻿using UnityEngine;

public interface IEffectCardController : ICardController {
    void DoEffect();
}

public class EffectCardController : CardController, IEffectCardController {
    private readonly IEffectCardView _view;

    public EffectCardController(IEffectCardView view) : base(view) {
        _view = view;
    }

    public override CardType GetCardType() {
        return Type;
    }

    public void DoEffect() {
        Debug.Log($"Doing effect");
    }
}