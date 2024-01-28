using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanBehaviour : MonoBehaviour
{
    public GameController gameController;

    public string beanName;
    public float moveSpeed;
    public int health;
    public int maxHealth;
    public bool isWounded;
    public bool isHealing { get; set; }
    public bool canMove = true;

    public Transform retreatPoint;

    public Transform startArea;
    public bool startingAreaMovement = true;

    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        if (gameObject.tag == "Friendly")
        {
            startArea = GameObject.Find("BeanRunFromSpawn").transform;
            retreatPoint = GameObject.Find("BeanSpawn").transform;
        }

        if (gameObject.tag == "Enemy")
        {
            startArea = GameObject.Find("RunnerRunFromSpawn").transform;
            retreatPoint = GameObject.Find("RunnerSpawn").transform;
        }
    }

    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameController>(); 
        spriteRenderer = GetComponent<SpriteRenderer>();

        maxHealth = health;
    }

    private void Update()
    {
        if (startingAreaMovement && canMove)
        {
            MoveToStartArea();

            if (Vector3.Distance(transform.position, startArea.position) < 0.1f)
            {
                startingAreaMovement = false;
            }
        }

        if (health == maxHealth)
        {
            isWounded = false;
            canMove = true;
        }

        if (health < 1f * maxHealth)
        {
            isWounded = true;
        }

        if (health < 0.5f * maxHealth)
        {
            //MoveToStartArea();
            FindHealer();
        }

        if (health <1)
        {
            BeanDeath();
        }
    }

    public void MoveToStartArea()
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startArea.position;

        Vector3 direction = targetPosition - startPosition;
        direction.Normalize();

        transform.Translate(direction * moveSpeed * Time.deltaTime);

        FlipSprite(direction.x);
    }

    private void FlipSprite(float movementDirectionX)
    {
        if (movementDirectionX > 0)
        {
            spriteRenderer.flipX = false;
        }

        else if (movementDirectionX < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void FindHealer()
    {
        Debug.Log("I need amberlamps");
    }

    private void BeanDeath()
    {
        if (beanName == "Collector")
        {
            Transform carryingTransform = transform.Find("CarryingTransform");
            Transform nakedBeanChild = carryingTransform.Find("Naked Bean");

            if (nakedBeanChild != null)
            {
                nakedBeanChild.SetParent(null);
            }
        }

        if (gameObject.tag == "Friendly")
        {
            gameController.playerTotalPopulation--;
        }

        if (gameObject.tag == "Enemy")
        {
            gameController.enemyTotalPopulation--;
            gameController.deathsToTriggerSpecial++;
        }

        Destroy(gameObject);
    }
}