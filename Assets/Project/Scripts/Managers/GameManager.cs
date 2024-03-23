using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public interface IGameManager {
    void ApplyDamage(int playerId);
    void ValidateHealthStatus();
    void ExecuteEffect();
} 

public class GameManager : MonoBehaviourSingleton<GameManager>, IGameManager {
    public Phase CurrentPhase { get; private set; }
    public int currentPriority; // player Id
    public BoardView boardView;
    public List<PlayerView> playerList;

    public event Action OnMasterServerConnected;
    public event Action<Phase> ExecutePhases;

    public void OnConnectedToServer() {
        OnMasterServerConnected?.Invoke();
    }

    public void ApplyDamage(int playerId) {
        
    }

    public void ValidateHealthStatus() {
        
    }

    public void ExecuteEffect() {
        
    }

    public void PrepareForMatch(IMatchView matchView) {
        playerList.ForEach(player => player.PlayerController.ShuffleDeck());
        ChangePhase(new DrawPhase(matchView));
    }

    public void ChangePhase(Phase phase) {
        CurrentPhase = phase;
        ExecutePhases?.Invoke(CurrentPhase);
    }

    protected override void OnDestroy() {
        Instance = null;
    }
}