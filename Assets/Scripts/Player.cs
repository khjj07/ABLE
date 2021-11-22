using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx.Triggers;
using UniRx;
public class Player : MonoBehaviour
{
    public float MoveOffset=1f;//������ Offset
    public float MoveDuration = 0.5f;//������ Duration
    public float RayDistinctRange = 1f; //��ֹ� ���� Ray �Ÿ�
    private bool MovingFlag = false; //Tween �ߺ� ����

    void Start()
    {
        float CommandDuration = BlockManager.instance.ActDuration;
        MoveDuration = CommandDuration > MoveDuration ? MoveDuration : CommandDuration;
        //MoveDuration�� CommandDuration���� ���� ��� CommandDuration���� ����
    }
    private Action DirectionBlocked(Vector3 direction)
    {
        if (direction == Vector3.left)
            return Action.LeftBlocked;
        else if (direction == Vector3.right)
            return Action.RightBlocked;
        else if (direction == Vector3.forward)
            return Action.ForwardBlocked;
        else if (direction == Vector3.back)
            return Action.BackwardBlocked;
        return Action.None;
    }
    private Action DirectionMove(Vector3 direction)
    {
        if (direction == Vector3.left)
            return Action.MoveLeft;
        else if (direction == Vector3.right)
            return Action.MoveRight;
        else if (direction == Vector3.forward)
            return Action.MoveForward;
        else if (direction == Vector3.back)
            return Action.MoveBackward;
        return Action.None;
    }
    private Action Move(Vector3 direction) // ������
    {
        if (!MovingFlag && !CheckObstacle(direction)) //Tween��� Ȯ�� �� ��ֹ� ���� üũ
        {
            MovingFlag = true;
           transform.DOMove(transform.position + direction * MoveOffset, MoveDuration)
                .OnComplete(() => { MovingFlag = false; }); // direction���� Offset��ŭ Duration���� �̵�
            return DirectionMove(direction);
        }
        return DirectionBlocked(direction);
    }

    private bool CheckObstacle(Vector3 direction) //RayCast�� ��ֹ� ����
    {
        RaycastHit hit;
        bool result = Physics.Raycast(transform.position, direction, out hit, RayDistinctRange); //RayDistinctRange �Ÿ� ��ŭ Ray���� ��ֹ� ����� True
        if (result)
        {
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red); 
        }
        else
            Debug.DrawRay(transform.position, transform.forward * RayDistinctRange, Color.red);
        return result && hit.collider.CompareTag("Obstacle"); //�±� ���ؼ� ��ȯ
    }

    public Action Excute(Command cmd) //Ŀ�ǵ� ����
    {
        if (cmd == Command.MoveForward)
          return Move(Vector3.forward);
        else if (cmd == Command.MoveBackward)
            return Move(Vector3.back);
        else if (cmd == Command.MoveLeft)
            return Move(Vector3.left);
        else if (cmd == Command.MoveRight)
            return Move(Vector3.right);
        return Action.None;
    }
    public void ReverseExcute(Command cmd) //Ŀ�ǵ� �ݴ�� ����
    {
        if (cmd == Command.MoveForward)
            Move(Vector3.back);
        else if (cmd == Command.MoveBackward)
            Move(Vector3.forward);
        else if (cmd == Command.MoveLeft)
            Move(Vector3.right);
        else if (cmd == Command.MoveRight)
            Move(Vector3.left);
    }

}
