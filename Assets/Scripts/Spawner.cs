using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject blockObstacle;
    public float respawnTime = 1.0f;
    private Vector2 screenBounds;

    // Start is called before the first frame update
    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        StartCoroutine(blockWave());
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x < screenBounds.x * 2)
        {
            
        }
    }

    private void spawnBlocks()
    {
        GameObject a = Instantiate(blockObstacle) as GameObject;
        a.transform.position = new Vector2(screenBounds.x * -2, Random.Range(-screenBounds.y, screenBounds.y));
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
