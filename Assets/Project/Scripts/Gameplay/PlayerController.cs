using System.Collections.Generic;
using UnityEngine;

public interface IPlayerController {
    void DrawCards(int amount);
    void EquipCard(int indexHandList);
    void DismissCard(int indexHandList);
    void ShuffleDeck();
    void SelectAttack();
    void SelectMovement();
    void SelectDefense();
    void DoDamage(int damage);
}

public class PlayerController : IPlayerController {
    private readonly IPlayerView _view;
    
    public int playerId;
    private int _health;
    private int _scrapPoints;
    private List<CardView> _deck;
    private List<CardView> _hand;
    private List<CardView> _scrapPile;
    private List<CardView> _factory;
    private PilotCardView _pilot;
    // private EquipmentCardView legs;
    // private EquipmentCardView leftArm;
    // private EquipmentCardView rightArm;
    // private EquipmentCardView bodyArmor;

    public PlayerController(IPlayerView view) {
        _view = view;
    }

    public void DrawCards(int amount) {
        
    }

    public void EquipCard(int indexHandList) {
        
    }

    public void DismissCard(int indexHandList) {
        
    }

    public void ShuffleDeck() {
        
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