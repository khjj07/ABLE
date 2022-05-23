using Google.XR.ARCoreExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SessionFinder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.GetComponent<ARCoreExtensions>().Session = GameObject.Find("AR Session").GetComponent<ARSession>();
    }

}
