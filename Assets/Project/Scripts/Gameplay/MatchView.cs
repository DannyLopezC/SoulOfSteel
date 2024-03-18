using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public interface IMatchView {
    void SetCurrentPhaseText(string text);
}

public class MatchView : MonoBehaviour, IMatchView {
    [SerializeField] private TMP_Text currentPhaseText;

    private IMatchController _matchController;

    public IMatchController MatchController {
        get { return _matchController ??= new MatchController(this); }
    }

    private void Awake() {
        GameManager.Instance.ExecutePhases += ExecutePhases;
    }

    private void Start() {
        StartCoroutine(MatchController.PrepareMatch());
    }

    public void SetCurrentPhaseText(string text) {
        currentPhaseText.text = text;
    }

    private void ExecutePhases() {
        StartCoroutine(MatchController.ExecutePhases());
    }
    
    private void OnDestroy() {
        GameManager.Instance.ExecutePhases -= ExecutePhases;
    }
}
