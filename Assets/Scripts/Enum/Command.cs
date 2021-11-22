using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum  Command //명령 Enum
{
    MoveLeft, MoveRight, MoveForward, MoveBackward,
}


// ARTargetImage에서 command 읽어들이고 정렬하기 위함
public class CommandPattern
{
    public int command; // 나중에 정렬되었을 때 다시 첫번째 값들만 추려서(Command)로 변환한다.
    public int distance;

    public CommandPattern(Command command, int distance)
    {
        this.command =  (int)command;
        this.distance = distance;
    }

}
