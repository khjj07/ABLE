using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Command //��� Enum
{
    MoveLeft, MoveRight, MoveForward, MoveBackward, None
}


// ARTargetImage���� command �о���̰� �����ϱ� ����
public class CommandNode
{
    public Command command; // ���߿� ���ĵǾ��� �� �ٽ� ù��° ���鸸 �߷���(Command)�� ��ȯ�Ѵ�.
    public float distance;
    public GameObject marker;
    public CommandNode(Command command, float distance, GameObject marker)
    {
        this.command = command;
        this.distance = distance;
        this.marker = marker;
    }
    public CommandNode Compile(Mesh mesh)
    {
        marker.GetComponent<MeshFilter>().mesh = mesh;
        return this;
    }

    public CommandNode Decompile(Mesh mesh)
    {
        marker.GetComponent<MeshFilter>().mesh = mesh;
        return this;
    }

}
