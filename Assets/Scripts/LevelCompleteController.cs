using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteController : MonoBehaviour
{
    public GameObject levelCompletePanel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>() != null)
        {
            levelCompletePanel.SetActive(true);
            SoundManager.Instance.PlayLevelCompleteMusic(ESounds.LevelComplete);
            LevelManager.Instance.MarkCurrentLevelComplete();
            StartCoroutine(LoadNextLevel());

        }
        else
        {
            Debug.Log("Level is not Completed");
        }
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        levelCompletePanel.SetActive(false);
    }


}
