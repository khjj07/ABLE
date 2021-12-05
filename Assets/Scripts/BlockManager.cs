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
    public List<CommandNode> NodeList = new List<CommandNode>();   // 정렬을 위한 list
    public List<CommandNode> CompiledList = new List<CommandNode>();
    public List<GameObject> MarkerList = new List<GameObject>();
    public GameObject Marker;
    public Stack<Action> ActionStack = new Stack<Action>();
    public float ActDuration = 1f; //명령 Duration
    public int NodePointer = 0; //명령 배열 포인터
    public bool mark = true;
    //상호작용 키 목록
    public KeyCode Play = KeyCode.Alpha1;
    public KeyCode StepForward = KeyCode.Alpha2;
    public KeyCode StepBackward = KeyCode.Alpha3;
    public KeyCode Stop = KeyCode.Alpha4;
    public KeyCode Pause = KeyCode.Alpha5;
    //상호작용 키 목록
    private bool Playing = false; //중복 실행 방지
    public Mesh CheckingMesh;
    public Mesh CompiledMesh;
    public bool Compiled = false;
    private Vector3 InitialPosition;
    public Camera camera;
    public ARTrackedImageManager m_TrackedImageManager;

    public void play()
    {
        if(!Playing)
            StartCoroutine(PlayCommands());
    }

    public void ChangeCompileState()
    {
       
        if (Compiled)
        {
            for (int i = 0; i < CompiledList.Count; i++)
            {
                CompiledList.Remove(CompiledList[i].Decompile(CheckingMesh));
            }
            for (int i = 0; i < NodeList.Count; i++)
            {
                NodeList[i].Decompile(CheckingMesh);
            }
            Compiled = false;
            NodePointer = 0;
        }
        else
        {
            string text = "";
            for (int i = 0; i < NodeList.Count; i++)
            {
                CompiledList.Add(NodeList[i].Compile(CompiledMesh));
                if (CompiledList[i].command == Command.MoveBackward)
                    text = text + "B ";
                else if (CompiledList[i].command == Command.MoveForward)
                    text = text + "F ";
                else if (CompiledList[i].command == Command.MoveLeft)
                    text = text + "L ";
                else if (CompiledList[i].command == Command.MoveRight)
                    text = text + "R ";
            }
            Compiled = true;
            Debug.Log(text);
        }
    }
    public void ChangeMarkState(bool m)
    {
        mark = m;
        if(!m)
        {
            for (int i = 0; i < NodeList.Count; i++)
            {
                NodeList[i].marker.SetActive(false);
                
            }
            NodeList.Clear();
        }
        
    }


    public IEnumerator PlayCommands() //재생 Coroutine
    {
        Playing = true;
        while (NodePointer < CompiledList.Count)
        {
            ActionStack.Push(player.Excute(CompiledList[NodePointer].command));
            NodePointer++;
            yield return new WaitForSeconds(ActDuration);
        }
        Playing = false;

        yield return 0;
    }
    public IEnumerator StepForwardCommands() //한 블럭씩 재생 Coroutine
    {
        if (NodePointer < NodeList.Count)
        {
            Playing = true;
            ActionStack.Push(player.Excute(NodeList[NodePointer].command));
            NodePointer++;
            yield return new WaitForSeconds(ActDuration);
            Playing = false;
            yield return 0;
        }
    }
    public IEnumerator StepBackwardCommands() //한 블럭씩 역재생 Coroutine
    {
        if (NodePointer > 0)
        {
            Action PoppedAction = ActionStack.Pop();
            --NodePointer;
            if (PoppedAction == Action.MoveForward || PoppedAction == Action.MoveBackward || PoppedAction == Action.MoveLeft || PoppedAction == Action.MoveRight)
            {
                Playing = true;
                player.ReverseExcute(NodeList[NodePointer].command);
                yield return new WaitForSeconds(ActDuration);
                Playing = false;
            }
            yield return 0;
        }
    }
    public void StopCommands() //정지
    {
        StopAllCoroutines();
        NodePointer = 0;
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
        //테스트 키에 따른 재생 스트림
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

        //테스트 키에 따른 재생 스트림
    }
    private Command ParseCommand(string name)
    {
        if (name.Contains("forward"))
        {
            return Command.MoveForward;
        }
        else if (name.Contains("backward"))
        {
            return Command.MoveBackward;
        }
        else if (name.Contains("right"))
        {
            return Command.MoveRight;
        }
        else if (name.Contains("left"))
        {
            return Command.MoveLeft;
        }
        return Command.None;
    }
    public void AddNode(ARTrackedImage newImage)
    {
        var name = newImage.referenceImage.name.ToLower();
        if(name.Contains("block"))
        {
            Command command = ParseCommand(name);
            Debug.LogWarning(name);
            Debug.LogWarning((int)command);
            Vector3 CameraPosition = camera.gameObject.transform.position;
            Vector3 ImagePosition = newImage.transform.position;
            GameObject new_marker = Instantiate(Marker);
            CommandNode new_node = new CommandNode(command, Vector3.Distance(CameraPosition, ImagePosition), new_marker);

            NodeList.Add(new_node);

            this.UpdateAsObservable()
                .Where(_ => UpdateNode(newImage, new_node))
                .Subscribe(_ => StartCoroutine(ChangeNodeState(newImage, new_node)))
                .AddTo(gameObject); //노드 업데이트 스트림
        }
    }



    private IEnumerator ChangeNodeState(ARTrackedImage img, CommandNode node)
    {
        if (mark && img.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking && !(node.marker.activeSelf))
        {
            node.marker.SetActive(true);
            NodeList.Add(node);
        }
        yield return new WaitForSeconds(1f);
        if (!mark || (img.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Limited && node.marker.activeSelf))
        {
            node.marker.SetActive(false);
            NodeList.Remove(node);
        }
         
        yield return null;
    }

    private bool UpdateNode(ARTrackedImage img, CommandNode node)
    {
        Vector3 CameraPosition = camera.gameObject.transform.position;
        Vector3 ImagePosition = img.transform.position;
        node.marker.transform.position = ImagePosition;
        node.distance = Vector3.Distance(CameraPosition, ImagePosition);
        

        if (img.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Limited && node.marker.activeSelf)
        {
            return true;
        }
        else if (img.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking && !(node.marker.activeSelf))
        {
            return true;
        }

        return false;
    }

    void OnEnable() => m_TrackedImageManager.trackedImagesChanged += OnChanged;

    void OnDisable() => m_TrackedImageManager.trackedImagesChanged -= OnChanged;

    void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var newImage in eventArgs.added)
        {
            AddNode(newImage);
        }
    }

}
