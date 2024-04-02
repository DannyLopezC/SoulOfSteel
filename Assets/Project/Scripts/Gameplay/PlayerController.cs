using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public interface IPlayerController {
    void DrawCards(int amount, bool fullDraw);
    void EquipCard(int indexHandList);
    void DismissCard(int indexHandList);
    void ShuffleDeck();
    void SelectAttack();
    void SelectMovement();
    void SelectDefense();
    void DoDamage(int damage);
    IEnumerator AddCards(int amount);
}

public class PlayerController : IPlayerController {
    private readonly IPlayerView _view;

    public int PlayerId;
    private int _health;
    private int _scrapPoints;
    private readonly List<CardView> _deck;
    private List<CardView> _shuffledDeck;
    private List<CardView> _hand;
    private List<CardView> _scrapPile;
    private List<CardView> _factory;
    private PilotCardView _pilot;
    private EquipmentCardView _legs;
    private EquipmentCardView _leftArm;
    private EquipmentCardView _rightArm;
    private EquipmentCardView _bodyArmor;

    public PlayerController(IPlayerView view, List<CardView> deck) {
        _view = view;
        _deck = new List<CardView>();
        foreach (CardView cv in deck) {
            _deck.Add(cv);
        }

        _hand = new List<CardView>();
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
            int random = Random.Range(0, _shuffledDeck.Count - 1);
            CardView cardToAdd = _shuffledDeck[random];
            _hand.Add(cardToAdd);
            _view.AddCardToPanel(cardToAdd);
            _shuffledDeck.RemoveAt(random);
            if (_shuffledDeck.Count == 0) ShuffleDeck();
            amount--;
        }

        GameManager.Instance.OnDrawFinished();
    }

    public void EquipCard(int indexHandList) {
        
    }

    public void DismissCard(int indexHandList) {
        
    }

    public void ShuffleDeck() {
        List<CardView> temporalDeck = _deck.ToList();

        int n = temporalDeck.Count;
        while (n > 1) {
            n--;
            int k = Random.Range(0, n + 1);
            (temporalDeck[k], temporalDeck[n]) = (temporalDeck[n], temporalDeck[k]);
        }

        _shuffledDeck = temporalDeck;
    }

    public void SelectAttack() {
        
    }

    public void SelectMovement() {
        
    }

    public void SelectDefense() {
        
    }

    public void DoDamage(int damage) {
        
    }
}