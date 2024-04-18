using UnityEngine;

public interface IEffectCardController : ICardController {
}

public class EffectCardController : CardController, IEffectCardController {
    private readonly IEffectCardView _view;

    public EffectCardController(IEffectCardView view) : base(view) {
        _view = view;
    }

    public override CardType GetCardType() {
        return Type;
    }

    public override void DoEffect(int originId) {
        base.DoEffect(originId);
        Debug.Log($"putting mines");

        switch (Id) {
            case 0:
                EffectManager.Instance.PutMines(originId, 3);
                break;
        }
    }
}