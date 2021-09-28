using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Triggers;
using UniRx;

public class Tester : MonoBehaviour
{
    public Player player; //player참조
    //테스트 키 목록
    public KeyCode Forward= KeyCode.W;
    public KeyCode Backward= KeyCode.S;
    public KeyCode Left = KeyCode.A;
    public KeyCode Right = KeyCode.D;
    //테스트 키 목록
    void Start()
    {
        //Input에 따른 테스트 스트림
        this.UpdateAsObservable()
            .Where(_ =>Input.GetKeyDown(Forward))
            .Subscribe(_ => player.Excute(Command.MoveForward))
            .AddTo(gameObject); //Forward

        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(Backward))
            .Subscribe(_ => player.Excute(Command.MoveBackward))
            .AddTo(gameObject); //Backward

        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(Left))
            .Subscribe(_ => player.Excute(Command.MoveLeft))
            .AddTo(gameObject); //Left

        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(Right))
            .Subscribe(_ => player.Excute(Command.MoveRight))
            .AddTo(gameObject); //Right

        //Input에 따른 테스트 스트림

    }
}
