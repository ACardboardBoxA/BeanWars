using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AIDecisionTree : MonoBehaviour
{
    public GameController gameController;

    public Vector3 spawnPosition;
    public GameObject InitialSpawn;

    public GameObject[] beanCharacter;
    public int collectorCost;
    public int fighterCost;
    public int healerCost;
    public int AOECost;


    public bool startingCycle = false;
    public bool savingBeans = false;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameController>();
        spawnPosition = InitialSpawn.transform.position;

        startingCycle = true;
        RecruitmentDecision();
    }

    private void Update()
    {
        /*if (gameController != null && !startingCycle)
        {
            startingCycle = true;
            if (!savingBeans) RecruitmentDecision();
        }*/

        BonusWarriors();

    }

    private void RecruitmentDecision()
    {
        if (gameController.enemyTotalPopulation <= 100)
        {
            if (gameController.resources.beansAmountAI >= 4 && gameController.resources.beansAmountAI <= 12)
            {
                int StartDecision = Random.Range(0, 100);

                if (StartDecision >= 90 && gameController.resources.beansAmountAI > fighterCost && gameController.enemyCollectorsTotal > 4)
                {
                    GameObject beanSpawned;
                    beanSpawned = Instantiate(beanCharacter[1], spawnPosition, transform.rotation);
                    gameController.resources.beansAmountAI -= fighterCost;
                    gameController.enemyFightersTotal++;
                    gameController.enemyTotalPopulation++;
                }

                if (StartDecision >= 75 && StartDecision < 90)
                {
                    SavingBeansForBiggerSpend();
                }

                else
                {
                    GameObject beanSpawned;
                    beanSpawned = Instantiate(beanCharacter[0], spawnPosition, transform.rotation);
                    gameController.resources.beansAmountAI -= collectorCost;
                    gameController.enemyCollectorsTotal++;
                    gameController.enemyTotalPopulation++;
                }
            }

            if (gameController.resources.beansAmountAI > 12 && gameController.resources.beansAmountAI <= 50)
            {
                //econ focus with fighter backbone
                int Decision = Random.Range(0, 100);

                if (Decision >= 70 && gameController.resources.beansAmountAI > fighterCost)
                {
                    GameObject beanSpawned;
                    beanSpawned = Instantiate(beanCharacter[1], spawnPosition, transform.rotation);
                    gameController.resources.beansAmountAI -= fighterCost;
                    gameController.enemyFightersTotal++;
                    gameController.enemyTotalPopulation++;
                }

                if (Decision >= 60 && Decision < 70)
                {
                    SavingBeansForBiggerSpend();
                }

                else
                {
                    GameObject beanSpawned;
                    beanSpawned = Instantiate(beanCharacter[0], spawnPosition, transform.rotation);
                    gameController.resources.beansAmountAI -= collectorCost;
                    gameController.enemyCollectorsTotal++;
                    gameController.enemyTotalPopulation++;
                }
            }

            if (gameController.resources.beansAmountAI > 50 && gameController.resources.beansAmountAI < 100)
            {
                //more heavier troops
                //aoe and healer should make a show here
                int Decision = Random.Range(0, 100);

                if (Decision <= 50)
                {
                    GameObject beanSpawned;
                    beanSpawned = Instantiate(beanCharacter[0], spawnPosition, transform.rotation);
                    gameController.resources.beansAmountAI -= collectorCost;
                    gameController.enemyCollectorsTotal++;
                    gameController.enemyTotalPopulation++;
                }

                if (Decision > 50 && Decision < 90 && gameController.resources.beansAmountAI > fighterCost)
                {
                    GameObject beanSpawned;
                    beanSpawned = Instantiate(beanCharacter[1], spawnPosition, transform.rotation);
                    gameController.resources.beansAmountAI -= fighterCost;
                    gameController.enemyFightersTotal++;
                    gameController.enemyTotalPopulation++;
                }


                if (Decision >= 90 && gameController.resources.beansAmountAI > healerCost)
                {
                    GameObject beanSpawned;
                    beanSpawned = Instantiate(beanCharacter[2], spawnPosition, transform.rotation);
                    gameController.resources.beansAmountAI -= healerCost;
                    gameController.enemyHealersTotal++;
                    gameController.enemyTotalPopulation++;
                }
            }


            if (gameController.resources.beansAmountAI >= 100)
            {
                //total war
                int Decision = Random.Range(0, 100);

                if (Decision <= 30)
                {
                    GameObject beanSpawned;
                    beanSpawned = Instantiate(beanCharacter[0], spawnPosition, transform.rotation);
                    gameController.resources.beansAmountAI -= collectorCost;
                    gameController.enemyCollectorsTotal++;
                    gameController.enemyTotalPopulation++;
                }

                if (Decision > 30 && Decision < 60 && gameController.resources.beansAmountAI > fighterCost)
                {
                    GameObject beanSpawned;
                    beanSpawned = Instantiate(beanCharacter[1], spawnPosition, transform.rotation);
                    gameController.resources.beansAmountAI -= fighterCost;
                    gameController.enemyFightersTotal++;
                    gameController.enemyTotalPopulation++;
                }

                if (Decision >= 60 && Decision < 80 && gameController.resources.beansAmountAI > healerCost)
                {
                    GameObject beanSpawned;
                    beanSpawned = Instantiate(beanCharacter[2], spawnPosition, transform.rotation);
                    gameController.resources.beansAmountAI -= healerCost;
                    gameController.enemyHealersTotal++;
                    gameController.enemyTotalPopulation++;
                }


                if (Decision >= 80 && gameController.resources.beansAmountAI > AOECost)
                {
                    GameObject beanSpawned;
                    beanSpawned = Instantiate(beanCharacter[3], spawnPosition, transform.rotation);
                    gameController.resources.beansAmountAI -= AOECost;
                    gameController.enemyAOETotal++;
                    gameController.enemyTotalPopulation++;
                }
            }
        }

        StartCoroutine(RecruitingCoolDown());
    }

    private void BonusWarriors()
    {

        if (gameController.deathsToTriggerSpecial >= 10)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject beanSpawned;
                beanSpawned = Instantiate(beanCharacter[1], spawnPosition, transform.rotation);
                gameController.resources.beansAmountAI -= fighterCost;
                gameController.enemyFightersTotal++;
                gameController.enemyTotalPopulation++;
            }

            gameController.deathsToTriggerSpecial = 0;
        }

        else return;
    }

    private void SavingBeansForBiggerSpend()
    {
        Debug.Log("Saving Beans");
        savingBeans = true;
    }

    private IEnumerator RecruitingCoolDown()
    {
        if (!savingBeans)
        {
            float decisionCoolDown = Random.Range(0, 5);
            yield return new WaitForSeconds(decisionCoolDown);
            RecruitmentDecision();
        }

        if(savingBeans)
        {
            yield return new WaitForSeconds(5);
            savingBeans = false;
            RecruitmentDecision();
        }
    }
}
