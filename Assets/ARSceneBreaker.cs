using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ARSceneBreaker : MonoBehaviour
{
    public string scene;
    private void OnDestroy()
    {
        SceneManager.LoadScene(scene);
    }
}
