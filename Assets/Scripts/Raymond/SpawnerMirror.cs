﻿using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerMirror : NetworkBehaviour
{
    [SerializeField]
    private GameObject blockObstacle;
    [SerializeField]
    private float respawnTime = 1.0f;
    [SerializeField]
    private Vector2 screenBounds;

    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        StartCoroutine(BlockWave());
    }

    private void SpawnBlocks() {
        GameObject obstacle = Instantiate(blockObstacle);
        obstacle.transform.position = new Vector2(screenBounds.x * -2, Random.Range(-screenBounds.y, screenBounds.y));
        NetworkServer.Spawn(obstacle);
    }

    IEnumerator BlockWave() {
        while (true) {
            yield return new WaitForSeconds(respawnTime);
            SpawnBlocks();
        }
    }

}