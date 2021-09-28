using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
public class BlockManager : Singleton<BlockManager>
{
    public Player player; //player����
    public List<Command> CommandList = new List<Command>(); //Inspecter â���� Ŀ�ǵ� ����Ʈ ��������
    public float CommandDuration = 1f; //��� Duration
    public int CommandPointer = 0; //��� �迭 ������

    //��ȣ�ۿ� Ű ���
    public KeyCode Play=KeyCode.Alpha1;
    public KeyCode StepForward = KeyCode.Alpha2;
    public KeyCode StepBackward = KeyCode.Alpha3;
    public KeyCode Stop = KeyCode.Alpha4;
    public KeyCode Pause = KeyCode.Alpha5;
    //��ȣ�ۿ� Ű ���
    private bool Playing=false; //�ߺ� ���� ����


    public IEnumerator PlayCommands() //��� Coroutine
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
    public IEnumerator StepForwardCommands() //�� ���� ��� Coroutine
    {
        Playing = true;
        player.Excute(CommandList[CommandPointer]);
        CommandPointer++;
        yield return new WaitForSeconds(CommandDuration);
        Playing = false;
        yield return 0;
    }
    public IEnumerator StepBackwardCommands() //�� ���� ����� Coroutine
    {
        Playing = true;
        --CommandPointer;
        player.ReverseExcute(CommandList[CommandPointer]);
        yield return new WaitForSeconds(CommandDuration);
        Playing = false;
        yield return 0;
    }
    public void StopCommands() //����
    {
        StopAllCoroutines();
        CommandPointer = 0;
        Playing = false;
    }
    public void PauseCommands() //�Ͻ�����
    {
        StopAllCoroutines();
        Playing = false;
    }


    void Start()
    {
        //Input�� ���� ��� ��Ʈ��
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

        //Input�� ���� ��� ��Ʈ��
    }
}
