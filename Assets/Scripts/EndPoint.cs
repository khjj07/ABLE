using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EndPoint : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Clear;
    void Start()
    {
        Clear = GameObject.Find("Clear");
        Clear.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Destroy(gameObject, 1f);
            Debug.Log("Game Clear");
            Clear.SetActive(true);
        }
    }



}
