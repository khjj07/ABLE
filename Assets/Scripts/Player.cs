using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx.Triggers;
using UniRx;

public class Player : MonoBehaviour
{
    public float MoveOffset = 1f;//������ Offset
    public float MoveDuration = 0.5f;//������ Duration
    public float RayDistinctRange = 1f; //��ֹ� ���� Ray �Ÿ�
    private bool MovingFlag = false; //Tween �ߺ� ����
    public Animator animator;
    void Start()
    {
        Character character = GameObject.Find("Models").GetComponent<ModelManager>().type;
        GameObject newCharacter;
        if(character == Character.Cat)
        {
            newCharacter = transform.Find("Cat").gameObject;
            newCharacter.SetActive(true);
            animator=newCharacter.GetComponent<Animator>();
        }
        else if(character == Character.Chicken)
        {
            newCharacter = transform.Find("Chicken").gameObject;
            newCharacter.SetActive(true);
            animator = newCharacter.GetComponent<Animator>();
        }
        else if (character == Character.Dog)
        {
            newCharacter = transform.Find("Dog").gameObject;
            newCharacter.SetActive(true);
            animator=newCharacter.GetComponent<Animator>();
        }
        else if (character == Character.Lion)
        {
            newCharacter = transform.Find("Lion").gameObject;
            newCharacter.SetActive(true);
            animator=newCharacter.GetComponent<Animator>();
        }
        else if (character == Character.Penguin)
        {
            newCharacter = transform.Find("Penguin").gameObject;
            newCharacter.SetActive(true);
            animator = newCharacter.GetComponent<Animator>();
        }
        Destroy(GameObject.Find("Models"));
        GameObject.Find("Play Scene").SetActive(false);
        float CommandDuration = BlockManager.instance.ActDuration;
        MoveDuration = CommandDuration > MoveDuration ? MoveDuration : CommandDuration;
        //MoveDuration�� CommandDuration���� ���� ��� CommandDuration���� ����
    }
    private void Update()
    {
        //Debug.Log(transform.position);
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
    private void RotateToDirection(Vector3 direction)
    {
        if (direction == Vector3.forward)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        if (direction == Vector3.back)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        if (direction == Vector3.left)
            transform.rotation = Quaternion.Euler(0, -90, 0);
        if (direction == Vector3.right)
            transform.rotation = Quaternion.Euler(0, 90, 0);
    }
    private Action Move(Vector3 direction) // ������
    {
        
        if (!MovingFlag ) //Tween��� Ȯ�� �� ��ֹ� ���� üũ
        {
            RotateToDirection(direction);
            if (!CheckObstacle(direction))
            {
                MovingFlag = true;

                animator.SetInteger("Walk", 1);
                transform.DOMove(transform.position + direction * MoveOffset, MoveDuration)
                     .OnComplete(() => {
                         MovingFlag = false;
                     // Debug.Log(transform.position);
                     animator.SetInteger("Walk", 0);
                     }); // direction���� Offset��ŭ Duration���� �̵�
                return DirectionMove(direction);
            }
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
