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
        float CommandDuration = BlockManager.instance.CommandDuration;
        MoveDuration = CommandDuration > MoveDuration ? MoveDuration : CommandDuration;
        //MoveDuration�� CommandDuration���� ���� ��� CommandDuration���� ����
    }
    private void Move(Vector3 direction) // ������
    {
        if(!MovingFlag && !CheckObstacle(direction)) //Tween��� Ȯ�� �� ��ֹ� ���� üũ
        {
            MovingFlag = true;
            transform.DOMove(transform.position + direction * MoveOffset, MoveDuration)
                .OnComplete(() => { MovingFlag = false; }); // direction���� Offset��ŭ Duration���� �̵�
        }
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

    public void Excute(Command cmd) //Ŀ�ǵ� ����
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
