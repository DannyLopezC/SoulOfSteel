using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviourSingleton<GameManager> {
    public CardsDataBase cardDataBase;

    public bool testing;

    public HandPanel handPanel;
    public HandPanel middlePanel;

    public Phase CurrentPhase { get; private set; }
    public GameObject LocalPlayerInstance { get; set; }
    public string LocalPlayerName;

    public int currentPriority; // player Id
    public BoardView boardView;
    public List<PlayerView> playerList;

    public event Action OnMasterServerConnected;
    public event Action<Phase> ExecutePhases;
    public event Action OnDrawFinishedEvent;
    public event Action<string> OnDataDownloadedEvent;
    public event Action<Vector2> OnCellClickedEvent;
    public event Action OnGameStartedEvent;
    public event Action OnSelectingFinishedEvent;
    public event Action<CardView, bool> OnCardSelectedEvent;

    public void OnCardSelected(CardView card, bool selected) {
        OnCardSelectedEvent?.Invoke(card, selected);
    }

    public void OnSelectingFinished() {
        OnSelectingFinishedEvent?.Invoke();
    }

    public void OnCellClicked(Vector2 index) {
        OnCellClickedEvent?.Invoke(index);
    }

    public void OnGameStarted() {
        StartCoroutine(OnGameStartedCoroutine());
    }

    public IEnumerator OnGameStartedCoroutine() {
        while (playerList.Count < 2 && !testing) {
            yield return null;
        }

        OnGameStartedEvent?.Invoke();
    }

    public void OnDataDownloaded(string data) {
        OnDataDownloadedEvent?.Invoke(data);
    }

    public void OnDrawFinished() {
        OnDrawFinishedEvent?.Invoke();
    }

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