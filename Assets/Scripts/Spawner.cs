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

    /*
     * 추가 예정 기능
     * 현재는 spawnPoints 전체에 스폰되는 로직임.
     * 하나의 맵에서 여러 스폰 포인트를 통제하려면 다른 로직을 도입해야 함.
     * 미래의 나야 부탁해~ 20241001
     */
    private void Start()
    {
        StartCoroutine(MonsterSpwanCoroutine());
        
        //  현재는 Monster Prefab을 수동으로 가져와야 함. 자동으로 폴더에서 Include하는 방법도 있지만 그건 나중에..
        if (monsterPrefabs.Length == 0)
        {
            Debug.LogError("monsterPrefabs가 비어있습니다. Prefab을 수동으로 넣어 주어야 합니다.");
            enabled = false;
        }
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
