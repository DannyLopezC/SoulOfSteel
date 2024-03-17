using System;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public interface IPilotCardView : ICardView{
    void SetCardUI(string cardName, string cardDescription, int scrapCost, Sprite imageSource, int health);
}

[Serializable]
public class PilotCardView: CardView, IPilotCardView {
    [Header("Pilot Card UI Components")]
    [SerializeField] private TMP_Text healthTMP;

    private IPilotCardController _pilotCardController;

    public IPilotCardController PilotCardController {
        get { return _pilotCardController ??= new PilotCardController(this); }
    }
    
    private void Start() {
        PilotCardController.InitializePilotCard("Charizard",
            "esta carta está rotisima no hay nada que hacer contra ella",
            6,
            9,
            false,
            null,
            100,
            null);
    }
    
    public void SetCardUI(string cardName, string cardDescription, int scrapCost, Sprite imageSource, int _health) {
        base.SetCardUI(cardName, cardDescription, scrapCost, imageSource);

        if(healthTMP != null) healthTMP.text = $"Vida: {_health}";
    }
}
