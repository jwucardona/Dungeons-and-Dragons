using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class WizardButtonsScript : MonoBehaviour
{
    public Button fireBoltButton;
    public Button rayOfFrostButton;
    public Button magicMissileButton;
    public Button scorchingRayButton;
    public Button attackButton;
    public Button spellSlot1Button;
    public Button spellSlot2Button;
    public Button spellSlot3Button;

    public GameObject SS1Parent, SS2Parent, SS3Parent, MMParent, SRParent;

    public TurnControl turnControl;
    [SerializeField] TextMeshProUGUI DiceText;
    [SerializeField] TextMeshProUGUI instructions;
    RollScript Dice;
    public AbstractUnit target;

    string spellChoice = "";

    // Start is called before the first frame update
    void Start()
    {
        Dice = new RollScript();
        fireBoltButton.GetComponent<Button>().onClick.AddListener(FBTaskOnClick);
        rayOfFrostButton.GetComponent<Button>().onClick.AddListener(ROFTaskOnClick);
        magicMissileButton.GetComponent<Button>().onClick.AddListener(MMTaskOnClick);
        scorchingRayButton.GetComponent<Button>().onClick.AddListener(SRTaskOnClick);
        attackButton.GetComponent<Button>().onClick.AddListener(AttackTaskOnClick);
        spellSlot1Button.GetComponent<Button>().onClick.AddListener(SS1TaskOnClick);
        spellSlot2Button.GetComponent<Button>().onClick.AddListener(SS2TaskOnClick);
        spellSlot3Button.GetComponent<Button>().onClick.AddListener(SS3TaskOnClick);
    }

    // Update is called once per frame
    void Update()
    {
        if (WizardUnit.getInstance().getSS1() == 0 && WizardUnit.getInstance().getSS2() == 0 && WizardUnit.getInstance().getSS3() == 0)
        {
            MMParent.SetActive(false);
        }
        if (WizardUnit.getInstance().getSS2() == 0 && WizardUnit.getInstance().getSS3() == 0)
        {
            SRParent.SetActive(false);
        }
    }

    void FBTaskOnClick()
    {
        int turnRoll = Dice.rollD("D20");
        //instructions.text = "cleric " + i + " rolls " + turnRoll;
        DiceText.text = turnRoll.ToString();

        //if the roll is higher than the targets armor class, attack
        if (turnRoll > target.getArmor())
        {
            WizardUnit.getInstance().FireBolt();
        }
        GameControllerScript.getInstance().wizardParentButton.SetActive(false);
    }

    void ROFTaskOnClick()
    {
        int turnRoll = Dice.rollD("D20");
        //instructions.text = "cleric " + i + " rolls " + turnRoll;
        DiceText.text = turnRoll.ToString();

        //if the roll is higher than the targets armor class, attack
        if (turnRoll > target.getArmor())
        {
            WizardUnit.getInstance().RayOfFrost();
        }
        GameControllerScript.getInstance().wizardParentButton.SetActive(false);
    }

    void MMTaskOnClick()
    {
        spellChoice = "MM";

        int turnRoll = Dice.rollD("D20");
        //instructions.text = "cleric " + i + " rolls " + turnRoll;
        DiceText.text = turnRoll.ToString();

        //if the roll is higher than the targets armor class, attack
        if (turnRoll > target.getArmor())
        {
            GameControllerScript.getInstance().wizSpellSlotsParent.SetActive(true);
            if (WizardUnit.getInstance().getSS1() > 0)
            {
                SS1Parent.SetActive(true);
            }
            else
            {
                SS1Parent.SetActive(false);
            }

            if (WizardUnit.getInstance().getSS2() > 0)
            {
                SS2Parent.SetActive(true);
            }
            else
            {
                SS2Parent.SetActive(false);
            }

            if (WizardUnit.getInstance().getSS3() > 0)
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

    void SRTaskOnClick()
    {
        spellChoice = "SR";

        int turnRoll = Dice.rollD("D20");
        //instructions.text = "cleric " + i + " rolls " + turnRoll;
        DiceText.text = turnRoll.ToString();

        //if the roll is higher than the targets armor class, attack
        if (turnRoll > target.getArmor())
        {
            GameControllerScript.getInstance().wizSpellSlotsParent.SetActive(true);


            SS1Parent.SetActive(false);

            if (WizardUnit.getInstance().getSS2() > 0)
            {
                SS2Parent.SetActive(true);
            }
            else
            {
                SS2Parent.SetActive(false);
            }

            if (WizardUnit.getInstance().getSS3() > 0)
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

    void AttackTaskOnClick()
    {
        int turnRoll = Dice.rollD("D20");
        //instructions.text = "cleric " + i + " rolls " + turnRoll;
        DiceText.text = turnRoll.ToString();

        //if the roll is higher than the targets armor class, attack
        if (turnRoll > target.getArmor())
        {
            WizardUnit.getInstance().startAttack();
        }
        GameControllerScript.getInstance().wizardParentButton.SetActive(false);
    }

    void SS1TaskOnClick()
    {
        WizardUnit.getInstance().setSS1(WizardUnit.getInstance().getSS1() - 1);
        GameControllerScript.getInstance().wizSpellSlotsParent.SetActive(false);

        WizardUnit.getInstance().MagicMissile();
        spellChoice = "";
    }

    void SS2TaskOnClick()
    {
        WizardUnit.getInstance().setSS2(WizardUnit.getInstance().getSS2() - 1);
        GameControllerScript.getInstance().wizSpellSlotsParent.SetActive(false);

        if (spellChoice.Equals("MM"))
        {
            WizardUnit.getInstance().MagicMissile();
            spellChoice = "";
        }
        else if (spellChoice.Equals("SR"))
        {
            WizardUnit.getInstance().ScorchingRay();
            spellChoice = "";
        }
    }

    void SS3TaskOnClick()
    {
        WizardUnit.getInstance().setSS3(WizardUnit.getInstance().getSS3() - 1);
        GameControllerScript.getInstance().wizSpellSlotsParent.SetActive(false);

        if (spellChoice.Equals("MM"))
        {
            WizardUnit.getInstance().MagicMissile();
            spellChoice = "";
        }
        else if (spellChoice.Equals("SR"))
        {
            WizardUnit.getInstance().ScorchingRay();
            spellChoice = "";
        }
    }
}
