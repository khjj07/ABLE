using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx.Triggers;
using UniRx;
public class Player : MonoBehaviour
{
    public float MoveOffset=1f;
    public float MoveDuration = 0.5f;
    public float RayDistinctRange = 1f;
    private bool MovingFlag = false;
    void Start()
    {
  
    }
    private void Move(Vector3 direction)
    {
        if(!MovingFlag && !CheckObstacle(direction))
        {
            MovingFlag = true;
            transform.DOMove(transform.position + direction * MoveOffset, MoveDuration)
                .OnComplete(() => { MovingFlag = false; });
        }
    }
    private bool CheckObstacle(Vector3 direction)
    {
        RaycastHit hit;
        bool result = Physics.Raycast(transform.position, direction, out hit, RayDistinctRange);
        //Debug.Log(Movable);
        if (result)
        {
            //Debug.Log("hit point : " + hit.point + ", distance : " + hit.distance + ", name : " + hit.collider.name);
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);
        }
        else
            Debug.DrawRay(transform.position, transform.forward * RayDistinctRange, Color.red);
        return result && hit.collider.CompareTag("Obstacle");
    }
    public void Excute(Command cmd)
    {
        if (cmd == Command.MoveForward)
            Move(Vector3.forward);
        else if (cmd == Command.MoveBackward)
            Move(Vector3.back);
        else if (cmd == Command.MoveLeft)
            Move(Vector3.left);
        else if (cmd == Command.MoveRight)
            Move(Vector3.right);
    }

}
