using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

public class RechargePhase : Phase {
    private List<CardView> _effectCards;

    public RechargePhase(IMatchView matchView) : base(matchView) {
        GameManager.Instance.OnCardSelectedEvent += CardSelected;
        GameManager.Instance.OnSelectingFinishedEvent += AllCardsSelected;

        _effectCards = new List<CardView>();
    }

    public override IEnumerator Start() {
        matchView.SetCurrentPhaseText("recharge phase");

        yield return new WaitForSeconds(3);

        GameManager.Instance.playerList.ForEach(player => player.SelectCards(CardType.CampEffect, 1));
    }

    private void CardSelected(CardView card, bool selected) {
        if (selected) _effectCards.Add(card);
        else _effectCards.Remove(card);

        if (_effectCards.Count >= 1) GameManager.Instance.OnSelectingFinished();
    }

    private void AllCardsSelected() {
        foreach (CardView card in _effectCards) {
            card.DoEffect();
            card.SetIsSelecting(false);
            card.Select(true);
        }

        _effectCards.Clear();
        GameManager.Instance.ChangePhase(new PrincipalPhase(matchView));

        GameManager.Instance.OnCardSelectedEvent -= CardSelected;
        GameManager.Instance.OnSelectingFinishedEvent -= AllCardsSelected;
    }

    public override IEnumerator End() {
        yield break;
    }
}