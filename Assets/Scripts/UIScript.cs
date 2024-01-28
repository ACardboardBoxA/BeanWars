using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIScript : MonoBehaviour
{
    private bool resourcesFound = false;
    private Resources resources;
    private bool gameControllerFound = false;
    private GameController gameController;
    public TextMeshProUGUI totalBeanText;
    public TextMeshProUGUI totalRunnerText;

    public TextMeshProUGUI totalBeanPopText;
    public TextMeshProUGUI totalRunnerPopText;

    public GameObject loseScreen;
    public GameObject winScreen;

    /*
    private bool resourceTipDisabled = false;
    public GameObject taskFailed;
    private bool popCapTiDisabled = false;

    public TextMeshProUGUI notEnoughResources;
    public TextMeshProUGUI popCapHit;*/

    public TextMeshProUGUI notificationText;
    public GameObject taskFailed;
    private bool resourceTipDisabled;
    private bool popCapTipDisabled;


    private void Start()
    {
        if (resources = null)
        {
            Debug.Log("No Script Found");
        }

        UpdateBeanText();
    }

    private void Update()
    {
        if (!resourcesFound)
        {
            resources = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Resources>();
        }

        if (!gameControllerFound)
        {
            gameController = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameController>();
        }

        if (taskFailed.activeInHierarchy && resourceTipDisabled == false)
        {
            resourceTipDisabled = true;
            ShowMessage("Not Enough Resources");           
            StartCoroutine(DisableTaskFailed());
        }

        if (taskFailed.activeInHierarchy && popCapTipDisabled == false && gameController.playerTotalPopulation >= 100)
        {
            popCapTipDisabled = true;
            ShowMessage("Pop Cap Reached");
            StartCoroutine(DisableTaskFailed());
        }

        UpdateBeanText();

        GameOver();
    }

    private void ShowMessage(string message)
    {
        notificationText.text = message;
    }

    private IEnumerator DisableTaskFailed()
    {
        yield return new WaitForSeconds(2);
        taskFailed.SetActive(false);
        popCapTipDisabled = false;
        resourceTipDisabled = false;
    }

    private void UpdateBeanText()
    {
        if (totalBeanText != null && resources != null)
        {
            totalBeanText.text = resources.beansAmount.ToString();
        }

        if (totalRunnerText != null && resources != null)
        {
            totalRunnerText.text = resources.beansAmountAI.ToString();
        }

        if (totalBeanPopText != null && gameController != null)
        {
            totalBeanPopText.text = gameController.playerTotalPopulation.ToString() + " /100";
        }

        if (totalRunnerPopText != null && gameController != null)
        {
            totalRunnerPopText.text = gameController.enemyTotalPopulation.ToString() + " /100";
        }
    }

    private void GameOver()
    {
        if (gameController.gameIsOver == true)
        {
            if (gameController.enemyTotalPopulation == 0 && gameController.resources.beansAmountAI < 4)
            {
                winScreen.SetActive(true);
            }

            if (gameController.playerTotalPopulation == 0 && gameController.resources.beansAmount < 4)
            {
                loseScreen.SetActive(true);
            }
        }

        else return;
    }
}