using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public enum Phases {
    Draw = 0,
    PriorityChange = 1,
    Recharge = 2,
    Principal = 3,
    Movement = 4,
    Battle = 5,
    Final = 6
}

public interface IGameManager {
    void ApplyDamage(int playerId);
    void ValidateHealthStatus();
    void ExecuteEffect();
} 

public class GameManager : MonoBehaviourSingleton<GameManager>, IGameManager {
    public Phases currentPhase;
    public int currentPriority; // player Id
    public BoardView boardView;
    public List<PlayerView> playerList;

    public event Action OnMasterServerConnected;
    public event Action ExecutePhases;

    public void OnConnectedToServer() {
        OnMasterServerConnected?.Invoke();
    }

    public void ApplyDamage(int playerId) {
        
    }

    public void ValidateHealthStatus() {
        
    }

    public void ExecuteEffect() {
        
    }

    public void PrepareForMatch() {
        playerList.ForEach(player => player.PlayerController.ShuffleDeck());
        ExecutePhases?.Invoke();
    }

    public void ChangePhase(Phases phase) {
        currentPhase = phase;
        ExecutePhases?.Invoke();
    }

    protected override void OnDestroy() {
        Instance = null;
    }
}