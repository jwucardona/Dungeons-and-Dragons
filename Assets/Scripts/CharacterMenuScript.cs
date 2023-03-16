    using System.Collections;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;
    using TMPro;

    public class CharacterMenuScript : MonoBehaviour, IPointerClickHandler
    {
        public List<int> indexesToDisable = new List<int>();


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

            //Trying to gray out options based on other gameobjects selected
            /*Toggle toggle = wizardDropdownMenu.GetComponent<Toggle>();
            Debug.Log(toggle);
            print(Int32.Parse(toggle.name))
            if (toggle != null && Int32.Parse(toggle.name) > 5-clericInput)
            {
                toggle.interactable = false;
            }*/
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var dropDownList = GetComponentInChildren<Canvas>();
            if (!dropDownList) return;

            // If the dropdown was opened find the options toggles
            var toggles = dropDownList.GetComponentsInChildren<Toggle>(true);

            // the first item will always be a template item from the dropdown we have to ignore
            // so we start at one and all options indexes have to be 1 based
            for (var i = 1; i < toggles.Length; i++)
            {
                // disable buttons if their 0-based index is in indexesToDisable
                // the first item will always be a template item from the dropdown
                // so in order to still have 0 based indexes for the options here we use i-1
                toggles[i].interactable = !indexesToDisable.Contains(i - 1);
            }
        }
        public void EnableOption(int index, bool enable)
        {
            if (index < 1 || index > wizardDropdownMenu.options.Count)
            {
                Debug.LogWarning("Index out of range -> ignored!", this);
                return;
            }

            if (enable)
            {
                // remove index from disabled list
                if (indexesToDisable.Contains(index)) indexesToDisable.Remove(index);
            }
            else
            {
                // add index to disabled list
                if (!indexesToDisable.Contains(index)) indexesToDisable.Add(index);
            }

            var dropDownList = GetComponentInChildren<Canvas>();

            // If this returns null than the Dropdown was closed
            if (!dropDownList) return;

            // If the dropdown was opened find the options toggles
            var toggles = dropDownList.GetComponentsInChildren<Toggle>(true);
            toggles[index].interactable = enable;
        }

        // Anytime change a value by string label
        public void EnableOption(string label, bool enable)
        {
            var index = wizardDropdownMenu.options.FindIndex(o => string.Equals(o.text, label));

            // We need a 1-based index
            EnableOption(index + 1, enable);
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
