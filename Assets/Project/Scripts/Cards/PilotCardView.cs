﻿using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public interface IPilotCardView : ICardView {
    void SetCardUI(string cardName, string cardDescription, int scrapCost, Sprite imageSource, int health);
}

[Serializable]
public class PilotCardView : CardView, IPilotCardView {
    [Header("Pilot Card UI Components")] [SerializeField]
    private TMP_Text healthTMP;

    private IPilotCardController _pilotCardController;

    public IPilotCardController PilotCardController {
        get { return _pilotCardController ??= new PilotCardController(this); }
    }

    private void Start() {
        // PilotCardController.InitializePilotCard("Charizard",
        //     "esta carta está rotisima no hay nada que hacer contra ella",
        //     Random.Range(3, 7),
        //     Random.Range(3, 7),
        //     false,
        //     null,
        //     Random.Range(1, 100),
        //     null,
        //     CardType.Pilot);
    }

    public override bool GetSelected() {
        return PilotCardController.GetSelected();
    }

    public override void Select(bool deselect = false) {
        PilotCardController.Select(deselect);
    }

    public override void DoEffect(int originId) {
        PilotCardController.DoEffect(originId);
    }

    public override void InitCard(int id, string cardName, string cardDescription, int scrapCost, int scrapRecovery,
        bool isCampEffect, Sprite imageSource, int health, List<Movement> defaultMovement, CardType type) {
        PilotCardController.InitializePilotCard(id, cardName, cardDescription, scrapCost, scrapRecovery,
            isCampEffect, imageSource, health, defaultMovement, type);
    }

    public void SetCardUI(string cardName, string cardDescription, int scrapCost, Sprite imageSource, int _health) {
        base.SetCardUI(cardName, cardDescription, scrapCost, imageSource);

        if (healthTMP != null) healthTMP.text = $"Vida: {_health}";
    }

    public override void ManageLeftClick() {
        PilotCardController.Select(false);
    }

    public override void ManageRightClick() {
        PilotCardController.ManageRightClick();
    }

    public override void SetIsSelecting(bool isSelecting) {
        PilotCardController.IsSelecting(isSelecting);
    }

    public override CardType GetCardType() {
        return PilotCardController.GetCardType();
    }
}