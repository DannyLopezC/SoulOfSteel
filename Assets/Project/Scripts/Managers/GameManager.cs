using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public interface IGameManager {
    void ApplyDamage(int playerId);
    void ValidateHealthStatus();
    void ExecuteEffect();
} 

public class GameManager : MonoBehaviourSingleton<GameManager>, IGameManager {
    public int currentPhase;
    public int currentPriority; // player Id
    [FormerlySerializedAs("board")] public BoardView boardView;
    public List<PlayerView> playerList;

    public Action OnMasterServerConnected;

    public void ApplyDamage(int playerId) {
        
    }

    public void ValidateHealthStatus() {
        
    }

    public void ExecuteEffect() {
        
    }
}