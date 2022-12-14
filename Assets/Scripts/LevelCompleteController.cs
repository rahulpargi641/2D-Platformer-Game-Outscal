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
            Debug.Log("Level Complete");
            levelCompletePanel.SetActive(true);
            SoundManager.Instance.PlayLevelCompleteMusic(ESounds.LevelComplete);
            LevelManager.Instance.MarkCurrentLevelComplete();
            StartCoroutine(LoadNextLevel());

        }
        else
        {
            Debug.Log("Level Not Complete");
        }
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        levelCompletePanel.SetActive(false);
    }


}
