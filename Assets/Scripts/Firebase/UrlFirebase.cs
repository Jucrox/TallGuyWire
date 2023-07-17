using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrlFirebase : MonoBehaviour
{
    public static UrlFirebase inst;
    private void Awake()
    {
        if (UrlFirebase.inst == null)
        {
            UrlFirebase.inst = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] public string url = null;

    private void Start()
    {
        if (url == null) Debug.LogError("URL Firebase empty");

        if (!url.EndsWith("/")) url = url + "/";
    }
}
