using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayEndScore : MonoBehaviour
{
    PersistentStats persistentStats;
    [SerializeField] TextMeshProUGUI scoreValue;
    [SerializeField] TextMeshProUGUI timeValue;

    void Start()
    {
        persistentStats = FindObjectOfType<PersistentStats>();
        scoreValue.text = persistentStats.score.ToString();
        timeValue.text = persistentStats.timeSurvived;
    }
}
