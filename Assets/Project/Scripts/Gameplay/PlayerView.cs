using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public interface IPlayerView {
    GameObject GetHandCardsPanel();
    void CleanHandsPanel();
    void AddCardToPanel(CardView card);
    void InitAddCards(int amount);

}

[Serializable]
public class PlayerView : MonoBehaviour, IPlayerView {
    [SerializeField] private List<CardView> deck;

    [SerializeField] private GameObject handCardsPanel;

    [SerializeField] private GameObject equipmentCardPrefab;
    [SerializeField] private GameObject pilotCardPrefab;
    [SerializeField] private GameObject effectCardPrefab;
    
    private IPlayerController _playerController;
    private IPlayerView _playerViewImplementation;

    public IPlayerController PlayerController {
        get { return _playerController ??= new PlayerController(this, deck); }
    }

    public GameObject GetHandCardsPanel() {
        return handCardsPanel;
    }

    public void CleanHandsPanel() {
        foreach (Transform t in handCardsPanel.transform) {
            Destroy(t.gameObject);
        }
    }

    public void AddCardToPanel(CardView card) {
        CardType type = card.GetCardType();

        GameObject prefab;
        switch (type) {
            case CardType.Pilot:
                prefab = pilotCardPrefab;
                break;
            case CardType.Weapon:
            case CardType.Armor:
                prefab = equipmentCardPrefab;
                break;
            case CardType.CampEffect:
                prefab = effectCardPrefab;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        GameObject GO = Instantiate(prefab, handCardsPanel.transform);

        GO.TryGetComponent(out RectTransform rt);
        rt.sizeDelta = new Vector2(250, 350);
        rt.localScale = new Vector3(0.7f, 0.7f, 0.7f);
    }

    public void InitAddCards(int amount) {
        StartCoroutine(PlayerController.AddCards(amount));
    }
}
