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
    PhotonView GetPv();
}

[Serializable]
public class PlayerView : MonoBehaviourPunCallbacks, IPlayerView, IPunObservable {
    [SerializeField] private PhotonView pv;
    [SerializeField] private PlayerCardsInfo _deckInfo;

    public bool _inAnimation;
    private bool _receivePriority;
    private GameObject _handCardsPanel;
    private bool _myEffectTurn;
    private bool _effectTurnDone;

    public GameObject HandCardsPanel {
        get { return _handCardsPanel ??= GameManager.Instance.handPanel.gameObject; }
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
        pv = GetComponent<PhotonView>();
        GameManager.Instance.playerList.Add(this);
        if (pv.IsMine) {
            GameManager.Instance.LocalPlayerInstance = this;
        }

        PlayerController.SetPlayerId(pv.Owner.ActorNumber);
    }

    private void Start() {
        if (GameManager.Instance.testing) TurnOnSprite();
        GameManager.Instance.OnPrioritySetEvent += ReceivePriority;
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
                // Debug.Log($"Prefab not found, using pilot prefab");
                break;
        }

        GameObject GO = Instantiate(prefab, HandCardsPanel.transform);

        GO.TryGetComponent(out RectTransform rt);
        rt.sizeDelta = new Vector2(250, 350);
        rt.localScale = new Vector3(0.7f, 0.7f, 0.7f);

        GO.TryGetComponent(out CardView card);
        GO.SetActive(pv.IsMine);
        return card;
    }

    public void InitAddCards(int amount) {
        StartCoroutine(PlayerController.AddCards(amount));
    }

    public PhotonView GetPv() {
        return pv;
    }

    public void SetCardsInfo() {
        if (pv.IsMine) {
            // Debug.Log($"actor number: {pv.Owner.ActorNumber}");

            int actorNumber = GameManager.Instance.testing ? 0 : pv.Owner.ActorNumber;
            int count = GameManager.Instance.cardDataBase.cardDataBase.Sheet1.Count;
            int halfCount = count / 2;

            int startIndex = actorNumber == 1 ? 0 : halfCount;

            _deckInfo = Resources.Load<PlayerCardsInfo>($"PlayerCards{1}");

            if (!GameManager.Instance.testing) {
                // _deckInfo.SetPlayerCards(Enumerable
                //     .Range(startIndex, actorNumber == 1 ? halfCount : count - startIndex)
                //     .ToList());

                _deckInfo.SetPlayerCards(new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            }
            else {
                _deckInfo.SetPlayerCards(new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 });
            }
        }
    }

    private void OnDestroy() {
        if (GameManager.HasInstance()) {
            GameManager.Instance.OnPrioritySetEvent -= ReceivePriority;
            GameManager.Instance.OnGameStartedEvent -= TurnOnSprite;
        }
    }

    public void DrawCards(int amount, bool fullDraw) {
        PlayerController.DrawCards(amount, fullDraw);
    }

    public void SelectCards(CardType type, int amount) {
        PlayerController.SelectCards(type, amount);
    }

    public void ReceivePriority(int priority) {
        _receivePriority = true;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting && pv.IsMine) {
            stream.SendNext(PlayerController.GetCardsSelected());
            stream.SendNext(PlayerController.GetPlayerId());

            stream.SendNext(GameManager.Instance.currentPriority);
            stream.SendNext(EffectManager.Instance.effectTurn);

            stream.SendNext(PlayerController.GetAllEffectsDone());
            Debug.Log(
                $"sending from player {PlayerController.GetPlayerId()} get all effects done {PlayerController.GetAllEffectsDone()} \n");
        }
        else if (stream.IsReading) {
            bool receivedSelection = (bool)stream.ReceiveNext();
            int receivedPlayerId = (int)stream.ReceiveNext();

            int receivedPriority = (int)stream.ReceiveNext();
            int receivedEffectTurn = (int)stream.ReceiveNext();

            bool receivedAllEffectsDone = (bool)stream.ReceiveNext();

            foreach (PlayerView player in GameManager.Instance.playerList) {
                if (receivedPlayerId == player.pv.Owner.ActorNumber) {
                    player.PlayerController.SetCardsSelected(receivedSelection);
                }
            }

            foreach (PlayerView player in GameManager.Instance.playerList) {
                if (receivedPlayerId == player.PlayerController.GetPlayerId()) {
                    player.PlayerController.SetAllEffectsDone(receivedAllEffectsDone);
                }
            }

            if (!_myEffectTurn) EffectManager.Instance.effectTurn = receivedEffectTurn;

            if (!PhotonNetwork.IsMasterClient && _receivePriority) {
                GameManager.Instance.currentPriority = receivedPriority;
                _receivePriority = false;
            }
        }
    }

    public void SelectCells(int amount) {
        StartCoroutine(PlayerController.SelectCells(amount));
    }

    public void SetMyEffectTurn(bool myEffectTurn) {
        this._myEffectTurn = myEffectTurn;
    }

    public void SetEffectTurnDone(bool effectTurnDone) {
        _effectTurnDone = effectTurnDone;
    }

    public bool GetEffectTurnDone() {
        return _effectTurnDone;
    }
}