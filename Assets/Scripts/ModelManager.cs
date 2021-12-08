using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelManager : MonoBehaviour
{
    public Character type=Character.Cat;
    private Character[] characters = { Character.Cat, Character.Chicken , Character.Dog , Character.Lion , Character.Penguin };
    private int flag = 0;
    private void Awake() 
    {
        DontDestroyOnLoad(gameObject);
    }
    public void NextType()
    {
        flag++;
        if(flag==characters.Length)
        {
            flag = 0;
        }
        type=characters[flag];
    }
    public void PreviousType()
    {
        flag--;
        if (flag < 0)
        {
            flag = characters.Length-1;
        }
        type = characters[flag];
    }
}
