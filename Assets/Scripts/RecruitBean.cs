using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecruitBean : MonoBehaviour
{
    public string beanName;
    public int requiredAmountToRecruit;
    private GameController gameController;
    public GameObject beanCharacter;

    public Vector3 spawnPosition;
    public GameObject InitialSpawn;


    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameController>();
    }

    public void OnButtonClick()
    {
        spawnPosition = InitialSpawn.transform.position;

        if (gameController.resources.beansAmount >= requiredAmountToRecruit && gameController.playerTotalPopulation <= 99)
        {
            GameObject beanSpawned;
            beanSpawned = Instantiate(beanCharacter, spawnPosition, transform.rotation);
            beanSpawned.name = beanName;
            gameController.resources.beansAmount -= requiredAmountToRecruit;


            gameController.playerTotalPopulation++;
        }

        else if (gameController.resources.beansAmount < requiredAmountToRecruit)
        {
            gameController.uiScript.taskFailed.SetActive(true);
        }

        else if (gameController.playerTotalPopulation > 100)
        {
            gameController.uiScript.taskFailed.SetActive(true);
        }
    }
}
