using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
public class ModelManager : Singleton<ModelManager>
{
    public ARTrackedImageManager m_TrackedImageManager;
    public Character type=Character.None;
    public UnityEvent selectAvailable;
    public UnityEvent selectUnavailable;
    public GameObject modelPrefab;
    public GameObject modelInstance;
    public GameState titleScene;
    private ARTrackedImage image;

    private void Awake() 
    {
        DontDestroyOnLoad(gameObject);
    }

    public Character ParseName(string name)
    {
        GameObject newCharacter;

        if (name.Contains("cat"))
        {
            return Character.Cat;
        }
        else if (name.Contains("dog"))
        {
            return Character.Dog;
        }
        else if (name.Contains("penguin"))
        {
            return Character.Penguin;
        }
        else if (name.Contains("lion"))
        {
            return Character.Lion;
        }
        else if (name.Contains("chicken"))
        {
            return Character.Chicken;
        }
        else
        {
            return Character.None;
        }
    }
  
    public bool UpdateModel(ARTrackedImage img, GameObject model)
    {
        if(model)
        {
            Vector3 ImagePosition = img.transform.position;
            model.transform.position = ImagePosition;

            if (img.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Limited && model.activeSelf)
            {
                return true;
            }
            else if (img.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking && !(model.activeSelf))
            {
                return true;
            }
        }
        return false;
    }

    public void DestroyModel()
    {
        Destroy(modelInstance);
        modelInstance = null;
    }

    public IEnumerator ChangeModelState(ARTrackedImage img, GameObject model)
    {
        if (model && img.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
        {
            model.SetActive(true);
            selectAvailable.Invoke();
        }
        yield return new WaitForSeconds(1f);
        if (!model || (img.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Limited))
        {
            model.SetActive(false);
            selectUnavailable.Invoke();
        }
        yield return new WaitForSeconds(1f);
        if (img.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Limited)
        {
            Destroy(model);
            modelInstance = null;
            selectUnavailable.Invoke();
        }

        yield return null;
    }

    public void CharacterChecking(ARTrackedImage newImage)
    {
        var name = newImage.referenceImage.name.ToLower();
        if(!modelInstance)
        {
            selectAvailable.Invoke();
            modelInstance = Instantiate(modelPrefab);
            this.UpdateAsObservable()
                     .Where(_ => UpdateModel(newImage, modelInstance))
                     .Subscribe(_ => StartCoroutine(ChangeModelState(newImage, modelInstance)))
                     .AddTo(modelInstance); //��� ������Ʈ ��Ʈ��
        }
       
        type = ParseName(name); 
    }


    void OnEnable() => m_TrackedImageManager.trackedImagesChanged += OnChanged;

    void OnDisable() => m_TrackedImageManager.trackedImagesChanged -= OnChanged;

    void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var newImage in eventArgs.added)
        {
            if(titleScene.gameObject.activeSelf || GameStateManager.instance.currentState== titleScene )
                CharacterChecking(newImage);
        }
    }
}
