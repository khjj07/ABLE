using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class CameraMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector2 PrevPoint;
    private float initTouchDist;
    public const float MaxZoom=20;
    public const float MinZoom = 120;
    public const float ZoomOffset = 1;
    public Vector2 CameraBorder = new Vector2(50,50);
    void Start()
    {
        this.UpdateAsObservable()
          .Where(_ => Input.touchCount==1 && Input.GetTouch(0).phase == TouchPhase.Began)
          .Subscribe(_ => { PrevPoint = Input.GetTouch(0).position; })
          .AddTo(gameObject);
        this.UpdateAsObservable()
        .Where(_ => Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved && InBorder())
        .Subscribe(_ => {
            gameObject.transform.Translate(-(Input.GetTouch(0).position.x - PrevPoint.x) * transform.GetComponent<Camera>().fieldOfView / 600,   0, -(Input.GetTouch(0).position.y - PrevPoint.y)* transform.GetComponent<Camera>().fieldOfView / 600, Space.World);
            PrevPoint = Input.GetTouch(0).position;
        })
        .AddTo(gameObject);
    }
    private void Update()
    {
        //Debug.Log(gameObject.transform.position);
        if (Input.touchCount > 1)
        {
            if(Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position) < initTouchDist && transform.GetComponent<Camera>().fieldOfView < MinZoom )
            {
                transform.GetComponent<Camera>().fieldOfView += ZoomOffset;
            }
            else if(Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position) > initTouchDist && transform.GetComponent<Camera>().fieldOfView > MaxZoom)
            {
                transform.GetComponent<Camera>().fieldOfView -= ZoomOffset;
            }
            initTouchDist = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
        }
        
    }
    private bool InBorder()
    {
        if(gameObject.transform.position.x > CameraBorder.x || gameObject.transform.position.x < -CameraBorder.x)
        {
            if(gameObject.transform.position.x < -CameraBorder.x)
            {
                gameObject.transform.position = new Vector3(-CameraBorder.x, transform.position.y, transform.position.z);
            }
            else
            {
                gameObject.transform.position = new Vector3(CameraBorder.x, transform.position.y, transform.position.z);
            }
            return false;
        }

        if (gameObject.transform.position.z > CameraBorder.y || gameObject.transform.position.z < -CameraBorder.y)
        {
            if (gameObject.transform.position.z < -CameraBorder.y)
            {
                gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, -CameraBorder.y);
            }
            else
            {
                gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, CameraBorder.y);
            }
            return false;
        }
        return true;
    }
}
