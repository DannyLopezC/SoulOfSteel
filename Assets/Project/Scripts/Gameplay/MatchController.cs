using System.Collections.Generic;
using UnityEngine;

public interface IMatchController {
    int ThrowPriorityDice();
    void SetNextPriority();
    void ChangePhase();
    void SelectQuadrant();
    void PrepareMatch();
    void RechargePhase();
    void PrincipalPhase();
    void MovementPhase();
    void BattlePhase();
    void FinalPhase();
}

public class MatchController : IMatchController {
    private int _matchId;
    private List<string> _matchLog;
    
    private readonly IMatchView _view;
    
    public MatchController(IMatchView view) {
        _view = view;
    }

    public int ThrowPriorityDice() {
        return Random.Range(1, 6);
    }

    public void SetNextPriority() {
    }

    public void ChangePhase() {
    }

    public void SelectQuadrant() {
    }

    public void PrepareMatch() {
    }

    public void RechargePhase() {
    }

    public void PrincipalPhase() {
    }

    public void MovementPhase() {
    }

    public void BattlePhase() {
    }

    public void FinalPhase() {
    }
}
