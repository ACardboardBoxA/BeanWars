using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BeanAOE : MonoBehaviour
{
    public BeanBehaviour beanBehaviour;
    SpriteRenderer spriteRenderer;

    public bool otherFactionsBeansAddedToList = false;
    public List<GameObject> otherFactionsBeans = new List<GameObject>();
    public GameObject targetBean;

    public bool movingToOpponent = false;
    public bool canMove = true;
    public bool isAttacking = false;

    public float attackRayLength;
    public float explosionRadius = 3.0f;

    private void Start()
    {
        beanBehaviour = GetComponent<BeanBehaviour>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (beanBehaviour.health >= 0.5f * beanBehaviour.maxHealth && beanBehaviour.canMove) DetectingOtherFaction();
    }

    private void Update()
    {
        if (beanBehaviour.health >= 0.5f * beanBehaviour.maxHealth && beanBehaviour.canMove) DetectingOtherFaction();

        FlipSprite(targetBean != null ? targetBean.transform.position.x - transform.position.x : 0f);
    }

    private void DetectingOtherFaction()
    {
        if (!otherFactionsBeansAddedToList)
        {
            string opposingTag = (gameObject.tag == "Friendly") ? "Enemy" : "Friendly";

            GameObject[] foundBeans = GameObject.FindGameObjectsWithTag(opposingTag);
            otherFactionsBeans.AddRange(foundBeans);
            otherFactionsBeansAddedToList = true;

            StartCoroutine(SweepDetection());
        }

        if (otherFactionsBeans.Count > 0)
        {
            targetBean = FindClosestBean();

            if (targetBean != null)
            {
                movingToOpponent = true;
                MoveTowardsBean();
            }

            else
            {
                movingToOpponent = false;
                if (canMove) beanBehaviour.MoveToStartArea();
            }
        }

        else
        {
            if (canMove) beanBehaviour.MoveToStartArea();
        }
    }

    private GameObject FindClosestBean()
    {
        GameObject closestOtherTeamBean = null;
        float closestOtherTeamBeanDistance = Mathf.Infinity;
        Vector3 beanFighterPosition = transform.position;

        foreach (GameObject otherTeamBean in otherFactionsBeans)
        {
            if (otherTeamBean != null && otherTeamBean.GetComponent<BeanBehaviour>() != null)
            {
                BeanBehaviour otherBeanComponent = otherTeamBean.GetComponent<BeanBehaviour>();

                if (otherBeanComponent != null)
                {
                    float distance = Vector3.Distance(beanFighterPosition, otherTeamBean.transform.position);

                    if (distance < closestOtherTeamBeanDistance)
                    {
                        closestOtherTeamBeanDistance = distance;
                        closestOtherTeamBean = otherTeamBean;
                    }
                }
            }
        }

        return closestOtherTeamBean;
    }

    private void MoveTowardsBean()
    {
        if (canMove)
        {

            if (movingToOpponent && targetBean != null)
            {
                Vector3 direction = targetBean.transform.position - gameObject.transform.position;
                direction.Normalize();

                FlipSprite(direction.x);

                transform.Translate(direction * beanBehaviour.moveSpeed * Time.deltaTime);
            }

            if (targetBean == null) return;

            else if (targetBean != null)
            {
                AttackIfPossible();
            }
        }
    }

    private void AttackIfPossible()
    {
        if (targetBean == null)
        {
            FindClosestBean();
        }

        Vector3 direction = targetBean.transform.position - gameObject.transform.position;
        direction.Normalize();

        Vector3 rayStartPoint = transform.position + direction * 0.25f;

        Debug.DrawRay(rayStartPoint, direction * attackRayLength, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(rayStartPoint, direction, attackRayLength);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject == targetBean)
            {
                Debug.Log("Ray hit targetBean: " + targetBean.name);

                StartCoroutine(AttackRoutine());

                if (targetBean == null)
                {
                    Debug.Log("TargetBean was destroyed. Setting it to null for the next sweep.");
                }
            }

            else
            {
                Debug.Log("Ray hit something else: " + hit.collider.gameObject.name);
            }
        }
    }

    private IEnumerator AttackRoutine()
    {
        canMove = false;
        isAttacking = true;
        yield return new WaitForSeconds(3.0f);

        BeanExplode();
    }

    private void BeanExplode()
    {
        Vector2 explosionCenter = transform.position;
        DrawExplosionVisualization(explosionCenter, explosionRadius);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionCenter, explosionRadius);

        foreach (Collider2D collider in colliders)
        {
            BeanBehaviour beanBehaviour = collider.GetComponent<BeanBehaviour>();
            if (beanBehaviour != null)
            {
                beanBehaviour.health -= 10;
            }
        }
    }

    private IEnumerator SweepDetection()
    {
        yield return new WaitForSeconds(3);
        UpdateOtherFactionsBeansList();
        otherFactionsBeans.Clear();
        otherFactionsBeansAddedToList = false;
        DetectingOtherFaction();
    }

    private void UpdateOtherFactionsBeansList()
    {
        otherFactionsBeans.RemoveAll(bean => bean == null);

        string opposingTag = (gameObject.tag == "Friendly") ? "Enemy" : "Friendly";
        GameObject[] foundBeans = GameObject.FindGameObjectsWithTag(opposingTag);

        foreach (GameObject bean in foundBeans)
        {
            if (!otherFactionsBeans.Contains(bean))
            {
                otherFactionsBeans.Add(bean);
            }
        }
    }

    private void DrawExplosionVisualization(Vector2 center, float radius)
    {
        int segments = 36;
        float angleIncrement = 360f / segments;

        for (int i = 0; i < segments; i++)
        {
            float angle = i * angleIncrement;
            Vector2 startPoint = center + new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle)) * radius;
            Vector2 endPoint = center + new Vector2(Mathf.Cos(Mathf.Deg2Rad * (angle + angleIncrement)), Mathf.Sin(Mathf.Deg2Rad * (angle + angleIncrement))) * radius;

            Debug.DrawLine(startPoint, endPoint, Color.red, 1.0f);
        }
    }

    void FlipSprite(float movementDirectionX)
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
}