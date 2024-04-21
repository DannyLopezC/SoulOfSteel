using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using WebSocketSharp;

[Serializable]
public class CardInfoSerialized {
    public List<CardInfoStruct> Sheet1;
    private List<CardInfoStruct> Sheet2;

    [Serializable]
    public class CardInfoStruct {
        Dictionary<string, CardType> typeMapping = new() {
            { "Campo", CardType.CampEffect },
            { "hackeo", CardType.Hacking },
            { "Generador", CardType.Generator },
            { "Arma", CardType.Weapon },
            { "Brazo", CardType.Arm },
            { "Pecho", CardType.Chest },
            { "Piernas", CardType.Legs },
        };

        [FoldoutGroup("Card")] public int Id;
        [FoldoutGroup("Card")] public string CardName;

        [OnValueChanged("SetType"), HideInInspector]
        public string Type;

        [FoldoutGroup("Card")] public CardType TypeEnum;

        [TextArea(1, 10), FoldoutGroup("Card/Description")] [FoldoutGroup("Card")]
        public string Description;

        [FoldoutGroup("Card")] public int Cost;
        [FoldoutGroup("Card")] public int Energy;
        [FoldoutGroup("Card")] public int Damage;
        [FoldoutGroup("Card")] public int Shield;
        [FoldoutGroup("Card")] public int Recovery;
        [FoldoutGroup("Card")] public bool IsCampEffect;
        [FoldoutGroup("Card")] public Sprite ImageSource;
        [FoldoutGroup("Card")] public int Health;
        [FoldoutGroup("Card")] public List<Movement> SerializedMovements;

        [OnValueChanged("SetType"), HideInInspector]
        public string Movements;

        public void SetType() {
            Type = Type.Replace(" ", "");
            if (Type == "Brazo/Arma") Type = "Arma";
            // Debug.Log($"{Type}");
            if (typeMapping.TryGetValue(Type, out CardType enumValue)) {
                TypeEnum = enumValue;
            }
            else {
                throw new ArgumentException($"Invalid CardType Type: {Type}");
            }
        }

        public void SetMovements() {
            if (Movements.IsNullOrEmpty() || Movements == "0") return;
            SerializedMovements = Movement.FromString(Movements);
        }
    }
}

[Serializable, CreateAssetMenu(fileName = "Card_Data", menuName = "Card_Data")]
public class CardsDataBase : ScriptableObject {
    public CardInfoSerialized cardDataBase;

    [Button]
    public void DownloadData() {
        Downloader.Instance.LoadInfo(this);
    }
}