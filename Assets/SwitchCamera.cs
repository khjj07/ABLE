using Google.XR.ARCoreExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SwitchCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public ARSessionOrigin newOrigin;
    public void ChangeCamera()
    {
        transform.GetComponent<ARCoreExtensions>().SessionOrigin = newOrigin;
        Debug.Log("22222222222222");
    }

}
