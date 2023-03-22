using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class ClericButtonsScript : MonoBehaviour
{
    public Button healingWordButton;
    public Button massHealingWordButton;
    public Button aidButton;
    public Button attackButton;
    public Button spellSlot1Button;
    public Button spellSlot2Button;
    public Button spellSlot3Button;

    public GameObject SS1Parent, SS2Parent, SS3Parent, HWParent, MHWParent, AParent;

    string spellChoice = "";

    public TurnControl turnControl;
    [SerializeField] TextMeshProUGUI DiceText;
    [SerializeField] TextMeshProUGUI instructions;
    RollScript Dice;
    public AbstractUnit target;

    // Start is called before the first frame update
    void Start()
    {
        Dice = new RollScript();
        healingWordButton.GetComponent<Button>().onClick.AddListener(HWTaskOnClick);
        massHealingWordButton.GetComponent<Button>().onClick.AddListener(MHWTaskOnClick);
        aidButton.GetComponent<Button>().onClick.AddListener(ATaskOnClick);
        attackButton.GetComponent<Button>().onClick.AddListener(AttackTaskOnClick);
        spellSlot1Button.GetComponent<Button>().onClick.AddListener(SS1TaskOnClick);
        spellSlot2Button.GetComponent<Button>().onClick.AddListener(SS2TaskOnClick);
        spellSlot3Button.GetComponent<Button>().onClick.AddListener(SS3TaskOnClick);
    }

    // Update is called once per frame
    void Update()
    {
        if (ClericUnit.getInstance().getSS1() == 0 && ClericUnit.getInstance().getSS2() == 0 && ClericUnit.getInstance().getSS3() == 0)
        {
            HWParent.SetActive(false);
        }
        if (ClericUnit.getInstance().getSS2() == 0 && ClericUnit.getInstance().getSS3() == 0)
        {
            AParent.SetActive(false);
        }
        if (ClericUnit.getInstance().getSS3() == 0)
        {
            MHWParent.SetActive(false);
        }
    }

    void HWTaskOnClick()
    {
        spellChoice = "HW";

        int turnRoll = Dice.rollD("D20");
        //instructions.text = "cleric " + i + " rolls " + turnRoll;
        DiceText.text = turnRoll.ToString();

        //if the roll is higher than the targets armor class, attack
        if (turnRoll > target.getArmor())
        {
            GameControllerScript.getInstance().cleSpellSlotsParent.SetActive(true); 
            if (ClericUnit.getInstance().getSS1() > 0)
            {
                SS1Parent.SetActive(true);
            }
            else
            {
                SS1Parent.SetActive(false);
            }

            if (ClericUnit.getInstance().getSS2() > 0)
            {
                SS2Parent.SetActive(true);
            }
            else
            {
                SS2Parent.SetActive(false);
            }

            if (ClericUnit.getInstance().getSS3() > 0)
            {
                SS3Parent.SetActive(true);
            }
            else
            {
                SS3Parent.SetActive(false);
            }
        }
        GameControllerScript.getInstance().wizardParentButton.SetActive(false);
    }

    void MHWTaskOnClick()
    {
        spellChoice = "MHW";
        
        int turnRoll = Dice.rollD("D20");
        //instructions.text = "cleric " + i + " rolls " + turnRoll;
        DiceText.text = turnRoll.ToString();

        //if the roll is higher than the targets armor class, attack
        if (turnRoll > target.getArmor())
        {
            GameControllerScript.getInstance().cleSpellSlotsParent.SetActive(true);

            SS1Parent.SetActive(false);
            SS2Parent.SetActive(false);
            if (ClericUnit.getInstance().getSS3() > 0)
            {
                SS3Parent.SetActive(true);
            }
            else
            {
                SS3Parent.SetActive(false);
            }
        }
        GameControllerScript.getInstance().clericParentButton.SetActive(false);
    }

    void ATaskOnClick()
    {
        spellChoice = "A";

        int turnRoll = Dice.rollD("D20");
        //instructions.text = "cleric " + i + " rolls " + turnRoll;
        DiceText.text = turnRoll.ToString();

        //if the roll is higher than the targets armor class, attack
        if (turnRoll > target.getArmor())
        {
            GameControllerScript.getInstance().cleSpellSlotsParent.SetActive(true);

            SS1Parent.SetActive(false);
            if (ClericUnit.getInstance().getSS2() > 0)
            {
                SS2Parent.SetActive(true);
            }
            else
            {
                SS2Parent.SetActive(false);
            }

            if (ClericUnit.getInstance().getSS3() > 0)
            {
                SS3Parent.SetActive(true);
            }
            else
            {
                SS3Parent.SetActive(false);
            }
        }
        GameControllerScript.getInstance().clericParentButton.SetActive(false);
    }

    void AttackTaskOnClick()
    {
        int turnRoll = Dice.rollD("D20");
        //instructions.text = "cleric " + i + " rolls " + turnRoll;
        DiceText.text = turnRoll.ToString();

        //if the roll is higher than the targets armor class, attack
        if (turnRoll > target.getArmor())
        {
            ClericUnit.getInstance().startAttack();
        }
        GameControllerScript.getInstance().clericParentButton.SetActive(false);
    }

    void SS1TaskOnClick()
    {
        ClericUnit.getInstance().setSS1(ClericUnit.getInstance().getSS1() - 1);
        GameControllerScript.getInstance().cleSpellSlotsParent.SetActive(false);

        ClericUnit.getInstance().HealingWord();
        spellChoice = "";
    }

    void SS2TaskOnClick()
    {
        ClericUnit.getInstance().setSS2(ClericUnit.getInstance().getSS2() - 1);
        GameControllerScript.getInstance().cleSpellSlotsParent.SetActive(false);

        if (spellChoice.Equals("HW"))
        {
            ClericUnit.getInstance().HealingWord();
            spellChoice = "";
        }
        else if (spellChoice.Equals("A"))
        {
            ClericUnit.getInstance().Aid();
            spellChoice = "";
        }
    }

    void SS3TaskOnClick()
    {
        ClericUnit.getInstance().setSS3(ClericUnit.getInstance().getSS3() - 1);
        GameControllerScript.getInstance().cleSpellSlotsParent.SetActive(false);

        if (spellChoice.Equals("HW"))
        {
            ClericUnit.getInstance().HealingWord();
            spellChoice = "";
        }
        else if (spellChoice.Equals("A"))
        {
            ClericUnit.getInstance().Aid();
            spellChoice = "";
        }
        else if (spellChoice.Equals("MHW"))
        {
            ClericUnit.getInstance().MassHealingWord();
            spellChoice = "";
        }
    }
}
