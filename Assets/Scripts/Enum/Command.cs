using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum  Command //��� Enum
{
    MoveLeft, MoveRight, MoveForward, MoveBackward,
}


// ARTargetImage���� command �о���̰� �����ϱ� ����
public class CommandPattern
{
    public int command; // ���߿� ���ĵǾ��� �� �ٽ� ù��° ���鸸 �߷���(Command)�� ��ȯ�Ѵ�.
    public int distance;

    public CommandPattern(Command command, int distance)
    {
        this.command =  (int)command;
        this.distance = distance;
    }

}
