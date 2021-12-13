using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class BlockArea : MonoBehaviour
{
    public GameObject block;
    public Queue<GameObject> BlockQueue = new Queue<GameObject>();
  
    public void UpdateQueue(GameObject instance)
    {
        if(!BlockQueue.Contains(instance))
        {
            Destroy(instance);
        }
    }

    public void EnqueueBlock(Command command)
    {
        GameObject instance = Instantiate(block);

        this.UpdateAsObservable()
         .Subscribe(_ => UpdateQueue(instance))
         .AddTo(instance); //블록 삭제 스트림

        instance.transform.parent = this.transform;
        BlockQueue.Enqueue(instance);
        if (command == Command.MoveForward)
            instance.transform.Find("Text").GetComponent<Text>().text = "Forward";
        else if (command == Command.MoveBackward)
            instance.transform.Find("Text").GetComponent<Text>().text = "Backward";
        else if (command == Command.MoveLeft)
            instance.transform.Find("Text").GetComponent<Text>().text = "Left";
        else if (command == Command.MoveRight)
            instance.transform.Find("Text").GetComponent<Text>().text = "Right";
    }
    public void DequeueBlock()
    {
        BlockQueue.Dequeue();
    }
}