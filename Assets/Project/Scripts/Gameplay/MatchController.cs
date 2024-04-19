using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;
using Photon.Pun;
using Random = UnityEngine.Random;

public interface IMatchController {
    IEnumerator PrepareMatch();
}

public class MatchController : IMatchController {
    private int _matchId;
    private List<string> _matchLog;

    private readonly IMatchView _view;

    public MatchController(IMatchView view) {
        _view = view;
    }

    private void ThrowPriorityDice() {
        if (PhotonNetwork.IsMasterClient) {
            GameManager.Instance.currentPriority = Random.Range(1, 3);
            Debug.Log($"master priority {GameManager.Instance.currentPriority}");
        }

        GameManager.Instance.OnPrioritySet(GameManager.Instance.currentPriority);

        _view.SetCurrentPhaseText($"Throwing priority dice, result = {GameManager.Instance.currentPriority}");
    }

    private void SelectQuadrant() {
        _view.SetCurrentPhaseText("Selecting quadrant");
    }

    public IEnumerator PrepareMatch() {
        yield return new WaitForSeconds(2);
        ThrowPriorityDice();
        yield return new WaitForSeconds(2);
        SelectQuadrant();
        yield return new WaitForSeconds(2);
        GameManager.Instance.PrepareForMatch(_view);
        _view.SetCurrentPhaseText("shuffling decks");
    }
}