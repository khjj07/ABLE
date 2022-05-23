using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{

    public GameObject needed;
    public void Change()
    {
        if (ModelManager.instance.modelInstance && ModelManager.instance.modelInstance.activeSelf == true)
        {
            GameStateManager.instance.Next();
        }
        else
            StartCoroutine(alert());
    }
    public IEnumerator alert()
    {
        needed.SetActive(true);
        yield return new WaitForSeconds(1f);
        needed.SetActive(false);
        yield return null;
    }
}
