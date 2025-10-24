using UnityEngine;
using System.Collections;

[System.Serializable]
public class SpawnableObject
{
    public GameObject prefab;           // Object to spawn
    public float minZ = -2f;            // Min Z position for this object
    public float maxZ = 2f;             // Max Z position for this object
    public float onY = 2f;             // Y position for this object
}

public class SpringSpawner : MonoBehaviour
{
    [Header("Spawn Set")]
    public SpawnableObject[] objectsToSpawn; // Array of different objects
    public Transform spawnOrigin;            // X/Y position for spawning (Z will be randomized per object)
    public float minSpawnTime = 1f;     // Min spawn interval for this object
    public float maxSpawnTime = 3f;     // Max spawn interval for this object

    void Start()
    {
        // Start a separate coroutine for each object type
        foreach (var obj in objectsToSpawn)
        {
            if (obj != null)
            {
                StartCoroutine(SpawnObjectLoop(obj));
            }
        }
    }

    IEnumerator SpawnObjectLoop(SpawnableObject obj)
    {
        while (true)
        {
            // Wait for a random interval specific to this object
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);

            // Pick a random Z position specific to this object
            float randomZ = Random.Range(obj.minZ, obj.maxZ);
            float yPosition = obj.onY;
            Vector3 spawnPos = new Vector3(spawnOrigin.position.x, spawnOrigin.position.y, randomZ);

            // Spawn the object
            if (obj != null)
            {
                Instantiate(obj.prefab, spawnPos, spawnOrigin.rotation);
            }
        }
    }
}
