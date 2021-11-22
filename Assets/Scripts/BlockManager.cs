using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;
using UnityEngine.XR.ARFoundation;

public class BlockManager : Singleton<BlockManager>
{
    public Player player; //player참조
    public List<Command> CommandList = new List<Command>(); //Inspecter 창에서 커맨드 리스트 지정가능
    public Stack<Action> ActionStack = new Stack<Action>();
    public float ActDuration = 1f; //명령 Duration
    public int CommandPointer = 0; //명령 배열 포인터

    //상호작용 키 목록
    public KeyCode Play=KeyCode.Alpha1;
    public KeyCode StepForward = KeyCode.Alpha2;
    public KeyCode StepBackward = KeyCode.Alpha3;
    public KeyCode Stop = KeyCode.Alpha4;
    public KeyCode Pause = KeyCode.Alpha5;
    //상호작용 키 목록
    private bool Playing=false; //중복 실행 방지
    private Vector3 InitialPosition;

    public ARTrackedImageManager m_TrackedImageManager;

    public IEnumerator PlayCommands() //재생 Coroutine
    {
        Playing = true;
        while (CommandPointer<CommandList.Count)
        {
            ActionStack.Push(player.Excute(CommandList[CommandPointer]));
            CommandPointer++;
            yield return new WaitForSeconds(ActDuration);
        }
        Playing = false;

        yield return 0;
    }
    public IEnumerator StepForwardCommands() //한 블럭씩 재생 Coroutine
    {
        if (CommandPointer < CommandList.Count)
        {
            Playing = true;
            ActionStack.Push(player.Excute(CommandList[CommandPointer]));
            CommandPointer++;
            yield return new WaitForSeconds(ActDuration);
            Playing = false;
            yield return 0;
        }
    }
    public IEnumerator StepBackwardCommands() //한 블럭씩 역재생 Coroutine
    {
        if (CommandPointer > 0)
        {
            Action PoppedAction = ActionStack.Pop();
            --CommandPointer;
            if (PoppedAction == Action.MoveForward || PoppedAction == Action.MoveBackward || PoppedAction == Action.MoveLeft || PoppedAction == Action.MoveRight)
            {
                Playing = true;
                player.ReverseExcute(CommandList[CommandPointer]);
                yield return new WaitForSeconds(ActDuration);
                Playing = false;
            }
            yield return 0;
        }
    }
    public void StopCommands() //정지
    {
        StopAllCoroutines();
        CommandPointer = 0;
        Playing = false;
        StartCoroutine(ResetPosition());
    }

    public void PauseCommands() //일시정지
    {
        StopAllCoroutines();
        Playing = false;
    }
   
    public IEnumerator ResetPosition()
    {
        yield return new WaitUntil(() => !DOTween.IsTweening(player.transform));
        player.transform.position = InitialPosition; //처음위치로
    }


    void Start()
    {
        InitialPosition = player.transform.position;//최초 위치 저장
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



    void OnEnable() => m_TrackedImageManager.trackedImagesChanged += OnChanged;

    void OnDisable() => m_TrackedImageManager.trackedImagesChanged -= OnChanged;

    void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var newImage in eventArgs.added)
        {
            if (newImage.referenceImage.name.Contains("Block"))
            {

            }

        }

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
