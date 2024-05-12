using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    public CardsDataBase cardDataBase;

    public bool testing;
    public bool isFirstRound;

    public int movementTurn;
    public int attackTurn;

    public HandPanel handPanel;
    public HandPanel middlePanel;
    public ScrapPanel scrapPanel;

    public EquipmentPanel myEquipmentPanel;
    public EquipmentPanel enemyEquipmentPanel;

    public Phase CurrentPhase { get; private set; }
    public PlayerView LocalPlayerInstance { get; set; }
    public PhotonGame PhotonGame { get; set; }

    public PilotCardView LocalPilotCardView;
    public string LocalPlayerName;

    public int currentPriority; // player Id
    public BoardView boardView;
    public List<PlayerView> playerList;

    public Phase CurrenPhase;
    public bool gameOver;

    #region Events

    public event Action OnMasterServerConnected;
    public event Action<Phase> ExecutePhases;
    public event Action OnDrawFinishedEvent;
    public event Action<Vector2> OnCellClickedEvent;
    public event Action OnGameStartedEvent;
    public event Action<PlayerView, CardView, bool> OnCardSelectedEvent; // card has been selected or deselected
    public event Action OnCardSelectingFinishedEvent; // all cards has been selected
    public event Action<int> OnPrioritySetEvent;

    //movement events
    public event Action<Movement, PlayerView, int> OnMovementSelectedEvent;
    public event Action OnMovementFinishedEvent;
    public event Action<int> OnSelectionConfirmedEvent;
    public event Action OnAllMovementSelectedEvent;
    public event Action OnMovementTurnDoneEvent;

    // battle events
    public event Action OnAllAttacksSelectedEvent;
    public event Action OnLocalAttackDoneEvent;

    #endregion

    #region EventsInvokes

    public void OnLocalAttackDone()
    {
        OnLocalAttackDoneEvent?.Invoke();
    }

    public void OnAllAttackSelected()
    {
        OnAllAttacksSelectedEvent?.Invoke();
    }

    public void OnMovementTurnDone()
    {
        OnMovementTurnDoneEvent?.Invoke();
    }

    public void OnAllMovementSelected()
    {
        OnAllMovementSelectedEvent?.Invoke();
    }

    public void OnSelectionConfirmed(int id)
    {
        OnSelectionConfirmedEvent?.Invoke(id);
    }

    public void OnMovementFinished()
    {
        OnMovementFinishedEvent?.Invoke();
    }

    public void OnMovementSelected(Movement movement, PlayerView player, int iterations)
    {
        OnMovementSelectedEvent?.Invoke(movement, player, iterations);
    }

    public void OnPrioritySet(int priority)
    {
        OnPrioritySetEvent?.Invoke(priority);
    }


    public void OnCardSelected(PlayerView playerView, CardView card, bool selected)
    {
        OnCardSelectedEvent?.Invoke(playerView, card, selected);
    }

    public void OnSelectingFinished()
    {
        OnCardSelectingFinishedEvent?.Invoke();
    }

    public void OnCellClicked(Vector2 index)
    {
        OnCellClickedEvent?.Invoke(index);
    }

    public void OnGameStarted()
    {
        StartCoroutine(OnGameStartedCoroutine());
    }

    public IEnumerator OnGameStartedCoroutine()
    {
        while (playerList.Count < 2 && !testing) {
            yield return null;
        }

        OnGameStartedEvent?.Invoke();
    }

    public void OnDrawFinished()
    {
        OnDrawFinishedEvent?.Invoke();
    }

    public void OnConnectedToServer()
    {
        OnMasterServerConnected?.Invoke();
    }

    public void ChangePhase(Phase phase)
    {
        if (gameOver) return;
        CurrentPhase = phase;
        ExecutePhases?.Invoke(CurrentPhase);
    }

    #endregion

    public bool ValidateHealthStatus()
    {
        foreach (PlayerView playerView in playerList) {
            if (playerView.PlayerController.GetCurrenHealth() <= 0) {
                Debug.Log($"current phase {CurrenPhase}");
                Debug.Log($"player view id {playerView.PlayerController.GetPlayerId()}");
                Debug.Log($"player health {playerView.PlayerController.GetCurrenHealth()}");
                gameOver = true;
                UIManager.Instance.SetText(
                    $"JUEGO TERMINADO, jugador {playerView.PlayerController.GetPlayerId()} perdio");
                CurrenPhase.End();
                StartCoroutine(BackToMenuFinishingGame());
                return false;
            }
        }

        return true;
    }

    IEnumerator BackToMenuFinishingGame()
    {
        yield return new WaitForSeconds(3);

        PhotonGame.DisconnectPlayer();
    }

    public void PrepareForMatch(IMatchView matchView)
    {
        playerList.ForEach(player => player.PlayerController.ShuffleDeck(true, false));
        ChangePhase(new DrawPhase(matchView));
    }

    protected override void OnDestroy()
    {
        Instance = null;
    }
}