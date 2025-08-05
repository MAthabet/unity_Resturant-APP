using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject[] Foods;
    [SerializeField]
    float spawnIntervalRange = 1f;
    [SerializeField]
    float spawnIntervalMin = 0.5f;
    const float SPAWN_XMax = 9.5f;
    const float SPAWN_XMIN = -6f;
    float spawnY;

    private float nextSpawnTime;
    private void Start()
    {
        updateNextSpawnTime();
        spawnY = transform.position.y;
    }
    private void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnFood();
            updateNextSpawnTime();
        }
    }
    private void updateNextSpawnTime()
    {
        nextSpawnTime = Time.time + Random.Range(spawnIntervalMin, spawnIntervalMin + spawnIntervalRange);
    }

    private void SpawnFood()
    {
        int foodIndex = Random.Range(0, Foods.Length);
        Vector3 spawnPosition = new Vector3(Random.Range(SPAWN_XMIN,SPAWN_XMax), transform.position.y,0);

        Instantiate(Foods[foodIndex], spawnPosition, Quaternion.identity);
    }
}
