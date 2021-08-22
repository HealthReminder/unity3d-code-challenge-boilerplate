using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Building : MonoBehaviour
{
    public int sceneIndex;
    public void Enter()
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
