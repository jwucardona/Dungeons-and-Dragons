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
        public List<int> indexesToDisable = new List<int>();


        //place to put wizard input box
        public Dropdown enemyDropdownMenu;
        public Dropdown wizardDropdownMenu;
        public Dropdown clericDropdownMenu;
        public TextMeshProUGUI wizardText;
        public TextMeshProUGUI clericText;
        public TextMeshProUGUI enemyText;
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
        }

        void Update ()
        {
            wizardText.text = wizardInput.ToString();
            clericText.text = clericInput.ToString();
            enemyText.text = enemyInput.ToString();
        }

        public void done()
        {
            SceneManager.LoadScene("MainScene");
        }
        //store value of wizard guys
        public void setWizardInput()
        {
            wizardInput = wizardDropdownMenu.value;
            clericDropdownMenu.ClearOptions();
            for (int i = 0; i < (6 - wizardInput); i++)
            {
                Dropdown.OptionData NewData = new Dropdown.OptionData();
                NewData.text = (i).ToString();
                clericDropdownMenu.options.Add(NewData);
            }
            //print(wizardInput);
        }
        public void setClericInput()
        {
            clericInput = clericDropdownMenu.value;
            wizardDropdownMenu.ClearOptions();
            for (int i = 0; i < (6 - clericInput); i++)
            {
                Dropdown.OptionData NewData = new Dropdown.OptionData();
                NewData.text = (i).ToString();
                wizardDropdownMenu.options.Add(NewData);
            }
            //print(clericInput);
        }
        public void setEnemyInput(){
            enemyInput = enemyDropdownMenu.value + 1;
            //print(enemyInput);
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
