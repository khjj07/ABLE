using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Triggers;
using UniRx;

public class Tester : MonoBehaviour
{
    // Start is called before the first frame update
    public Player player;
    void Start()
    {
        this.UpdateAsObservable()
            .Where(_ =>Input.GetKeyDown(KeyCode.W))
            .Subscribe(_ => player.Excute(Command.MoveForward))
            .AddTo(gameObject);
        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.S))
            .Subscribe(_ => player.Excute(Command.MoveBackward))
            .AddTo(gameObject);
        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.A))
            .Subscribe(_ => player.Excute(Command.MoveLeft))
            .AddTo(gameObject);
        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.D))
            .Subscribe(_ => player.Excute(Command.MoveRight))
            .AddTo(gameObject);

    }
}
