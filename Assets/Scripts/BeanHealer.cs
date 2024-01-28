using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BeanHealer : MonoBehaviour
{
    public BeanBehaviour beanBehaviour;

    public List<GameObject> woundedBeans = new List<GameObject>();
    public bool woundedBeansAddedToList = false;

    public bool canMove = true;
    public bool movingToWoundedBean = false;
    public GameObject targetBean;

    SpriteRenderer spriteRenderer;

    void Start()
    {
        beanBehaviour = GetComponent<BeanBehaviour>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        DetectWoundedBeans(gameObject.tag);
    }

    private void Update()
    {
        MoveTowardsWoundedBean();

        if (targetBean == null) beanBehaviour.startingAreaMovement = true;
        FlipSprite(targetBean != null ? targetBean.transform.position.x - transform.position.x : 0f);
    }

    private void DetectWoundedBeans(string healerTag)
    {
        if (!woundedBeansAddedToList)
        {
            string beanTag;

            if (healerTag == "Friendly")
            {
                beanTag = "Friendly";
            }
            else
            {
                beanTag = "Enemy";
            }

            GameObject[] beansWithTag = GameObject.FindGameObjectsWithTag(beanTag);

            foreach (GameObject bean in beansWithTag)
            {
                BeanBehaviour beanBehaviour = bean.GetComponent<BeanBehaviour>();

                if (beanBehaviour != null && beanBehaviour.isWounded && bean.tag == healerTag)
                {
                    woundedBeans.Add(bean);
                }
            }

            woundedBeansAddedToList = true;
        }

        StartCoroutine(DetectedWoundedBeans());
    }

    private void MoveTowardsWoundedBean()
    {
        if (canMove)
        {
            if (woundedBeans.Count > 0)
            {
                GameObject closestWoundedBean = FindClosestWoundedBean();

                if (closestWoundedBean != null)
                {
                    targetBean = closestWoundedBean;

                    // Check if the coroutine is already running for the current targetBean
                    if (!targetBean.GetComponent<BeanBehaviour>().isHealing)
                    {
                        movingToWoundedBean = true;

                        Vector3 direction = targetBean.transform.position - gameObject.transform.position;
                        float distance = direction.magnitude;

                        if (distance > 0.5f)
                        {
                            direction.Normalize();
                            transform.Translate(direction * beanBehaviour.moveSpeed * Time.deltaTime);

                            FlipSprite(direction.x);
                        }

                        else
                        {
                            movingToWoundedBean = false;
                            StartCoroutine(HealBeanOverTime(targetBean.GetComponent<BeanBehaviour>()));
                        }
                    }
                }
            }

            else
            {
                movingToWoundedBean = false;
                targetBean = null;
            }
        }
    }

    private GameObject FindClosestWoundedBean()
    {
        GameObject closestBean = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject bean in woundedBeans)
        {
            float distance = Vector3.Distance(transform.position, bean.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestBean = bean;
            }
        }

        return closestBean;
    }

    private IEnumerator DetectedWoundedBeans()
    {
        yield return new WaitForSeconds(3);
        woundedBeansAddedToList = false;
        woundedBeans.Clear();
        DetectWoundedBeans(gameObject.tag);
    }

    private IEnumerator HealBeanOverTime(BeanBehaviour targetBeanBehaviour)
    {
        targetBeanBehaviour.canMove = false;
        targetBeanBehaviour.isHealing = true;

        float elapsedTime = 0f;
        float healInterval = 1f; 

        while (targetBean != null && targetBeanBehaviour.health < targetBeanBehaviour.maxHealth)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= healInterval)
            {
                targetBeanBehaviour.health += 1;
                elapsedTime = 0f; 

                if (targetBeanBehaviour.health >= targetBeanBehaviour.maxHealth)
                {
                    targetBeanBehaviour.health = targetBeanBehaviour.maxHealth;
                    yield break; 
                }
            }

            yield return null; 
        }

        targetBeanBehaviour.isHealing = false;
    }

    void FlipSprite(float movementDirectionX)
    {
        if (movementDirectionX > 0)
        {
            // Moving right, set localScale.x to positive
            spriteRenderer.flipX = false;
        }
        else if (movementDirectionX < 0)
        {
            // Moving left, set localScale.x to negative
            spriteRenderer.flipX = true;
        }
        // If movementDirectionX is 0, the sprite will retain its current orientation
    }
}