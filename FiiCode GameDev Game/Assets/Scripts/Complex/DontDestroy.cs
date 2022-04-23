using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{
    private static DontDestroy _instance;
    public static DontDestroy Instance { get { return _instance; } }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            
        }
        else
        {
            _instance = this;
        }
    }
}
