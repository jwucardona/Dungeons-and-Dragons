using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Stats : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI classType;
    [SerializeField] private TextMeshProUGUI armor;
    [SerializeField] private TextMeshProUGUI weapon;
    [SerializeField] private TextMeshProUGUI ac;
    [SerializeField] private TextMeshProUGUI mov;
    

    public void setStats(string classIn, string armorIn, string weaponIn, int acIn, int movIn){
        classType.text = classIn;
        armor.text = armorIn;
        weapon.text = weaponIn;
        ac.text = acIn.ToString();
        mov.text = movIn.ToString();
    }
}
