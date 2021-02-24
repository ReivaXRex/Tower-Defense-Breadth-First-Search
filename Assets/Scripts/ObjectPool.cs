using System.Collections;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] [Range(0.1f, 10f)] private float spawnTimer = 1f;
    [SerializeField] [Range(0, 50)] private int poolSize = 5;

    private GameObject[] pool;

    private void Awake()
    {
        PopulatePool();
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    private void PopulatePool()
    {
        pool = new GameObject[poolSize]; // Initialize the array, the array length being equal to the poolSize variable.

        for (int i = 0; i < pool.Length; i++) // Iterate through each element in the pool[] array.
        {
            pool[i] = Instantiate(enemyPrefab, transform); // Spawn an enemy for each element in the pool[].
            pool[i].SetActive(false);
        }
    }

    private void EnableObjectInPool()
    {
        for (int i = 0; i < pool.Length; i++)
        {
            if (pool[i].activeInHierarchy == false)
            {
                pool[i].SetActive(true);
                return;
            }
        }
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            EnableObjectInPool();
            yield return new WaitForSeconds(spawnTimer);
        }
    }
}