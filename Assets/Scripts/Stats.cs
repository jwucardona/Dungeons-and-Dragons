using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Stats : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI classType;
    [SerializeField] private TextMeshProUGUI ac;
    [SerializeField] private TextMeshProUGUI mov;
    

    public void setStats(string classIn, int acIn, int movIn){
        classType.text = classIn;
        ac.text = acIn.ToString();
        mov.text = movIn.ToString();
    }
}
