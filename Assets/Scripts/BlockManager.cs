using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
public class BlockManager : Singleton<BlockManager>
{
    public Player player; //player참조
    public List<Command> CommandList = new List<Command>(); //Inspecter 창에서 커맨드 리스트 지정가능
    public float CommandDuration = 1f; //명령 Duration
    public int CommandPointer = 0; //명령 배열 포인터

    //상호작용 키 목록
    public KeyCode Play=KeyCode.Alpha1;
    public KeyCode StepForward = KeyCode.Alpha2;
    public KeyCode StepBackward = KeyCode.Alpha3;
    public KeyCode Stop = KeyCode.Alpha4;
    public KeyCode Pause = KeyCode.Alpha5;
    //상호작용 키 목록
    private bool Playing=false; //중복 실행 방지


    public IEnumerator PlayCommands() //재생 Coroutine
    {
        Playing = true;
        while (CommandPointer<CommandList.Count)
        {
            player.Excute(CommandList[CommandPointer]);
            CommandPointer++;
            yield return new WaitForSeconds(CommandDuration);
        }
        Playing = false;
        yield return 0;
    }
    public IEnumerator StepForwardCommands() //한 블럭씩 재생 Coroutine
    {
        Playing = true;
        player.Excute(CommandList[CommandPointer]);
        CommandPointer++;
        yield return new WaitForSeconds(CommandDuration);
        Playing = false;
        yield return 0;
    }
    public IEnumerator StepBackwardCommands() //한 블럭씩 역재생 Coroutine
    {
        Playing = true;
        --CommandPointer;
        player.ReverseExcute(CommandList[CommandPointer]);
        yield return new WaitForSeconds(CommandDuration);
        Playing = false;
        yield return 0;
    }
    public void StopCommands() //정지
    {
        StopAllCoroutines();
        CommandPointer = 0;
        Playing = false;
    }
    public void PauseCommands() //일시정지
    {
        StopAllCoroutines();
        Playing = false;
    }


    void Start()
    {
        //Input에 따른 재생 스트림
        this.UpdateAsObservable()
           .Where(_ => Input.GetKeyDown(Play) && !Playing)
           .Subscribe(_ => StartCoroutine(PlayCommands()))
           .AddTo(gameObject); //Play

        this.UpdateAsObservable()
          .Where(_ => Input.GetKeyDown(StepForward) && !Playing)
          .Subscribe(_ => StartCoroutine(StepForwardCommands()))
          .AddTo(gameObject); //SteoForward

        this.UpdateAsObservable()
        .Where(_ => Input.GetKeyDown(StepBackward) && !Playing)
        .Subscribe(_ => StartCoroutine(StepBackwardCommands()))
        .AddTo(gameObject); //SteoBackward

        this.UpdateAsObservable()
       .Where(_ => Input.GetKeyDown(Stop))
       .Subscribe(_ => StopCommands())
       .AddTo(gameObject); //Stop

        this.UpdateAsObservable()
       .Where(_ => Input.GetKeyDown(Pause))
       .Subscribe(_ => PauseCommands())
       .AddTo(gameObject); //Pause

        //Input에 따른 재생 스트림
    }
}
