using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TextButton : MonoBehaviour
{
    public Text textUI;

    private Queue<string> textQueue = new Queue<string>();
    public List<string> textList = new List<string>();
    public void Start()
    {
        for(int i=0;i<textList.Count;i++)
        {
            textQueue.Enqueue(textList[i]);
        }

        string new_queue = textQueue.Dequeue();
        textQueue.Enqueue(new_queue);
        textUI.text = new_queue;
    }
    public void NextText()
    {
        string new_queue = textQueue.Dequeue();
        textQueue.Enqueue(new_queue);
        textUI.text = new_queue;
    }
}
