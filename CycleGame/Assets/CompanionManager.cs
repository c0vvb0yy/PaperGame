using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionManager : MonoBehaviour
{
    private static Vector3 savedPos;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SavePos()
    {
        savedPos = transform.position;
    }

    public void LoadPos()
    {
        if(savedPos != null)
            transform.position = savedPos;
    }
}
