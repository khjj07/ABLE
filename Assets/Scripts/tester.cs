using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
public class tester : MonoBehaviour
{
    // Start is called before the first frame update
    private Player player;
    void Start()
    {
        player = GetComponent<Player>();
        this.UpdateAsObservable()
            .Where(_ => Input.GetKey(KeyCode.W))
            .Subscribe(_ => player.Excute(Command.MoveForward))
            .AddTo(gameObject);
        this.UpdateAsObservable()
           .Where(_ => Input.GetKey(KeyCode.S))
           .Subscribe(_ => player.Excute(Command.MoveBackward))
           .AddTo(gameObject);
        this.UpdateAsObservable()
           .Where(_ => Input.GetKey(KeyCode.A))
           .Subscribe(_ => player.Excute(Command.MoveLeft))
           .AddTo(gameObject);
        this.UpdateAsObservable()
           .Where(_ => Input.GetKey(KeyCode.D))
           .Subscribe(_ => player.Excute(Command.MoveRight))
           .AddTo(gameObject);

    }
}
