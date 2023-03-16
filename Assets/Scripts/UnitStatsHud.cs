using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitStatsHud : MonoBehaviour
{
    private ProgressBarScript healthBar;
    private Stats stats;

    public void UpdateHealth(int currentHealth, int maxHealth){
        healthBar.setValues(currentHealth, maxHealth);
    }

    public void createStats(string classIn, string armorIn, string weaponIn, int acIn, int movIn){
       stats.setStats(classIn, armorIn, weaponIn, acIn, movIn);
    }
}
