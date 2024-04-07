using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager instance;
    private string highScoreKey = "BestWaveSavedValue";
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this);
    }

    public void SaveHighScore(int score)
    {
        PlayerPrefs.SetInt(highScoreKey,score);
    }

    public int LoadHighScore()
    {
        if (PlayerPrefs.HasKey(highScoreKey))
        {
            return PlayerPrefs.GetInt(highScoreKey);
        }
        else
        {
            return 0;
        }
    }
}
