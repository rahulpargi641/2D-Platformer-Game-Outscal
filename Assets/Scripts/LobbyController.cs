using UnityEngine;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    public Button startButton;
    public Button quitButton;
    [SerializeField] GameObject InitialScreen;
    [SerializeField] GameObject LevelSelection;
    private void Awake()
    {
       startButton.onClick.AddListener(PlayGame);
       quitButton.onClick.AddListener(QuitGame);
    }

    private void PlayGame()
    {
        //SceneManager.LoadScene(1);
        SoundManager.Instance.PlayButtonClickSound(ESounds.MenuButtonStart);
        InitialScreen.SetActive(false);
        LevelSelection.SetActive(true);
    }
    private void QuitGame()
    {
        Application.Quit();
        SoundManager.Instance.PlayButtonClickSound(ESounds.MenuButtonClick);
    }
}
