using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class CardInfoSerialized {
    public List<CardInfoStruct> Sheet1;
    private List<CardInfoStruct> Sheet2;

    [Serializable]
    public struct CardInfoStruct {
        [FoldoutGroup("Card")] public int Id;
        [FoldoutGroup("Card")] public string CardName;

        private CardType _type;

        [FoldoutGroup("Card")]
        public CardType Type {
            set {
                Dictionary<string, CardType> typeMapping = new() {
                    { "Campo", CardType.CampEffect },
                    { "hackeo", CardType.Hacking },
                    { "Generador", CardType.Generator },
                    { "Arma", CardType.Weapon },
                    { "Brazo", CardType.Arm },
                    { "Pecho", CardType.Chest },
                    { "Piernas", CardType.Legs },
                };

                if (typeMapping.TryGetValue(value.ToString(), out CardType enumValue)) {
                    _type = enumValue;
                }
                else {
                    throw new ArgumentException($"Invalid CardType value: {value}");
                }
            }
            get => _type;
        }

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
        [FoldoutGroup("Card")] public BoardView DefaultMovement;
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