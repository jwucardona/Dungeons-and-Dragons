using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitStatsHud : MonoBehaviour
{
    private ProgressBarScript healthBar;

    public void UpdateHealth(int currentHealth, int maxHealth){
        healthBar.setValues(currentHealth, maxHealth);
    }
}
