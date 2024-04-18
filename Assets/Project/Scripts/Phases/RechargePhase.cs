using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

public class RechargePhase : Phase {
    private bool _allCardSelected;
    private List<CardView> _effectCards;

    public RechargePhase(IMatchView matchView) : base(matchView) {
        GameManager.Instance.OnCardSelectedEvent += CardSelected;
        GameManager.Instance.OnSelectingFinishedEvent += AllCardsSelected;

        _effectCards = new List<CardView>();
    }

    public override IEnumerator Start() {
        matchView.SetCurrentPhaseText("recharge phase");

        GameManager.Instance.playerList.ForEach(player => player.SelectCards(CardType.CampEffect, 1));

        while (!_allCardSelected) {
            bool localAllSelected = true;
            foreach (PlayerView player in GameManager.Instance.playerList) {
                if (!player.PlayerController.GetCardsSelected()) {
                    localAllSelected = false;
                    break;
                }
            }

            _allCardSelected = localAllSelected && GameManager.Instance.testing;

            yield return null;
        }

        foreach (CardView card in _effectCards) {
            card.DoEffect(GameManager.Instance.LocalPlayerInstance.PlayerController.GetPlayerId());
            yield return new WaitForSeconds(20);
            card.SetIsSelecting(false);
            card.Select(true);
        }

        _effectCards.Clear();
        GameManager.Instance.LocalPlayerInstance.PlayerController.SetCardsSelected(false);
        GameManager.Instance.ChangePhase(new PrincipalPhase(matchView));

        GameManager.Instance.OnCardSelectedEvent -= CardSelected;
        GameManager.Instance.OnSelectingFinishedEvent -= AllCardsSelected;
    }

    private void CardSelected(CardView card, bool selected) {
        if (selected) _effectCards.Add(card);
        else _effectCards.Remove(card);

        // if (_effectCards.Count >= 1)
        GameManager.Instance.OnSelectingFinished();
    }

    private void AllCardsSelected() {
        GameManager.Instance.LocalPlayerInstance.PlayerController.SetCardsSelected(_effectCards.Count >= 1);
    }

    public override IEnumerator End() {
        yield break;
    }
}