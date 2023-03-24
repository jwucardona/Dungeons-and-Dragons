using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressBarScript : MonoBehaviour
{
    private int baseValue;
    private int maxValue;

    [SerializeField] private Image fill;
    [SerializeField] private TextMeshProUGUI amount;

    public void setValues(int baseValue, int maxValue){
        this.baseValue = baseValue;
        this.maxValue = maxValue;
        amount.text = baseValue.ToString();

        CalculateFillAmount();
    }

    private void CalculateFillAmount(){
        float fillAmount = (float)baseValue / (float)maxValue;
        fill.fillAmount = fillAmount;
    }
}
