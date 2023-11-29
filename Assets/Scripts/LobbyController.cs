using UnityEngine;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    public Button m_StartButton;
    public Button m_QuitButton;
    [SerializeField] GameObject m_MainMenuGO;
    [SerializeField] GameObject m_LevelSelectionMenuGO;

    private void Awake()
    {
       m_StartButton.onClick.AddListener(PlayGame);
       m_QuitButton.onClick.AddListener(QuitGame);
    }

    private void PlayGame()
    {
        //SceneManager.LoadScene(1);
        SoundManager.Instance.PlayButtonClickSound(ESounds.MenuButtonStart);
        m_MainMenuGO.SetActive(false);
        m_LevelSelectionMenuGO.SetActive(true);
    }

    private void QuitGame()
    {
        Application.Quit();
        SoundManager.Instance.PlayButtonClickSound(ESounds.MenuButtonClick);
    }
}
