using UnityEngine;

public class GroundTile : MonoBehaviour
{
    GroundSpawner groundSpawner;

    private void Start()
    {
        groundSpawner = GameObject.FindObjectOfType<GroundSpawner>();
        spawnObstacle();
        spawncoin();
    }

    private void OnTriggerExit(Collider other)
    {
        groundSpawner.SpawnTile();
        Destroy(gameObject, 2);
    }

    // Obstacle
    public GameObject obstaclePrefab; 
    void spawnObstacle()
    {
        // Choose a random point to spawn the obstacle
        int obstacleSpawnIndex = Random.Range(2, 5);
        Transform spawnPoint = transform.GetChild(obstacleSpawnIndex).transform;

        // Spawn obstacle at the position 
        Instantiate(obstaclePrefab, spawnPoint.position, Quaternion.identity, transform);
    }

    // Coin prefab
    public GameObject coinPrefab;
    void spawncoin()
    {
        int coinsToSpawn = 10;
        Collider tileCollider = GetComponent<Collider>();

        for (int i = 0; i < coinsToSpawn; i++)
        {
            Vector3 spawnPos = GetRandomPointInCollider(tileCollider);

            // ✅ New: Skip coin spawn if too close to an obstacle
            float checkRadius = 0.5f; // tweak this for spacing
            Collider[] nearby = Physics.OverlapSphere(spawnPos, checkRadius);
            bool tooCloseToObstacle = false;

            foreach (Collider col in nearby)
            {
                if (col.GetComponent<Obstacle>() != null)
                {
                    tooCloseToObstacle = true;
                    break;
                }
            }

            if (tooCloseToObstacle)
                continue; // skip this coin spawn

            GameObject temp = Instantiate(coinPrefab, spawnPos, coinPrefab.transform.rotation, transform);


        }
    }
    
    Vector3 GetRandomPointInCollider(Collider collider)
    {
        Vector3 point = new Vector3(
            Random.Range(collider.bounds.min.x, collider.bounds.max.x),
            Random.Range(collider.bounds.min.y, collider.bounds.max.y),
            Random.Range(collider.bounds.min.z, collider.bounds.max.z)
        );

        if (point != collider.ClosestPoint(point))
        {
            point = GetRandomPointInCollider(collider);
        }

        point.y = 1;
        return point;
    }
}
