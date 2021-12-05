using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Command //명령 Enum
{
    MoveLeft, MoveRight, MoveForward, MoveBackward, None
}


// ARTargetImage에서 command 읽어들이고 정렬하기 위함
public class CommandNode
{
    public Command command; // 나중에 정렬되었을 때 다시 첫번째 값들만 추려서(Command)로 변환한다.
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
