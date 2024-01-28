using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NakedBeanSpawner : MonoBehaviour
{
    private string beanName = "Naked Bean";
    public GameObject NakedBean;
    public Vector3 spawnPosition;

    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;


    private void Start()
    {
        BeanCycleSpawning();
    }

    private void BeanCycleSpawning()
    {
        StartCoroutine(NakedBeanSpawning());
    }

    private IEnumerator NakedBeanSpawning()
    {
        float lowEndSpawning =  Random.Range(5f, 20f);

        yield return new WaitForSeconds(lowEndSpawning);

        int amountOfNakedBeansToSpawn = Random.Range(5, 10);
        //Vector2 spawnAreaMin = new Vector2(-0.8f, 6f);
        //Vector2 spawnAreaMax = new Vector2(1.5f, 8f);

        for (int i = 0; i < amountOfNakedBeansToSpawn; i++)
        {
            Vector3 randomSpawnPosition = new Vector3(Random.Range(spawnAreaMin.x, spawnAreaMax.x), spawnPosition.y + Random.Range(spawnAreaMin.y, spawnAreaMax.y), spawnPosition.z);

            GameObject beanSpawned = Instantiate(NakedBean, randomSpawnPosition, transform.rotation);
            beanSpawned.name = beanName;

            StartCoroutine(FallBean(beanSpawned));
        }

        BeanCycleSpawning();
    }

    private IEnumerator FallBean(GameObject bean)
    {
        float fallSpeed = 5f;
        float stopY = Random.Range(-2.2f, -1.1f);

        while (bean.transform.position.y > stopY)
        {
            bean.transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
