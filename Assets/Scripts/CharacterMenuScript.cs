    using System.Collections;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;
    using TMPro;

    public class CharacterMenuScript : MonoBehaviour
    {
       
        //place to put wizard input box
        public Dropdown enemyDropdownMenu;
        public Dropdown wizardDropdownMenu;
        public Dropdown clericDropdownMenu;
        public static int wizardInput = 0;
        public static int clericInput = 0;
        public static int enemyInput = 1;

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

            //Trying to gray out options based on other gameobjects selected
            /*Toggle toggle = wizardDropdownMenu.GetComponent<Toggle>();
            Debug.Log(toggle);
            print(Int32.Parse(toggle.name))
            if (toggle != null && Int32.Parse(toggle.name) > 5-clericInput)
            {
                toggle.interactable = false;
            }*/
        }

        public void done(){
            SceneManager.LoadScene("MainScene");
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
            enemyInput = enemyDropdownMenu.value + 1;
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
