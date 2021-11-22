using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;
using UnityEngine.XR.ARFoundation;

public class BlockManager : Singleton<BlockManager>
{
    public Player player; //player����
    public List<CommandPattern> PairedList = new List<CommandPattern>();    // ������ ���� list
    public List<Command> CommandList = new List<Command>(); //Inspecter â���� Ŀ�ǵ� ����Ʈ ��������
    public Stack<Action> ActionStack = new Stack<Action>();
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
    private Vector3 InitialPosition;

    public ARTrackedImageManager m_TrackedImageManager;

    public IEnumerator PlayCommands() //��� Coroutine
    {
        Playing = true;
        while (CommandPointer<CommandList.Count)
        {
            ActionStack.Push(player.Excute(CommandList[CommandPointer]));
            CommandPointer++;
            yield return new WaitForSeconds(CommandDuration);
        }
        Playing = false;

        yield return 0;
    }
    public IEnumerator StepForwardCommands() //�� ���� ��� Coroutine
    {
        if (CommandPointer < CommandList.Count)
        {
            Playing = true;
            ActionStack.Push(player.Excute(CommandList[CommandPointer]));
            CommandPointer++;
            yield return new WaitForSeconds(CommandDuration);
            Playing = false;
            yield return 0;
        }
    }
    public IEnumerator StepBackwardCommands() //�� ���� ����� Coroutine
    {
        if (CommandPointer > 0)
        {
            Action PoppedAction = ActionStack.Pop();
            --CommandPointer;
            if (PoppedAction == Action.MoveForward || PoppedAction == Action.MoveBackward || PoppedAction == Action.MoveLeft || PoppedAction == Action.MoveRight)
            {
                Playing = true;
                player.ReverseExcute(CommandList[CommandPointer]);
                yield return new WaitForSeconds(CommandDuration);
                Playing = false;
            }
            yield return 0;
        }
    }
    public void StopCommands() //����
    {
        StopAllCoroutines();
        CommandPointer = 0;
        Playing = false;
        StartCoroutine(ResetPosition());
    }

    public void PauseCommands() //�Ͻ�����
    {
        StopAllCoroutines();
        Playing = false;
    }
   
    public IEnumerator ResetPosition()
    {
        yield return new WaitUntil(() => !DOTween.IsTweening(player.transform));
        player.transform.position = InitialPosition; //ó����ġ��
    }


    void Start()
    {
        InitialPosition = player.transform.position;//���� ��ġ ����
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



    void OnEnable() => m_TrackedImageManager.trackedImagesChanged += OnChanged;

    void OnDisable() => m_TrackedImageManager.trackedImagesChanged -= OnChanged;

    void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {

        foreach (var newImage in eventArgs.added)
        {
            var name = newImage.referenceImage.name.ToLower();
            if (name.Contains("forward"))
            {
                PairedList.Add(new CommandPattern((int)Command.MoveForward, Vector3.Distance(new Vector3(0, 0, 0), newImage.transform.position)));
            }
            else if (name.Contains("backward"))
            {
                PairedList.Add(new CommandPattern((int)Command.MoveBackward, Vector3.Distance(new Vector3(0, 0, 0), newImage.transform.position)));
            }
            else if (name.Contains("right"))
            {
                PairedList.Add(new CommandPattern((int)Command.MoveRight, Vector3.Distance(new Vector3(0, 0, 0), newImage.transform.position)));
            }
            else if (name.Contains("left")){
                PairedList.Add(new CommandPattern((int)Command.MoveLeft, Vector3.Distance(new Vector3(0, 0, 0), newImage.transform.position)));
            }

        }
/* ���� ������*/
/*          PairedList.Sort(delegate CommandPattern (CommandPattern p1, CommandPattern p2) => { return p1.distance > p2.distance});
*//*        Delegate �������� �ۼ��� ���� �Դϴ�.*/
/*        PairedList.Sort( (double value) => { })
*/
        foreach (var updatedImage in eventArgs.updated)
        {
            // Handle updated event
        }

        foreach (var removedImage in eventArgs.removed)
        {
            // Handle removed event
        }
    }

}
