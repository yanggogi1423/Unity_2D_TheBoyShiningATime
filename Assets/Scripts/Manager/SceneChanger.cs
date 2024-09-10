using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void GoToMain()
    {
        SceneManager.LoadScene("Main");
        Debug.Log("Scene Changed \"Main\"");
    }
    
    public void GoToScene1()
    {
        SceneManager.LoadScene("Scene_1");
        Debug.Log("Scene Changed \"Scene_1\"");
    }
    
    /*
     * Scene 1 : 내 방
     */
}
