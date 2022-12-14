using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class LevelLoader : MonoBehaviour
{
    Button buttonLoadLevel;
    public string LevelName;
    private void Awake()
    {
        buttonLoadLevel = GetComponent<Button>();
        buttonLoadLevel.onClick.AddListener(OnClickLoadLevelButton);
    }

    private void OnClickLoadLevelButton()
    {
        E_LevelStatus levelStatus = LevelManager.Instance.GetLevelStatus(LevelName);
        switch(levelStatus)
        {
            case E_LevelStatus.Locked:
                Debug.Log("Can't play level till you unlock it");
                SoundManager.Instance.PlayButtonClickSound(ESounds.MenuButtonClick);
                break;

            case E_LevelStatus.Unlocked:
                SoundManager.Instance.PlayButtonClickSound(ESounds.MenuButtonSelectLevel);
                SceneManager.LoadScene(LevelName);
                break;

            case E_LevelStatus.Completed:
                SoundManager.Instance.PlayButtonClickSound(ESounds.MenuButtonSelectLevel);
                SceneManager.LoadScene(LevelName);
                break;
        }
    }
}
