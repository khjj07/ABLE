using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public void DeleteChilds()
    {
        // child 에는 부모와 자식이 함께 설정 된다.
        var child = this.GetComponentsInChildren<Transform>();

        foreach (var iter in child)
        {
            // 부모(this.gameObject)는 삭제 하지 않기 위한 처리
            if (iter != this.transform)
            {
                Destroy(iter.gameObject);
            }
        }
    }
    public void DestoryObject()
    {
        DeleteChilds();
        Destroy(gameObject);
    }
}
