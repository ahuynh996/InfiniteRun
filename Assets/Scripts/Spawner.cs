using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject blockObstacle;
    public float respawnTime = 1.0f;
    private Vector2 screenBounds;
    private float minY = -5.0f;
    private float maxY = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        StartCoroutine(blockWave());
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    private void spawnBlocks()
    {
        GameObject blockSpawnPosition = Instantiate(blockObstacle) as GameObject;

        blockSpawnPosition.transform.position = new Vector3(transform.position.x, Random.Range(minY, maxY), transform.position.z);
    }

    IEnumerator blockWave()
    {
        while (true)
        {
            yield return new WaitForSeconds(respawnTime);
            spawnBlocks();
        }
        
    }
}
