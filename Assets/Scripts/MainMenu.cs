using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public TMP_Text higscoreUI;
    
    private string newGameScene = "FightScene";

    public AudioClip bgMusic;
    public AudioSource mainChannel;

    private void Start()
    {
        mainChannel.PlayOneShot(bgMusic);
        int highScore = SaveLoadManager.instance.LoadHighScore();
        higscoreUI.text = $"Top Wave Survived: {highScore}";
    }

    public void StartNewGame()
    {
        mainChannel.Stop();
        SceneManager.LoadScene(newGameScene);
    }

    public void ExitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
 Application.Quit();
#endif
    }
    
}
