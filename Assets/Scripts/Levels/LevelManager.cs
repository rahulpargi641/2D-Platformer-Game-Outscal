using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    public static LevelManager Instance { get { return instance; } } 

    public string[] Levels;
  
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;

            //foreach (string level in Levels)
            //{
            //    SetLevelStatus(level, E_LevelStatus.Locked);
            //}                                 // for testing if level unlocked

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (GetLevelStatus(Levels[0]) == ELevelStatus.Locked)
        {
            SetLevelStatus(Levels[0], ELevelStatus.Unlocked);
        }
    }

    public ELevelStatus GetLevelStatus(string level)
    {
        ELevelStatus levelStatus = (ELevelStatus)PlayerPrefs.GetInt(level, 0);  // 0 means Locked
        return levelStatus;
    }

    public void SetLevelStatus(string level, ELevelStatus levelStatus)
    {
        PlayerPrefs.SetInt(level, (int)levelStatus);
        Debug.Log("Setting Level: " + level + "Status" + levelStatus);
    }

    public void MarkCurrentLevelComplete()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SetLevelStatus(currentScene.name, ELevelStatus.Completed);
        //SetLevelStatus(SceneManager.GetActiveScene().name, E_LevelStatus.Completed);

        int currentSceneIndex = System.Array.FindIndex(Levels, Levels => Levels == currentScene.name);
        int nextSceneIndex = currentSceneIndex + 1;
        if(nextSceneIndex < Levels.Length)
        {
            SetLevelStatus(Levels[nextSceneIndex], ELevelStatus.Unlocked);
        }
    }
}
