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

    public Action OnMasterServerConnected;
    public Action ExecutePhases;

    private void Start() {
        playerList = new List<PlayerView> {
            Instantiate(Resources.Load("Player").GetComponent<PlayerView>()),
            Instantiate(Resources.Load("Player").GetComponent<PlayerView>())
        };
    }

    public void ApplyDamage(int playerId) {
        
    }

    public void ValidateHealthStatus() {
        
    }

    public void ExecuteEffect() {
        
    }
}