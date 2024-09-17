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
        ELevelStatus levelStatus = LevelManager.Instance.GetLevelStatus(LevelName);
        switch(levelStatus)
        {
            case ELevelStatus.Locked:
                Debug.Log("Can't play level till you unlock it");
                SoundManager.Instance.PlayButtonClickSound(ESounds.MenuButtonClick);
                break;

            case ELevelStatus.Unlocked:
                SoundManager.Instance.PlayButtonClickSound(ESounds.MenuButtonSelectLevel);
                SceneManager.LoadScene(LevelName);
                break;

            case ELevelStatus.Completed:
                SoundManager.Instance.PlayButtonClickSound(ESounds.MenuButtonSelectLevel);
                SceneManager.LoadScene(LevelName);
                break;
        }
    }
}
