﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawnManager : MonoBehaviour
{
  public GameObject enemy;
  public Camera cam;
  public Transform player;
  public Tilemap tilemap;

  public int spawnDelay = 1;

  Vector3 worldMin;
  Vector3 worldMax;

  private void Start()
  {
    LoadBoundaries();
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.T))
    {
      SpawnEnemy();
    }
  }

  public void StartSpawning()
  {
    StartCoroutine(SpawnEnemies());
  }

  public void StopSpawning()
  {
    StopCoroutine(SpawnEnemies());
  }

  IEnumerator SpawnEnemies()
  {
    while (true)
    {
      SpawnEnemy();
      yield return new WaitForSeconds(spawnDelay);
    }
  }

  void SpawnEnemy()
  {
    float randomPosOnCamX = Random.Range(cam.ViewportToWorldPoint(new Vector3(0, 0)).x, cam.ViewportToWorldPoint(new Vector3(1, 0)).x);
    float randomPosOnCamY = Random.Range(cam.ViewportToWorldPoint(new Vector3(0, 0)).y, cam.ViewportToWorldPoint(new Vector3(1, 1)).y);
    Vector3 enemyPosition = new Vector3(randomPosOnCamX, randomPosOnCamY, 0f);
    Debug.Log(IsWithinBoundaries(enemyPosition));
    if ((enemyPosition - player.transform.position).magnitude >= 3 && IsWithinBoundaries(enemyPosition))
    {
      Instantiate(enemy, enemyPosition, Quaternion.identity);
    }
  }

  void LoadBoundaries()
  {
    tilemap.CompressBounds();
    worldMin = tilemap.transform.TransformPoint(tilemap.localBounds.min); // bottom left
    worldMax = tilemap.transform.TransformPoint(tilemap.localBounds.max); // top right
  }

  bool IsWithinBoundaries(Vector3 pos)
  {
    if (pos.x >= worldMin.x && pos.x <= worldMax.x)
    {
      if (pos.y >= worldMin.y && pos.y <= worldMax.y)
      {
        return true;
      }
    }
    return false;
  }
}
