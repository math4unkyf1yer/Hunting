using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveLoad : MonoBehaviour
{
    public static SaveLoad instance;
    private SaveLoadManager saveLoadManager;
    public int[] summerHighScore;
    public int[] winterHighScore;
    public int[] fallHighScore;
    public TextMeshProUGUI[] summerHighScoreText;
    public TextMeshProUGUI[] winterHighScoreText;
    GameData data;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            saveLoadManager = FindObjectOfType<SaveLoadManager>();
            if (saveLoadManager != null)
            {
                data = saveLoadManager.LoadGame();
                if (data == null)
                {
                    data = new GameData(); // fallback
                                           // Manually initialize arrays to match your score lengths
                    data.saveSummerScore = new int[summerHighScore.Length];
                    data.saveWinterScore = new int[winterHighScore.Length];
                    data.saveFallScore = new int[fallHighScore.Length];
                }
                LoadHighScore();
            }
        }
        else
        {
            Destroy(gameObject); // Kill duplicate
        }
    }


    public void LoadHighScore()
    {
        if (saveLoadManager != null && data != null)
        {
            for (int i = 0; i < summerHighScore.Length; i++)
            {
                summerHighScore[i] = data.saveSummerScore[i];
                winterHighScore[i] = data.saveWinterScore[i];
                fallHighScore[i] = data.saveFallScore[i];
            }
        }
    }
    public void SaveHighScore()
    {
        if (data == null || saveLoadManager == null)
        {
            Debug.LogWarning("SaveHighScore failed: GameData or SaveLoadManager is null.");
            return;
        }
        for (int i = 0; i < summerHighScore.Length; i++)
        {
            data.saveSummerScore[i] = summerHighScore[i];
            data.saveWinterScore[i] = winterHighScore[i];
            data.saveFallScore[i] = fallHighScore[i];
        }
        saveLoadManager.SaveGame(data);
    }
    public void ChangeHighScore(int recentScore, int scene)
    {
        if(scene == 0)
        {
            for (int i = 0; i < summerHighScore.Length; i++)
            {
                if (recentScore > summerHighScore[i])
                {
                    // Shift scores down from the bottom up to the current position
                    for (int j = summerHighScore.Length - 1; j > i; j--)
                    {
                        summerHighScore[j] = summerHighScore[j - 1];
                    }

                    // Insert the new high score at the correct position
                    summerHighScore[i] = recentScore;
                    break;
                }
            }
        }
        else if(scene == 1)
        {
            for (int i = 0; i < winterHighScore.Length; i++)
            {
                if (recentScore > winterHighScore[i])
                {
                    // Shift scores down from the bottom up to the current position
                    for (int j = winterHighScore.Length - 1; j > i; j--)
                    {
                        winterHighScore[j] = winterHighScore[j - 1];
                    }

                    // Insert the new high score at the correct position
                    winterHighScore[i] = recentScore;
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < fallHighScore.Length; i++)
            {
                if (recentScore > fallHighScore[i])
                {
                    // Shift scores down from the bottom up to the current position
                    for (int j = fallHighScore.Length - 1; j > i; j--)
                    {
                        fallHighScore[j] = fallHighScore[j - 1];
                    }

                    // Insert the new high score at the correct position
                    fallHighScore[i] = recentScore;
                    break;
                }
            }
        }
    }

}
