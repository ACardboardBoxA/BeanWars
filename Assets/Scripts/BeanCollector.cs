using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanCollector : MonoBehaviour
{
    public BeanBehaviour beanBehaviour;

    public List<GameObject> nakedBeans = new List<GameObject>();
    public bool beansAddedToList = false;
    public bool isCarryingBean = false;
    public Transform carryingBean;
    public Transform dropOffPoint;

    SpriteRenderer spriteRenderer;


    private void Start()
    {
        beanBehaviour = GetComponent<BeanBehaviour>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (gameObject.tag == "Friendly")
        {
            dropOffPoint = GameObject.Find("BeanSpawn").transform;
        }

        if (gameObject.tag == "Enemy")
        {
            dropOffPoint = GameObject.Find("RunnerSpawn").transform;
        }

        if (beanBehaviour.health >= 0.5f * beanBehaviour.maxHealth && beanBehaviour.canMove) CollectorCoroutines();
    }

    private void Update()
    {
        if (beanBehaviour.health >= 0.5f * beanBehaviour.maxHealth && beanBehaviour.canMove) CollectorBehaviour();
        FlipSprite(isCarryingBean ? dropOffPoint.position.x - transform.position.x : 0f);
    }

    private void CollectorCoroutines()
    {
        if (!beansAddedToList)
        {
            nakedBeans.AddRange(GameObject.FindGameObjectsWithTag("NakedBean"));
            beansAddedToList = true;
        }

        StartCoroutine(BeanUpdater());
    }

    private void CollectorBehaviour()
    {
        if (!isCarryingBean)
        {
            if (nakedBeans.Count > 0)
            {
                GameObject closestBean = null;
                float closestBeanDistance = Mathf.Infinity;
                Vector3 beanCollectorPosition = transform.position;

                foreach (GameObject nakedBean in nakedBeans)
                {
                    if (nakedBean != null && nakedBean.GetComponent<NakedBean>() != null)
                    {
                        NakedBean beanComponent = nakedBean.GetComponent<NakedBean>();

                        if (beanComponent != null && !beanComponent.isBeingCarried)
                        {
                            float distance = Vector3.Distance(beanCollectorPosition, nakedBean.transform.position);

                            if (distance < closestBeanDistance)
                            {
                                closestBeanDistance = distance;
                                closestBean = nakedBean;
                            }
                        }
                    }
                }

                if (closestBean != null)
                {
                    Vector3 direction = closestBean.transform.position - beanCollectorPosition;
                    direction.Normalize();

                    FlipSprite(direction.x);

                    transform.Translate(direction * beanBehaviour.moveSpeed * Time.deltaTime);

                    Collider2D collectorCollider = GetComponent<Collider2D>();

                    if (collectorCollider.bounds.Intersects(closestBean.GetComponent<Collider2D>().bounds))
                    {
                        PickUpBean(closestBean);
                    }
                }

                else
                {
                    beanBehaviour.MoveToStartArea();
                }
            }
        }

        if (isCarryingBean)
        {
            Vector3 beanCollectorPosition = transform.position;
            Vector3 dropOffPointPosition = dropOffPoint.position;

            Vector3 direction = dropOffPointPosition - beanCollectorPosition;
            direction.Normalize();

            Vector3 newPosition = Vector3.MoveTowards(beanCollectorPosition, dropOffPointPosition, beanBehaviour.moveSpeed * Time.deltaTime);

            transform.position = newPosition;  
            
            if (Vector3.Distance(beanCollectorPosition, dropOffPointPosition) < 0.1f)
            {
                DropOffBean();
            }
        }
    }

    private IEnumerator BeanUpdater()
    {

        yield return new WaitForSeconds(3);

        nakedBeans.Clear();
        beansAddedToList = false;
        CollectorCoroutines();

    }

    private void PickUpBean(GameObject bean)
    {
        Transform holdPosition = transform.Find("CarryingTransform");
        bean.transform.SetParent(holdPosition);
        bean.transform.localPosition = Vector3.zero;
        carryingBean = bean.transform;
        bean.GetComponent<NakedBean>().isBeingCarried = true;
        isCarryingBean = true;
    }

    private void DropOffBean()
    {
        Transform carryingTransform = transform.Find("CarryingTransform");

        if (carryingTransform != null)
        {
            Transform nakedBeanTransform = carryingTransform.Find("Naked Bean");
           
            if (nakedBeanTransform != null)
            {
                GameObject carriedBean = nakedBeanTransform.gameObject;
                Resources resourceScript = beanBehaviour.gameController.GetComponent<Resources>();

                if (resourceScript != null)
                {
                    if (gameObject.tag == "Friendly") resourceScript.AddBeanToPlayerResource();
                    if (gameObject.tag == "Enemy") resourceScript.AddBeanToEnemyResource();
                    Destroy(carriedBean);
                    carryingBean = null;
                    isCarryingBean = false;

                    FlipSprite(dropOffPoint.position.x - transform.position.x);
                }
            }
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