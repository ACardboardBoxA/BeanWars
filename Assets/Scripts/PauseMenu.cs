using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject featureNotIn;
    public string sceneName;

    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    public void OnClickPause()
    {
        Time.timeScale = 0.0f;
        pauseMenu.SetActive(true);
    }

    public void OnClickResume()
    {
        Time.timeScale = 1.0f;
        pauseMenu.SetActive(false);
    }

    public void OnClickOptions()
    {
        featureNotIn.SetActive(true);
        StartCoroutine(FeatureNotEnabled());
    }

    public void OnClickQuit()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OnClickRestart()
    {
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator FeatureNotEnabled()
    {
        yield return new WaitForSeconds(2);
        featureNotIn.SetActive(false);
    }
}
