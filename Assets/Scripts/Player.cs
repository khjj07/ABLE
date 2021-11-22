using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx.Triggers;
using UniRx;
public class Player : MonoBehaviour
{
    public float MoveOffset=1f;//움직임 Offset
    public float MoveDuration = 0.5f;//움직임 Duration
    public float RayDistinctRange = 1f; //장애물 감지 Ray 거리
    private bool MovingFlag = false; //Tween 중복 방지

    void Start()
    {
        float CommandDuration = BlockManager.instance.ActDuration;
        MoveDuration = CommandDuration > MoveDuration ? MoveDuration : CommandDuration;
        //MoveDuration이 CommandDuration보다 작은 경우 CommandDuration으로 변경
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
    private Action Move(Vector3 direction) // 움직임
    {
        if (!MovingFlag && !CheckObstacle(direction)) //Tween재생 확인 및 장애물 유무 체크
        {
            MovingFlag = true;
           transform.DOMove(transform.position + direction * MoveOffset, MoveDuration)
                .OnComplete(() => { MovingFlag = false; }); // direction으로 Offset만큼 Duration동안 이동
            return DirectionMove(direction);
        }
        return DirectionBlocked(direction);
    }

    private bool CheckObstacle(Vector3 direction) //RayCast로 장애물 감지
    {
        RaycastHit hit;
        bool result = Physics.Raycast(transform.position, direction, out hit, RayDistinctRange); //RayDistinctRange 거리 만큼 Ray쏴서 장애물 존재시 True
        if (result)
        {
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red); 
        }
        else
            Debug.DrawRay(transform.position, transform.forward * RayDistinctRange, Color.red);
        return result && hit.collider.CompareTag("Obstacle"); //태그 비교해서 반환
    }

    public Action Excute(Command cmd) //커맨드 실행
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
    public void ReverseExcute(Command cmd) //커맨드 반대로 실행
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
