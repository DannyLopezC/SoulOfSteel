using System.Collections.Generic;
using UnityEngine;

public interface IArmCardView : IEquipmentCardView {
}

public class ArmCardView : EquipmentCardView, IArmCardView {
    private IArmCardController _armCardController;

    public IArmCardController ArmCardController {
        get { return _armCardController ??= new ArmCardController(this); }
    }

    // protected override void Start() {
    //     CardInfoSerialized.CardInfoStruct cardInfoStruct =
    //         GameManager.Instance.cardDataBase.cardDataBase.Sheet1.Find(c => c.TypeEnum == CardType.Legs);
    //
    //     InitCard(cardInfoStruct.Id, cardInfoStruct.CardName, cardInfoStruct.Description,
    //         cardInfoStruct.Cost, cardInfoStruct.Recovery, cardInfoStruct.SerializedMovements,
    //         cardInfoStruct.ImageSource, cardInfoStruct.TypeEnum);
    //
    //     GameManager.Instance.LocalPlayerInstance.PlayerController.SetLegsCard(this);
    // }
    //
    // public void InitCard(int id, string cardName, string cardDescription, int scrapCost, int scrapRecovery,
    //     List<Movement> movements, Sprite imageSource, CardType type) {
    //     ArmCardController.InitCard(id, cardName, cardDescription, scrapCost,
    //         scrapRecovery, movements, imageSource, type);
    // }
}