using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {
    [SerializeField] private Button playButton;

    private void Start() {
        GameManager.Instance.OnMasterServerConnected += OnMasterServerConnected;
    }

    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void OnExitButton() {
        Debug.Log($"Closing the game");        
        Application.Quit();
    }

    private void OnMasterServerConnected() {
        playButton.interactable = true;
    }
}