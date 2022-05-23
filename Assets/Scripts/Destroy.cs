using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public void DeleteChilds()
    {
        // child ���� �θ�� �ڽ��� �Բ� ���� �ȴ�.
        var child = this.GetComponentsInChildren<Transform>();

        foreach (var iter in child)
        {
            // �θ�(this.gameObject)�� ���� ���� �ʱ� ���� ó��
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
