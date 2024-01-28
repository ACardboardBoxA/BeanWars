using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Resources resources;
    public UIScript uiScript;

    public int playerCollectorsTotal;
    public int playerFightersTotal;
    public int playerHealersTotal;
    public int playerAOETotal;

    public int enemyCollectorsTotal;
    public int enemyFightersTotal;
    public int enemyHealersTotal;
    public int enemyAOETotal;

    public int playerTotalPopulation;
    public int enemyTotalPopulation;

    public int deathsToTriggerSpecial;

    public bool gameHasStarted = false;
    public bool gameIsOver = false;

    void Awake()
    {
        resources = GetComponent<Resources>();
        uiScript = GameObject.FindGameObjectWithTag("UI").GetComponent<UIScript>();
        Time.timeScale = 0.0f;
    }

    public void GameStarter()
    {
        Time.timeScale = 1.0f;
        GameStateLoop();
    }

    private void GameStateLoop()
    {

            StartCoroutine(GameStateChecker());
    }

    private IEnumerator GameStateChecker()
    {
        yield return new WaitForSeconds(4);
        if (playerTotalPopulation == 0 && resources.beansAmount < 4 || enemyTotalPopulation == 0 && resources.beansAmountAI < 4)
        {
            gameIsOver = true;
        }

        GameStateLoop();
    }
}