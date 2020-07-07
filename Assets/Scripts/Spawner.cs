using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject blockObstacle;
    public GameObject topBorder;
    public GameObject bottomBorder;
    public float respawnTime = 1.0f;
    public float borderRespawnTime;
    private Vector2 screenBounds;

    // Start is called before the first frame update
    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        StartCoroutine(blockWave());
        StartCoroutine(borderWave());

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

    private void spawnBorder()
    {
        GameObject top = Instantiate(topBorder) as GameObject;
        GameObject bottom = Instantiate(bottomBorder) as GameObject;

        top.transform.position = new Vector2(screenBounds.x * -2, screenBounds.y);
        bottom.transform.position = new Vector2(screenBounds.x * -2, -screenBounds.y);
    }

    IEnumerator blockWave()
    {
        while (true)
        {
            yield return new WaitForSeconds(respawnTime);
            spawnBlocks();
        }
        
    }
    
    IEnumerator borderWave()
    {
       while(true)
        {
            yield return new WaitForSeconds(borderRespawnTime);
            spawnBorder();

        }
    }
    

}
