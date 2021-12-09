using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BlockArea : MonoBehaviour
{
    public GameObject block;
    public Queue<GameObject> BlockQueue = new Queue<GameObject>();
    public void EnqueueBlock(Command command)
    {
        GameObject instance = Instantiate(block);
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
        Destroy(BlockQueue.Dequeue());
    }
}