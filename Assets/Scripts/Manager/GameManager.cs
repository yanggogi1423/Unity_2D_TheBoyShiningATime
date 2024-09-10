using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region SINGLETON
    
    private static GameManager _instance;

    public static GameManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<GameManager>();
            if (_instance != null) return _instance;

            _instance = new GameManager().AddComponent<GameManager>();
            _instance.name = "GameManager";
        }

        return _instance;
    }
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        //  GM이 존재하지만 this가 아닌 경우 -> this를 삭제
        else if (_instance != this)
        {
            Destroy(this);
        }
    }
    #endregion
    
    
}
