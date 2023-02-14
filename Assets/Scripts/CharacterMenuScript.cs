using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterMenuScript : MonoBehaviour
{
    //place to put wizard input box
    public Text Wizard_field;
    private int wizardInput;

    public void done(){
        SceneManager.LoadScene("SampleScene");
    }
    //store value of wizard guys
    public void setWizardInput(){
        wizardInput = Convert.ToInt32(Wizard_field.text);
        print(wizardInput);
    }

    public int getWizardInput(){
        return wizardInput;
    }
}
