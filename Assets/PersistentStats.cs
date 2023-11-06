using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentStats : MonoBehaviour
{
    public float score;
    public string timeSurvived;
    void Start()
    {
        DontDestroyOnLoad(this);
    }

}
