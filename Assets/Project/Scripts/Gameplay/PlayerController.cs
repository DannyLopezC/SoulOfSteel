using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon.StructWrapping;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public interface IPlayerController {
    void SetPlayerId(int id);
    void DrawCards(int amount, bool fullDraw);
    void EquipCard(int indexHandList);
    void ShuffleDeck(bool firstTime);
    void SelectAttack();
    void SelectMovement();
    void SelectDefense();
    void SelectCards(CardType type, int amount, bool setIsSelecting = true);
    void GetDamage(int damage);
    IEnumerator AddCards(int amount);
    int GetPlayerId();
    bool GetCardsSelected();
    void SetCardsSelected(bool cardsSelected);
    IEnumerator SelectCells(int amount);
    bool GetMoving();
    void SetMoving(bool moving);
    bool GetDoingEffect();
    void SetDoingEffect(bool doingEffect);
    bool GetAllEffectsDone();
    void SetAllEffectsDone(bool allEffectsDone);
    void SetCurrentCell(Vector2 currentCell);
    Vector2 GetCurrentCell();
    void SetCurrentDegrees(int currentDegrees);
    int GetCurrentDegrees();
    void SetLegsCard(LegsCardView legsCardView);
    bool GetMovementSelected();
    void SetMovementSelected(bool movementSelected);
    bool GetMovementDone();
    void SetMovementDone(bool movementDone);
    void DoMovement();
}

public class PlayerController : IPlayerController {
    private readonly IPlayerView _view;

    private int _playerId;
    [ShowInInspector] private int _health;
    private int _scrapPoints;
    private PlayerCardsInfo _shuffledDeck;
    private List<CardView> _hand;
    private List<CardView> _scrapPile;
    private List<CardView> _factory;
    private PilotCardView _pilot;
    private LegsCardView _legs;
    private EquipmentCardView _leftArm;
    private EquipmentCardView _rightArm;
    private EquipmentCardView _bodyArmor;

    private bool _movementSelected;
    private bool _movementDone;

    private bool _cardsSelected;
    private bool _selectingCells;
    private bool _moving;
    private bool _doingEffect;
    private bool _allEffectsDone;

    private int _currentMovementId;

    private List<Vector2> cellsSelected;

    private Vector2 _currentCell;
    private int _currentDegrees;

    public PlayerController(IPlayerView view) {
        _view = view;

        _hand = new List<CardView>();
        _shuffledDeck = ScriptableObject.CreateInstance<PlayerCardsInfo>();
        _moving = false;
        _currentDegrees = 270;
        _currentCell = new Vector2(5, 5);
    }

    public void DrawCards(int amount, bool fullDraw) {
        if (fullDraw) {
            // not destroying the select animatino reference
            GameManager.Instance.handPanel.animationReference.SetParent(
                GameManager.Instance.handPanel.transform.parent);
            _view.CleanHandsPanel();
            _hand.Clear();
        }

        _view.InitAddCards(amount);
    }

