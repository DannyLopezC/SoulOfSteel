using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public interface IPlayerView {
    GameObject GetHandCardsPanel();
    void CleanHandsPanel();
    CardView AddCardToPanel(CardType cardType);
    void InitAddCards(int amount);
}

[Serializable]
public class PlayerView : MonoBehaviour, IPlayerView {
    [SerializeField] private PhotonView pv;
    [SerializeField] private PlayerCardsInfo _deckInfo;

    private GameObject _handCardsPanel;

    public GameObject HandCardsPanel {
        get { return _handCardsPanel ??= GameManager.Instance.handPanel; }
    }

    [SerializeField] private GameObject equipmentCardPrefab;
    [SerializeField] private GameObject pilotCardPrefab;
    [SerializeField] private GameObject effectCardPrefab;

    [SerializeField] private TMP_Text playerName;

    private IPlayerController _playerController;
    private IPlayerView _playerViewImplementation;

    public IPlayerController PlayerController {
        get { return _playerController ??= new PlayerController(this, _deckInfo); }
    }

    private void Awake() {
        GameManager.Instance.OnGameStartedEvent += TurnOnSprite;
    }

    private void Start() {
        pv = GetComponent<PhotonView>();
        GameManager.Instance.playerList.Add(this);

        if (pv.IsMine) {
            GameManager.Instance.LocalPlayerInstance = gameObject;
        }

        if (GameManager.Instance.testing) TurnOnSprite();
    }

    public void TurnOnSprite() {
        TryGetComponent(out Image image);
        image.enabled = true;
        playerName.gameObject.SetActive(true);
        if (!GameManager.Instance.testing) playerName.text = pv.Owner.NickName;

        SetCardsInfo();
    }

    public GameObject GetHandCardsPanel() {
        return HandCardsPanel;
    }

    public void CleanHandsPanel() {
        foreach (Transform t in HandCardsPanel.transform) {
            Destroy(t.gameObject);
        }
    }

    public CardView AddCardToPanel(CardType cardType) {
        GameObject prefab;
        switch (cardType) {
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
                prefab = pilotCardPrefab;
                Debug.Log($"Prefab not found, using pilot prefab");
                break;
        }

        GameObject GO = Instantiate(prefab, HandCardsPanel.transform);

        GO.TryGetComponent(out RectTransform rt);
        rt.sizeDelta = new Vector2(250, 350);
        rt.localScale = new Vector3(0.7f, 0.7f, 0.7f);

        GO.TryGetComponent(out CardView card);
        return card;
    }

    public void InitAddCards(int amount) {
        StartCoroutine(PlayerController.AddCards(amount));
    }

    public void SetCardsInfo() {
        if (pv.IsMine) {
            // Debug.Log($"actor number: {pv.Owner.ActorNumber}");

            int actorNumber = GameManager.Instance.testing ? 1 : pv.Owner.ActorNumber;
            int count = GameManager.Instance.cardDataBase.cardDataBase.Sheet1.Count;
            int halfCount = count / 2;

            int startIndex = actorNumber == 1 ? 0 : halfCount;

            _deckInfo = Resources.Load<PlayerCardsInfo>($"PlayerCards{actorNumber}");

            _deckInfo.SetPlayerCards(Enumerable
                .Range(startIndex, actorNumber == 1 ? halfCount : count - startIndex)
                .ToList());
        }
    }

    private void OnDestroy() {
        if (GameManager.HasInstance()) GameManager.Instance.OnGameStartedEvent -= TurnOnSprite;
    }

    public void DrawCards(int amount, bool fullDraw) {
        if (pv.IsMine) {
            PlayerController.DrawCards(amount, fullDraw);
        }
    }

    public void SelectCards(CardType type, int amount) {
        if (pv.IsMine) {
            StartCoroutine(PlayerController.SelectCards(type, amount));
        }
    }
}