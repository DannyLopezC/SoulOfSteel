using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviourSingleton<UIManager> {
    [SerializeField] private GameObject gamePanel;
    private MatchView _currentGamePanel;
    
    [SerializeField] private GameObject cardPanel;
    private CardPanel _currentCardPanel;

    [SerializeField] private GameObject waitingForOpponentPanel;
    private Canvas _currentWaitingForOpponentPanel;

    public void ShowWaitingForOpponentPanel(bool activate = true) {
        FindOrInstantiatePanel(ref _currentWaitingForOpponentPanel, waitingForOpponentPanel);

        _currentWaitingForOpponentPanel.gameObject.SetActive(activate);
    }

    public void ShowGamePanel(bool activate = true) {
        FindOrInstantiatePanel(ref _currentGamePanel, gamePanel);

        _currentGamePanel.gameObject.SetActive(activate);
        ShowWaitingForOpponentPanel(false);
    }

    public void ShowCardPanel(string cardName, string cardDescription,
        int scrapCost, Sprite imageSource, bool activate = true) {
        FindOrInstantiatePanel(ref _currentCardPanel, cardPanel);

        _currentCardPanel.Init(cardName, cardDescription, scrapCost, imageSource);
        _currentCardPanel.gameObject.SetActive(activate);
    }

    private static void FindOrInstantiatePanel<T>(ref T panel, GameObject prefab) where T : Component {
        if (panel == null) {
            Instantiate(prefab).TryGetComponent<T>(out panel);
        }
    }
}