    public IEnumerator AddCards(int amount) {
        while (amount > 0) {
            yield return new WaitForSeconds(0.5f);
            UIManager.Instance.SetText($"adding cards {amount}");
            int random = Random.Range(0, _shuffledDeck.playerCards.Count - 1);
            CardInfoSerialized.CardInfoStruct cardInfoStruct = _shuffledDeck.playerCards[random];

            CardView card = null;

            if (cardInfoStruct.TypeEnum != CardType.Pilot && cardInfoStruct.TypeEnum != CardType.Legs) {
                card = _view.AddCardToPanel(cardInfoStruct.TypeEnum);
            }

            switch (cardInfoStruct.TypeEnum) {
                case CardType.Pilot:
                    Debug.Log($"Cannot have pilot card here");
                    continue;
                case CardType.CampEffect:
                case CardType.Hacking:
                case CardType.Generator:
                    ((EffectCardView)card).InitCard(cardInfoStruct.Id, cardInfoStruct.CardName,
                        cardInfoStruct.Description, cardInfoStruct.Cost, cardInfoStruct.Recovery,
                        cardInfoStruct.IsCampEffect, cardInfoStruct.ImageSource, cardInfoStruct.TypeEnum);
                    break;
                case CardType.Weapon:
                case CardType.Armor:
                case CardType.Arm:
                case CardType.Legs:
                    // ((LegsCardView)card).InitCard(cardInfoStruct.Id, cardInfoStruct.CardName,
                    //     cardInfoStruct.Description, cardInfoStruct.Cost, cardInfoStruct.Recovery,
                    //     cardInfoStruct.SerializedMovements, cardInfoStruct.ImageSource, cardInfoStruct.TypeEnum);
                    continue;
                case CardType.Chest:
                    ((EquipmentCardView)card).InitCard(cardInfoStruct.Id, cardInfoStruct.CardName,
                        cardInfoStruct.Description, cardInfoStruct.Cost, cardInfoStruct.Recovery,
                        cardInfoStruct.ImageSource, cardInfoStruct.TypeEnum);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            _hand.Add(card);
            _shuffledDeck.playerCards.RemoveAt(random);
            if (_shuffledDeck.playerCards.Count == 0) ShuffleDeck(false);
            amount--;
        }

        GameManager.Instance.OnDrawFinished();
    }

    public int GetPlayerId() {
        return _playerId;
    }

    public bool GetCardsSelected() {
        return _cardsSelected;
    }

    public void SetCardsSelected(bool cardsSelected) {
        _cardsSelected = cardsSelected;
    }

    public void EquipCard(int indexHandList) {
    }

    public void ShuffleDeck(bool firstTime) {
        List<CardInfoSerialized.CardInfoStruct> temporalDeck = _view.GetDeckInfo().playerCards.ToList();

        int n = temporalDeck.Count;
        while (n > 1) {
            n--;
            int k = Random.Range(0, n + 1);
            (temporalDeck[k], temporalDeck[n]) = (temporalDeck[n], temporalDeck[k]);
        }

        _shuffledDeck.playerCards = temporalDeck;

        // foreach (CardInfoSerialized.CardInfoStruct shuffledDeckPlayerCard in _deckInfo.playerCards) {
        //     Debug.Log($"{shuffledDeckPlayerCard.CardName} \n");
        // }

        if (firstTime && _view.GetPv().IsMine) {
            SetPilotCard();
            SetLegsCard();
        }

        _shuffledDeck.playerCards.Remove(_shuffledDeck.playerCards.Find(p => p.TypeEnum == CardType.Pilot));
    }

    public void SelectAttack() {
    }

    public void SelectMovement() {
        if (!_view.GetPv().IsMine) return;

        if (_legs == null && _pilot != null) {
            Movement movement = _pilot.PilotCardController.GetDefaultMovement();
            if (movement != null) GameManager.Instance.OnMovementSelected(movement, (PlayerView)_view);
        }
        else {
            GameManager.Instance.OnSelectionConfirmedEvent += OnMovementSelected;
            _legs.SelectMovement();
        }
    }

    public void OnMovementSelected(int movementId) {
        SetMovementSelected(true);
        _currentMovementId = movementId;
        GameManager.Instance.OnAllMovementSelectedEvent += OnAllMovementSelected;
    }

    public void OnAllMovementSelected() {
        GameManager.Instance.OnSelectionConfirmedEvent -= OnMovementSelected;
        GameManager.Instance.OnAllMovementSelectedEvent -= OnAllMovementSelected;
    }

    public void DoMovement() {
        Debug.Log($"player id {GetPlayerId()} turn {GameManager.Instance.movementTurn}");
        GameManager.Instance.OnMovementSelected(_legs.LegsCardController.GetMovements()[_currentMovementId],
            (PlayerView)_view);
    }

    public void SelectDefense() {
    }

    public void GetDamage(int damage) {
        _health -= damage;
    }

    public void SetPlayerId(int id) {
        _playerId = id;
    }

    public void SelectCards(CardType type, int amount, bool setSelecting = true) {
        foreach (CardView card in _hand) {
            if (card.GetCardType() == type) {
                card.SetIsSelecting(setSelecting);
            }
        }
    }

    public IEnumerator SelectCells(int amount) {
        int currentAmount = amount;
        cellsSelected = new List<Vector2>();
        cellsSelected.Clear();

        _selectingCells = true;
        EffectManager.Instance.OnSelectedCellEvent += CellSelected;

        while (currentAmount > 0) {
            yield return null;
            currentAmount = amount - cellsSelected.Count;
        }

        _selectingCells = false;
        EffectManager.Instance.OnSelectedCellEvent -= CellSelected;

        EffectManager.Instance.CellsSelected(cellsSelected);
    }

    public bool GetMoving() {
        return _moving;
    }

    public void SetMoving(bool moving) {
        _moving = moving;
    }

    public bool GetDoingEffect() {
        return _doingEffect;
    }

    public void SetDoingEffect(bool doingEffect) {
        _doingEffect = doingEffect;
    }

    public bool GetAllEffectsDone() {
        return _allEffectsDone;
    }

    public void SetAllEffectsDone(bool allEffectsDone) {
        _allEffectsDone = allEffectsDone;
    }

    public void SetCurrentCell(Vector2 currentCell) {
        _currentCell = currentCell;
    }

    public Vector2 GetCurrentCell() {
        return _currentCell;
    }

    public void SetCurrentDegrees(int currentDegrees) {
        _currentDegrees = currentDegrees;
    }

    public int GetCurrentDegrees() {
        return _currentDegrees;
    }

    public void SetLegsCard(LegsCardView legsCardView) {
        _legs = legsCardView;
    }

    public bool GetMovementSelected() {
        return _movementSelected;
    }

    public void SetMovementSelected(bool movementSelected) {
        _movementSelected = movementSelected;
    }

    public bool GetMovementDone() {
        return _movementDone;
    }

    public void SetMovementDone(bool movementDone) {
        _movementDone = movementDone;
    }

    private void CellSelected(Vector2 index, bool select) {
        if (select) cellsSelected.Add(index);
        else cellsSelected.Remove(index);
    }

    public void SetLegsCard() {
        // _view.ClearPanel(GameManager.Instance.myEquipmentPanel.transform);
        // _view.ClearPanel(GameManager.Instance.enemyEquipmentPanel.transform);

        CardInfoSerialized.CardInfoStruct cardInfoStruct =
            _shuffledDeck.playerCards.Find(c => c.TypeEnum == CardType.Legs);


        LegsCardView card = (LegsCardView)_view.AddCardToPanel(cardInfoStruct.TypeEnum);

        card.InitCard(cardInfoStruct.Id, cardInfoStruct.CardName,
            cardInfoStruct.Description, cardInfoStruct.Cost, cardInfoStruct.Recovery,
            cardInfoStruct.SerializedMovements, cardInfoStruct.ImageSource, cardInfoStruct.TypeEnum);
        _legs = card;
    }

    private void SetPilotCard() {
        CardInfoSerialized.CardInfoStruct cardInfoStruct =
            _shuffledDeck.playerCards.Find(c => c.TypeEnum == CardType.Pilot);


        PilotCardView card = (PilotCardView)_view.AddCardToPanel(cardInfoStruct.TypeEnum);

        card.InitCard(cardInfoStruct.Id, cardInfoStruct.CardName,
            cardInfoStruct.Description, cardInfoStruct.Cost, cardInfoStruct.Recovery,
            cardInfoStruct.ImageSource, cardInfoStruct.Health, cardInfoStruct.TypeEnum,
            cardInfoStruct.SerializedMovements[0]);
        _pilot = card;
    }
}