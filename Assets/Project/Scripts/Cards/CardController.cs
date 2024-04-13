using System;
using UnityEngine;

[Flags]
public enum CardType {
    Pilot,
    Weapon,
    Armor,
    CampEffect,
    Hacking,
    Generator,
    Arm,
    Legs,
    Chest
}

public interface ICardController {
    CardType GetCardType();
    public void ManageRightClick();
    void PrintInfo();
    void Select(bool deselect = false);
    void IsSelecting(bool isSelecting);
    bool GetSelected();
}

public abstract class CardController : ICardController {
    private readonly ICardView _view;

    private bool _isSelecting;
    private bool _selected;
    private int _scrapRecovery;
    private bool _isCampEffect;

    protected CardType Type;
    protected string CardName { get; private set; }
    protected string CardDescription { get; private set; }
    protected int ScrapCost { get; private set; }
    protected Sprite ImageSource { get; private set; }

    protected CardController(ICardView view) {
        _view = view;
    }

    protected void InitCard(string cardName, string cardDescription, int scrapCost, int scrapRecovery,
        bool isCampEffect, Sprite imageSource, CardType type) {
        CardName = cardName;
        CardDescription = cardDescription;
        ScrapCost = scrapCost;
        _scrapRecovery = scrapRecovery;
        _isCampEffect = isCampEffect;
        ImageSource = imageSource;
        Type = type;

        SetCardUI();
    }

    protected virtual void SetCardUI() {
        _view.SetCardUI(CardName, CardDescription, ScrapCost, ImageSource);
    }

    protected virtual void ShowCard() {
        UIManager.Instance.ShowCardPanel(CardName, CardDescription, ScrapCost, ImageSource);
    }

    public virtual void ManageRightClick() {
        ShowCard();
    }

    public void PrintInfo() {
        string s = CardName + "\n";
        s += CardDescription + "\n";
        s += $"{ScrapCost}\n";
        Debug.Log(s);
    }

    public abstract CardType GetCardType();

    public void Select(bool deselect = false) {
        if (!_isSelecting) return;

        _selected = !deselect && !_selected;
        _view.GetGameObject().transform.localScale = _selected ? Vector3.one : new Vector3(0.7f, 0.7f, 0.7f);
    }

    public void IsSelecting(bool isSelecting) {
        _isSelecting = isSelecting;
    }

    public bool GetSelected() {
        return _selected;
    }
}