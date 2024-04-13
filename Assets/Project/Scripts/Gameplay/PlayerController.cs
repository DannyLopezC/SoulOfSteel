using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public interface IPlayerController {
    void SetPlayerId(int id);
    void DrawCards(int amount, bool fullDraw);
    void EquipCard(int indexHandList);
    void DismissCard(int indexHandList);
    void ShuffleDeck();
    void SelectAttack();
    void SelectMovement();
    void SelectDefense();
    IEnumerator SelectCards(CardType type, int amount);
    void DoDamage(int damage);
    IEnumerator AddCards(int amount);
    int GetPlayerId();
}

public class PlayerController : IPlayerController {
    private readonly IPlayerView _view;

    private int _playerId;
    [ShowInInspector] private int _health;
    private int _scrapPoints;
    private readonly PlayerCardsInfo _deckInfo;
    private PlayerCardsInfo _shuffledDeck;
    private List<CardView> _hand;
    private List<CardView> _scrapPile;
    private List<CardView> _factory;
    private PilotCardView _pilot;
    private EquipmentCardView _legs;
    private EquipmentCardView _leftArm;
    private EquipmentCardView _rightArm;
    private EquipmentCardView _bodyArmor;

    public PlayerController(IPlayerView view, PlayerCardsInfo deck) {
        _view = view;
        _deckInfo = deck;

        _hand = new List<CardView>();
        _shuffledDeck = ScriptableObject.CreateInstance<PlayerCardsInfo>();
    }

    public void DrawCards(int amount, bool fullDraw) {
        if (fullDraw) {
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
            CardView card = _view.AddCardToPanel(cardInfoStruct.TypeEnum);
            card.InitCard(cardInfoStruct.CardName, cardInfoStruct.Description, cardInfoStruct.Cost,
                cardInfoStruct.Recovery, cardInfoStruct.IsCampEffect, cardInfoStruct.ImageSource,
                cardInfoStruct.Health, cardInfoStruct.DefaultMovement, cardInfoStruct.TypeEnum);
            _hand.Add(card);
            _shuffledDeck.playerCards.RemoveAt(random);
            if (_shuffledDeck.playerCards.Count == 0) ShuffleDeck();
            amount--;
        }

        GameManager.Instance.OnDrawFinished();
    }

    public int GetPlayerId() {
        return _playerId;
    }

    public void EquipCard(int indexHandList) {
    }

    public void DismissCard(int indexHandList) {
    }

    public void ShuffleDeck() {
        List<CardInfoSerialized.CardInfoStruct> temporalDeck = _deckInfo.playerCards.ToList();

        int n = temporalDeck.Count;
        while (n > 1) {
            n--;
            int k = Random.Range(0, n + 1);
            (temporalDeck[k], temporalDeck[n]) = (temporalDeck[n], temporalDeck[k]);
        }

        _shuffledDeck.playerCards = temporalDeck;
    }

    public void SelectAttack() {
    }

    public void SelectMovement() {
    }

    public void SelectDefense() {
    }

    public void DoDamage(int damage) {
    }

    public void SetPlayerId(int id) {
        _playerId = id;
    }

    public IEnumerator SelectCards(CardType type, int amount) {
        List<CardView> selectedCards = new List<CardView>();
        int currentAmount = amount;

        if (_hand.Find(card => card.GetCardType() == CardType.CampEffect) == null) {
            GameManager.Instance.OnSelectingFinished(selectedCards);
            yield break;
        }

        foreach (CardView card in _hand) {
            if (card.GetCardType() == type) {
                card.SetIsSelecting(true);
            }
        }

        while (currentAmount > 0) {
            foreach (CardView card in _hand) {
                if (card.GetSelected()) {
                    if (!selectedCards.Contains(card)) {
                        selectedCards.Add(card);
                    }
                }
                else {
                    selectedCards.Remove(card);
                }
            }

            currentAmount = amount - selectedCards.Count;
            if (currentAmount <= 0) {
                break; // Exit the loop if the required amount is reached
            }

            yield return null;
        }

        yield return new WaitForSeconds(2);
        GameManager.Instance.OnSelectingFinished(selectedCards);
    }
}