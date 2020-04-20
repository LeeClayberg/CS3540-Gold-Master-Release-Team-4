using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnTime = 3.0f;
    public float size = 5;
    public int firstPointIndex;

    GameObject target;


    // Start is called before the first frame update
    void Start()
    {
        if (!GameController.isFlyThrough) {
            target = GameObject.FindGameObjectWithTag("Player");
            InvokeRepeating("SpawnEnemies", spawnTime, spawnTime);
        }
    }

    // Update is called once per frame
    void SpawnEnemies()
    {
        Vector3 spawnerLocation = ClosestSpawnLocation();
        Vector3 enemyPosition;

        if (Vector3.Distance(spawnerLocation, target.transform.position) < 20 && !GameController.isGameOver) {
            enemyPosition.x = spawnerLocation.x + Random.Range(-size / 2, size / 2);
            enemyPosition.y = spawnerLocation.y;
            enemyPosition.z = spawnerLocation.z + Random.Range(-size / 2, size / 2);

            Instantiate(enemyPrefab, enemyPosition, transform.rotation);
        }
    }

    Vector3 ClosestSpawnLocation()
    {
        Vector3 closestPoint = Vector3.zero;
        float currentDistance = 100000;

        foreach(GameObject spawnPoint in GameObject.FindGameObjectsWithTag("SpawnPoint"))
        {
            float distance = Vector3.Distance(spawnPoint.transform.position, target.transform.position);
            if (distance < currentDistance)
            {
                currentDistance = distance;
                closestPoint = spawnPoint.transform.position;
                
            }
            
        }
        return closestPoint;
    }

    //Show the spawn region
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        foreach (GameObject spawnPoint in GameObject.FindGameObjectsWithTag("SpawnPoint"))
        {
            Gizmos.DrawWireCube(spawnPoint.transform.position, new Vector3(size, 3, size));
        }
    }
}
