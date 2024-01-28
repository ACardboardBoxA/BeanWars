using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Resources : MonoBehaviour
{
    private GameController gameController;

    public string beansName = "Baked Beans";
    public int beansAmount = 5;

    public string beansNameAI = "Runner Beans";
    public int beansAmountAI = 5;


    private void Start()
    {
        gameController = GetComponent<GameController>();
    }

    private void Update()
    {
        if (beansAmount > 1000)
        {
            beansAmount = 1000;
        }

        if (beansAmountAI > 1000)
        {
            beansAmountAI = 1000;
        }

        if (beansAmount < 0)
        {
            beansAmount = 0;
        }

        if (beansAmountAI < 0)
        {
            beansAmountAI = 0;
        }
    }

    public void AddBeanToPlayerResource()
    {
        beansAmount += 1;
    }

    public void AddBeanToEnemyResource()
    {
        beansAmountAI += 1;
    }
}
