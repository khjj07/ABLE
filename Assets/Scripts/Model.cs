using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Model : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Character type = ModelManager.instance.type;
        if (type==Character.Cat)
        {
            transform.Find("Cat").gameObject.SetActive(true);
        }
        else if (type == Character.Chicken)
        {
            transform.Find("Chicken").gameObject.SetActive(true);
        }
        else if (type == Character.Dog)
        {
            transform.Find("Dog").gameObject.SetActive(true);
        }
        else if (type == Character.Lion)
        {
            transform.Find("Lion").gameObject.SetActive(true);
        }
        else if (type == Character.Penguin)
        {
            transform.Find("Penguin").gameObject.SetActive(true);
        }
        
        transform.DORotate(new Vector3(0, 360, 0), 2f, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.Linear);
    }
}
