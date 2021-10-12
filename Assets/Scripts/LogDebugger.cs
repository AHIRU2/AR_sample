using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogDebugger : MonoBehaviour
{
    public static LogDebugger instance;

    [SerializeField]
    private Text txtDebug;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Displaylog(string message)
    {
        txtDebug.text += message + "¥n";
    }
}
