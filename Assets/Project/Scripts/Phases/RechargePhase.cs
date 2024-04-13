using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

public class RechargePhase : Phase {
    public RechargePhase(IMatchView matchView) : base(matchView) {
        GameManager.Instance.OnSelectingFinishedEvent += CardsSelected;
    }

    public override IEnumerator Start() {
        matchView.SetCurrentPhaseText("recharge phase");

        yield return new WaitForSeconds(3);

        GameManager.Instance.playerList.ForEach(player => player.SelectCards(CardType.CampEffect, 1));

        yield break;
    }

    private void CardsSelected(List<CardView> cards) {
        List<PilotCardView> effectCards = cards.OfType<PilotCardView>().ToList();
        effectCards.ForEach(card => card.PilotCardController.DoEffect());
        effectCards.ForEach(card => card.PilotCardController.Select(true));
        effectCards.ForEach(card => card.SetIsSelecting(false));
        GameManager.Instance.ChangePhase(new PrincipalPhase(matchView));
    }

    public override IEnumerator End() {
        yield break;
    }
}