using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private int maxSpawn;
    [SerializeField] private int curSpawn;
    [SerializeField] private float spawnRate;
    
    public GameObject[] monsterPrefabs;
    public Transform[] spawnPoints;

    private GameObject _latestSpawn;

    private void Start()
    {
        StartCoroutine(MonsterSpwanCoroutine());
    }

    private void SpawnMonster()
    {
        
        _latestSpawn = GetMonsterRandom();
        if (_latestSpawn != null)
        {
            Instantiate(_latestSpawn, GetPointRandom(), Quaternion.identity); 
            curSpawn++;
        }
        else
        {
            Debug.Log("Spawn object is NULL");
        }
    }

    private GameObject GetMonsterRandom()
    {
        var idx = Random.Range(0, monsterPrefabs.Length);
        return monsterPrefabs[idx];
    }

    private Vector3 GetPointRandom()
    {
        var idx = Random.Range(0, spawnPoints.Length);
        return spawnPoints[idx].position;
    }

    public void UpdateSpawn()
    {
        curSpawn--;
    }

    private IEnumerator MonsterSpwanCoroutine()
    {
        while (true)
        {
            if (curSpawn < maxSpawn)
            {
                SpawnMonster();
                Debug.Log("Spawn Monster");
            }
            yield return new WaitForSeconds(spawnRate);
        }
    }
}
