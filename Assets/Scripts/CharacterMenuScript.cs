    using System.Collections;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;
    using TMPro;

    public class CharacterMenuScript : MonoBehaviour
    {
        //place to put wizard input box
        public Dropdown enemyDropdownMenu;
        public Dropdown wizardDropdownMenu;
        public Dropdown clericDropdownMenu;
        public static int wizardInput = 0;
        public static int clericInput = 0;
        public static int enemyInput = 0;

        private void Start(){
            enemyDropdownMenu.onValueChanged.AddListener(delegate{
                setEnemyInput();
            });
            wizardDropdownMenu.onValueChanged.AddListener(delegate{
                setWizardInput();
            });
            clericDropdownMenu.onValueChanged.AddListener(delegate{
                setClericInput();
            });
        }


        public void done(){
            SceneManager.LoadScene("SampleScene");
        }
        //store value of wizard guys
        public void setWizardInput(){
            wizardInput = wizardDropdownMenu.value;
            print(wizardInput);
        }
        public void setClericInput(){
            clericInput = clericDropdownMenu.value;
            print(clericInput);
        }
        public void setEnemyInput(){
            enemyInput = enemyDropdownMenu.value;
            print(enemyInput);
        }

        public static int getWizardInput(){
            return wizardInput;
        }
        public static int getClericInput(){
            return clericInput;
        }
        public static int getEnemyInput(){
            return enemyInput;
        }
    }
