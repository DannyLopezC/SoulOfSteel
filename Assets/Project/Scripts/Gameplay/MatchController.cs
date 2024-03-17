using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

public interface IMatchController {
    void ThrowPriorityDice();
    void SetNextPriority();
    void ChangePhase();
    void SelectQuadrant();
    IEnumerator PrepareMatch();
    IEnumerator ExecutePhases();
    void RechargePhase();
    void PrincipalPhase();
    void MovementPhase();
    void BattlePhase();
    void FinalPhase();
    void DrawPhase();
}

public class MatchController : IMatchController {
    private int _matchId;
    private List<string> _matchLog;
    
    private readonly IMatchView _view;
    
    public MatchController(IMatchView view) {
        _view = view;
    }

    public void ThrowPriorityDice() {
        GameManager.Instance.currentPriority = Random.Range(0, 1);
        _view.SetCurrentPhaseText($"Throwing priority dice, result={GameManager.Instance.currentPriority}");
    }

    public void ChangePhase() {
        _view.SetCurrentPhaseText("Changing Phase");

        int currentPhaseIndex = (int)GameManager.Instance.currentPhase;
        int totalPhases = Enum.GetNames(typeof(Phases)).Length;

        int nextPhaseIndex = (currentPhaseIndex + 1) % totalPhases;

        GameManager.Instance.currentPhase = (Phases)nextPhaseIndex;

        GameManager.Instance.ExecutePhases.Invoke();
    }

    public void SelectQuadrant() {
        _view.SetCurrentPhaseText("Selecting quadrant");
    }

    public IEnumerator PrepareMatch() {
        ThrowPriorityDice();
        yield return new WaitForSeconds(3);
        SelectQuadrant();
        yield return new WaitForSeconds(3);
        GameManager.Instance.playerList.ForEach(player => player.PlayerController.ShuffleDeck());
        _view.SetCurrentPhaseText("shuffling decks");
        GameManager.Instance.ExecutePhases.Invoke();
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

    public void DrawPhase() {
        GameManager.Instance.playerList.ForEach(player => player.PlayerController.DrawCards(5));
        _view.SetCurrentPhaseText("drawing cards");
    }

    public void SetNextPriority() {
        GameManager.Instance.currentPriority =
            (GameManager.Instance.currentPriority + 1) % GameManager.Instance.playerList.Count;
    }
    
    public void RechargePhase() {
        _view.SetCurrentPhaseText("recharge phase");
    }

    public void PrincipalPhase() {
        _view.SetCurrentPhaseText("principal phase");
    }

    public void MovementPhase() {
        _view.SetCurrentPhaseText("movement phase");
    }

    public void BattlePhase() {
        _view.SetCurrentPhaseText("battle phase");
    }

    public void FinalPhase() {
        _view.SetCurrentPhaseText("final phase");
    }
}
