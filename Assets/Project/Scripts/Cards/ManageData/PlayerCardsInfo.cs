using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;


[Serializable, CreateAssetMenu(fileName = "Card_", menuName = "Card")]
public class PlayerCardsInfo : ScriptableObject {
    public int playerId;
    public List<CardInfoSerialized.CardInfoStruct> playerCards;
}