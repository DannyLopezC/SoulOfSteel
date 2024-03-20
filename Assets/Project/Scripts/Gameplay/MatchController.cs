using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

public interface IMatchController {
    IEnumerator PrepareMatch();
    IEnumerator ExecutePhases();
}

public class MatchController : IMatchController {
    private int _matchId;
    private List<string> _matchLog;
    
    private readonly IMatchView _view;
    
    public MatchController(IMatchView view) {
        _view = view;
    }

    private void ThrowPriorityDice() {
        GameManager.Instance.currentPriority = Random.Range(0, 1);
        _view.SetCurrentPhaseText($"Throwing priority dice, result={GameManager.Instance.currentPriority}");
    }

    private void ChangePhase() {
        _view.SetCurrentPhaseText("Changing Phase");

        int currentPhaseIndex = (int)GameManager.Instance.currentPhase;
        int totalPhases = Enum.GetNames(typeof(Phases)).Length;

        int nextPhaseIndex = (currentPhaseIndex + 1) % totalPhases;
        GameManager.Instance.ChangePhase((Phases)nextPhaseIndex);
    }

    private void SelectQuadrant() {
        _view.SetCurrentPhaseText("Selecting quadrant");
    }

    public IEnumerator PrepareMatch() {
        ThrowPriorityDice();
        yield return 3f;
        SelectQuadrant();
        yield return 3f;
        GameManager.Instance.PrepareForMatch();
        _view.SetCurrentPhaseText("shuffling decks");
    }

    public IEnumerator ExecutePhases() {
        yield return new WaitForSeconds(3);
        switch (GameManager.Instance.currentPhase) {
            case Phases.Draw:
                DrawPhase();
                break;
            case Phases.PriorityChange:
                SetNextPriority();
                break;
            case Phases.Recharge:
                RechargePhase();
                break;
            case Phases.Principal:
                PrincipalPhase();
                break;
            case Phases.Movement:
                MovementPhase();
                break;
            case Phases.Battle:
                BattlePhase();
                break;
            case Phases.Final:
                FinalPhase();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        yield return new WaitForSeconds(3);
        ChangePhase();
    }

    private void DrawPhase() {
        GameManager.Instance.playerList.ForEach(player => player.PlayerController.DrawCards(5));
        _view.SetCurrentPhaseText("drawing cards");
    }

    private void SetNextPriority() {
        GameManager.Instance.currentPriority =
            (GameManager.Instance.currentPriority + 1) % GameManager.Instance.playerList.Count;
    }

    private void RechargePhase() {
        _view.SetCurrentPhaseText("recharge phase");
    }

    private void PrincipalPhase() {
        _view.SetCurrentPhaseText("principal phase");
    }

    private void MovementPhase() {
        _view.SetCurrentPhaseText("movement phase");
    }

    private void BattlePhase() {
        _view.SetCurrentPhaseText("battle phase");
    }

    private void FinalPhase() {
        _view.SetCurrentPhaseText("final phase");
    }
}
