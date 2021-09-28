using UnityEngine;
using System.Collections;


// 싱글톤 : 씬에서 하나의 객체로만 사용될경우 이클래쓰를 상속받아서 사용
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

    private static T _instance;
    public static T instance
    {
        get
        {
            if (_instance == null)
            {
                try
                {
                    _instance = (T)FindObjectOfType<T>();
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e.StackTrace);
                    return null;
                }
            }
            return _instance;
        }
    }
}
