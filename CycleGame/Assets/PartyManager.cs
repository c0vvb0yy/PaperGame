using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public GameObject Zaavan;
    public GameObject Anthrazit;
    private Vector3 ZaavPos;
    private Vector3 AnthraPos;
    
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void SavePos()
    {
        ZaavPos = Zaavan.transform.position;
        AnthraPos = Anthrazit.transform.position;
    }

    public void LoadPos()
    {
        if(ZaavPos != null)
        {
            Zaavan.transform.position = ZaavPos;
        }
        if(AnthraPos != null)
        {
            Anthrazit.transform.position = AnthraPos;
        }
    }

}