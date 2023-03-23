using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public enum TurnState { start, cleric, wizard, skelHorse, skeleton, win, lose}; //change states to wizard cleric skeleton etc

public class TurnControl : MonoBehaviour
{
    // Start is called before the first frame update
    //holds player wizard and cleric GameObjects
    public GameObject wizardPrefab;
    public GameObject clericPrefab;

    public GameObject skeletonPrefab;
    public GameObject warhorsePrefab;
    private GameControllerScript gc;


   [SerializeField] TextMeshProUGUI DiceText;
   [SerializeField] TextMeshProUGUI instructions;
   RollScript Dice;
   //5 good players
   List<WizardUnit> wiz = new List<WizardUnit>();
   List<ClericUnit> cleric = new List<ClericUnit>();
   List<SkelHorseUnit> skelHorse = new List<SkelHorseUnit>();
   List<SkeletonUnit> skel = new List<SkeletonUnit>();
   List<AbstractUnit> allUnits = new List<AbstractUnit>();
   private int turnCount = 0;
    private int countMoves;
    List<AbstractUnit> turnOrder = new List<AbstractUnit>();

    public GameObject wizardParentButton, clericParentButton, wizSpellSlotsParent, cleSpellSlotsParent, actionParent, moveParent, SS1Parent, SS2Parent, SS3Parent;
    string spellChoice = "";
    private bool IsrollDone = false;

    public void addSkelHorse(GameObject skH)
   {
        skelHorse.Add(skH.GetComponent<SkelHorseUnit>());
        allUnits.Add(skH.GetComponent<SkelHorseUnit>());
   }
   public void addCleric(GameObject cler)
   {
        cleric.Add(cler.GetComponent<ClericUnit>());
        allUnits.Add(cler.GetComponent<ClericUnit>());
   }
   public void addSkel(GameObject Sk)
   {
        skel.Add(Sk.GetComponent<SkeletonUnit>());
        allUnits.Add(Sk.GetComponent<SkeletonUnit>());
   }
   public void addWiz(GameObject wizard)
   {
       wiz.Add(wizard.GetComponent<WizardUnit>());
       allUnits.Add(wizard.GetComponent<WizardUnit>());
   }
   public TurnState state;
    
    void Start()
    {
        actionParent.SetActive(false);
        moveParent.SetActive(false);
        Dice = new RollScript();
        state = TurnState.start;
        StartCoroutine(SettupGame()); //will go to Start battle
        gc = GameControllerScript.getInstance();
    }

    void Update()
    {
        if (countMoves >= 2)
        {
            turnCount++;
            switchTurn();
        }
    }
    //roll D20 for all abstract Units and sort the list to determine the order

    //names use .getType() and they are"Wiz" "SkH" "Sk" "Cle"
    IEnumerator SettupGame() //coroutine aka waits until switches turns etc
    {
        //Dictionary<string, int> turnDict = new Dictionary<string,int>();
        Dictionary<AbstractUnit, int> turnDict = new Dictionary<AbstractUnit,int>();

        instructions.text = "Rolling for order...";
        yield return new WaitForSeconds(1f);
        for(int i = 0; i< cleric.Count; i++)
        {
            int turnRoll = Dice.rollD("D20");
            instructions.text = "cleric " + i + " rolls " + turnRoll;
            DiceText.text = turnRoll.ToString();
            //string whichUnit = "cleric" + i.ToString();
            turnDict.Add(cleric[i], turnRoll);
            yield return new WaitForSeconds(1f);
        }
        for(int i = 0; i< wiz.Count; i++)
        {
            int turnRoll = Dice.rollD("D20");
            instructions.text = "Wizard " + i + " rolls " + turnRoll;
            DiceText.text = turnRoll.ToString();
            string whichUnit = "Wizard" + i.ToString();
            turnDict.Add(wiz[i], turnRoll);
            yield return new WaitForSeconds(1f);
        }
        for(int i = 0; i< skel.Count; i++)
        {
            int turnRoll = Dice.rollD("D20");
            instructions.text = "Skeleton " + i + " rolls " + turnRoll;
            DiceText.text = turnRoll.ToString();
            string whichUnit = "Skeleton" + i.ToString();
            turnDict.Add(skel[i], turnRoll);
            yield return new WaitForSeconds(1f);
        }
        for(int i = 0; i< skelHorse.Count; i++)
        {
            int turnRoll = Dice.rollD("D20");
            instructions.text = "SkeletonHorse " + i + " rolls " + turnRoll;
            DiceText.text = turnRoll.ToString();
            string whichUnit = "SkeletonHorse" + i.ToString();
            turnDict.Add(skelHorse[i], turnRoll);
            yield return new WaitForSeconds(1f);
        }
        
        foreach (KeyValuePair<AbstractUnit,int> item in turnDict.OrderBy(key => key.Value)) //sort based on the rolls
        { 
           turnOrder.Add(item.Key);
        }
        turnOrder.Reverse(); //reverse the list so it is in the right order
        
        IsrollDone = true;
        switchTurn();
        
        
    }
    public bool rollDone()
    {
        return IsrollDone;
    }
    public void switchTurn()
    {
        countMoves = 0;
        //findPath(turnOrder[0], turnOrder[1]);
        if(turnCount == turnOrder.Count) //start from beginning
        {
              turnCount = 0;
        }
        //instructions.text = turnOrder[turnCount].tag + " turn";
        if(turnOrder[turnCount].tag.Equals("Cleric"))
        {
            actionParent.SetActive(true);
            moveParent.SetActive(true);
            state = TurnState.cleric;
            StartCoroutine(clericAction());
            //clericAction();
        }
        if(turnOrder[turnCount].tag.Equals("Skel"))
        {
            actionParent.SetActive(false);
            moveParent.SetActive(false);
            state = TurnState.skeleton;
            StartCoroutine(SkeletonAction());
        }
        if(turnOrder[turnCount].tag.Equals("Wiz"))
        {
            actionParent.SetActive(true);
            moveParent.SetActive(true);
            state = TurnState.wizard;
            StartCoroutine(WizardAction());
        }
        if(turnOrder[turnCount].tag.Equals("SkelHorse"))
        {
            actionParent.SetActive(false);
            moveParent.SetActive(false);
            state = TurnState.skelHorse;
            StartCoroutine(SkeletonHorseAction());
        }
        //turnCount++; //incriment the count after it is switched 
    }
    
    IEnumerator clericAction()
    {
        //working with turnOrder[count] object -- so can call cleric methods on this object
        instructions.text = "Cleric's turn select move or action ";
        yield return new WaitForSeconds(1f);
        //switchTurn();
        //do the if statements and looking at tags again to see which turn to switch to next
    }
    IEnumerator SkeletonHorseAction()
    {
        //always moves towards an enemy unless it can attack immedietly
        instructions.text = "SkeletonHorse's turn ";
        yield return new WaitForSeconds(1f);
        countMoves = 2;
        //switchTurn();

    }
    IEnumerator SkeletonAction()
    {
        instructions.text = "Skeleton's turn ";
        yield return new WaitForSeconds(1f);
        countMoves = 2;
        //switchTurn();
    }
    IEnumerator WizardAction()
    {
        instructions.text = "Wizard's turn select move or action ";
         yield return new WaitForSeconds(1f);
        //switchTurn();
    }

    public void FBTaskOnClick()
    {
        int turnRoll = Dice.rollD("D20");
        //instructions.text = "cleric " + i + " rolls " + turnRoll;
        DiceText.text = turnRoll.ToString();
        WizardUnit tempWizard = (WizardUnit)turnOrder[turnCount];

        //if the roll is higher than the targets armor class, attack
        //if (turnRoll > target.getArmor())
        //{
            tempWizard.FireBolt();
        //}
        wizardParentButton.SetActive(false);
        countMoves++;
    }

    public void ROFTaskOnClick()
    {
        int turnRoll = Dice.rollD("D20");
        //instructions.text = "cleric " + i + " rolls " + turnRoll;
        DiceText.text = turnRoll.ToString();
        WizardUnit tempWizard = (WizardUnit)turnOrder[turnCount];

        //if the roll is higher than the targets armor class, attack
        //if (turnRoll > target.getArmor())
        //{
            tempWizard.RayOfFrost();
        //}
        wizardParentButton.SetActive(false);
        countMoves++;
    }

    public void MMTaskOnClick()
    {
        spellChoice = "MM";

        int turnRoll = Dice.rollD("D20");
        //instructions.text = "cleric " + i + " rolls " + turnRoll;
        DiceText.text = turnRoll.ToString();

        //if the roll is higher than the targets armor class, attack
        //if (turnRoll > target.getArmor())
        //{
            wizSpellSlotsParent.SetActive(true);
            if (turnOrder[turnCount].getSS1() > 0)
            {
                SS1Parent.SetActive(true);
            }
            else
            {
                SS1Parent.SetActive(false);
            }

            if (turnOrder[turnCount].getSS2() > 0)
            {
                SS2Parent.SetActive(true);
            }
            else
            {
                SS2Parent.SetActive(false);
            }

            if (turnOrder[turnCount].getSS3() > 0)
            {
                SS3Parent.SetActive(true);
            }
            else
            {
                SS3Parent.SetActive(false);
            }
        //}
        wizardParentButton.SetActive(false);
    }

    public void SRTaskOnClick()
    {
        spellChoice = "SR";

        int turnRoll = Dice.rollD("D20");
        //instructions.text = "cleric " + i + " rolls " + turnRoll;
        DiceText.text = turnRoll.ToString();

        //if the roll is higher than the targets armor class, attack
        //if (turnRoll > target.getArmor())
        //{
            wizSpellSlotsParent.SetActive(true);


            SS1Parent.SetActive(false);

            if (turnOrder[turnCount].getSS2() > 0)
            {
                SS2Parent.SetActive(true);
            }
            else
            {
                SS2Parent.SetActive(false);
            }

            if (turnOrder[turnCount].getSS3() > 0)
            {
                SS3Parent.SetActive(true);
            }
            else
            {
                SS3Parent.SetActive(false);
            }
        //}
        wizardParentButton.SetActive(false);
    }

    public void AttackTaskOnClick()
    {
        int turnRoll = Dice.rollD("D20");
        //instructions.text = "cleric " + i + " rolls " + turnRoll;
        DiceText.text = turnRoll.ToString();

        if (turnOrder[turnCount].tag.Equals("Wiz"))
        {
            WizardUnit tempWizard = (WizardUnit)turnOrder[turnCount];
            //if the roll is higher than the targets armor class, attack
            //if (turnRoll > target.getArmor())
            //{
                tempWizard.startAttack();
            //}
        }
        else if (turnOrder[turnCount].tag.Equals("Cleric"))
        {
            ClericUnit tempCleric = (ClericUnit)turnOrder[turnCount];
            //if the roll is higher than the targets armor class, attack
            //if (turnRoll > target.getArmor())
            //{
                tempCleric.startAttack();
            //}
        }

        wizardParentButton.SetActive(false);
        countMoves++;
    }

    public void HWTaskOnClick()
    {
        spellChoice = "HW";

        int turnRoll = Dice.rollD("D20");
        //instructions.text = "cleric " + i + " rolls " + turnRoll;
        DiceText.text = turnRoll.ToString();

        //if the roll is higher than the targets armor class, attack
        //if (turnRoll > target.getArmor())
        //{
            cleSpellSlotsParent.SetActive(true);
            if (turnOrder[turnCount].getSS1() > 0)
            {
                SS1Parent.SetActive(true);
            }
            else
            {
                SS1Parent.SetActive(false);
            }

            if (turnOrder[turnCount].getSS2() > 0)
            {
                SS2Parent.SetActive(true);
            }
            else
            {
                SS2Parent.SetActive(false);
            }

            if (turnOrder[turnCount].getSS3() > 0)
            {
                SS3Parent.SetActive(true);
            }
            else
            {
                SS3Parent.SetActive(false);
            }
        //}
        clericParentButton.SetActive(false);
    }

    public void MHWTaskOnClick()
    {
        spellChoice = "MHW";

        int turnRoll = Dice.rollD("D20");
        //instructions.text = "cleric " + i + " rolls " + turnRoll;
        DiceText.text = turnRoll.ToString();

        //if the roll is higher than the targets armor class, attack
        //if (turnRoll > target.getArmor())
        //{
            cleSpellSlotsParent.SetActive(true);

            SS1Parent.SetActive(false);
            SS2Parent.SetActive(false);
            if (turnOrder[turnCount].getSS3() > 0)
            {
                SS3Parent.SetActive(true);
            }
            else
            {
                SS3Parent.SetActive(false);
            }
        //}
        clericParentButton.SetActive(false);
    }

    public void ATaskOnClick()
    {
        spellChoice = "A";

        int turnRoll = Dice.rollD("D20");
        //instructions.text = "cleric " + i + " rolls " + turnRoll;
        DiceText.text = turnRoll.ToString();

        //if the roll is higher than the targets armor class, attack
        //if (turnRoll > target.getArmor())
        //{
            cleSpellSlotsParent.SetActive(true);

            SS1Parent.SetActive(false);
            if (turnOrder[turnCount].getSS2() > 0)
            {
                SS2Parent.SetActive(true);
            }
            else
            {
                SS2Parent.SetActive(false);
            }

            if (turnOrder[turnCount].getSS3() > 0)
            {
                SS3Parent.SetActive(true);
            }
            else
            {
                SS3Parent.SetActive(false);
            }
        //}
        clericParentButton.SetActive(false);
    }

    public void SS1TaskOnClick()
    {
        turnOrder[turnCount].setSS1(turnOrder[turnCount].getSS1() - 1);
        if (turnOrder[turnCount].tag.Equals("Wiz"))
        {
            WizardUnit tempWizard = (WizardUnit)turnOrder[turnCount];
            tempWizard.MagicMissile();
            spellChoice = "";
        }
        else if (turnOrder[turnCount].tag.Equals("Cleric"))
        {
            ClericUnit tempCleric = (ClericUnit)turnOrder[turnCount];
            tempCleric.HealingWord();
        }
        countMoves++;
        wizSpellSlotsParent.SetActive(false);
        cleSpellSlotsParent.SetActive(false);
    }

    public void SS2TaskOnClick()
    {
        turnOrder[turnCount].setSS2(turnOrder[turnCount].getSS2() - 1);
        if (turnOrder[turnCount].tag.Equals("Wiz"))
        {
            WizardUnit tempWizard = (WizardUnit)turnOrder[turnCount];
            if (spellChoice.Equals("MM"))
            {
                tempWizard.MagicMissile();
                spellChoice = "";
            }
            else if (spellChoice.Equals("SR"))
            {
                tempWizard.ScorchingRay();
                spellChoice = "";
            }
        }
        else if (turnOrder[turnCount].tag.Equals("Cleric"))
        {
            ClericUnit tempCleric = (ClericUnit)turnOrder[turnCount];
            if (spellChoice.Equals("HW"))
            {
                tempCleric.HealingWord();
                spellChoice = "";
            }
            else if (spellChoice.Equals("A"))
            {
                tempCleric.Aid();
                spellChoice = "";
            }
        }
        countMoves++;
        wizSpellSlotsParent.SetActive(false);
        cleSpellSlotsParent.SetActive(false);
    }

    public void SS3TaskOnClick()
    {
        turnOrder[turnCount].setSS3(turnOrder[turnCount].getSS3() - 1);

        if (turnOrder[turnCount].tag.Equals("Wiz"))
        {
            WizardUnit tempWizard = (WizardUnit)turnOrder[turnCount];
            if (spellChoice.Equals("MM"))
            {
                tempWizard.MagicMissile();
                spellChoice = "";
            }
            else if (spellChoice.Equals("SR"))
            {
                tempWizard.ScorchingRay();
                spellChoice = "";
            }
        }
        else if (turnOrder[turnCount].tag.Equals("Cleric"))
        {
            ClericUnit tempCleric = (ClericUnit)turnOrder[turnCount];
            if (spellChoice.Equals("HW"))
            {
                tempCleric.HealingWord();
                spellChoice = "";
            }
            else if (spellChoice.Equals("A"))
            {
                tempCleric.Aid();
                spellChoice = "";
            }
            else if (spellChoice.Equals("MHW"))
            {
                tempCleric.MassHealingWord();
                spellChoice = "";
            }
        }
        countMoves++;
        wizSpellSlotsParent.SetActive(false);
        cleSpellSlotsParent.SetActive(false);
    }


    //each tile is 5 feet

    public GameObject getCurrentPlayer() //returns the players turn
    {
        return turnOrder[turnCount].gameObject;
    }

    void ActionButtonTask()
    {
        if (turnOrder[turnCount].tag.Equals("Wiz"))
        {
            print("wizard");
            wizardParentButton.SetActive(true);
        }
        else if (turnOrder[turnCount].tag.Equals("Cleric"))
        {
            print("cleric");
            clericParentButton.SetActive(true);
        }
    }

    void MoveButtonTask()
    {
        //countMoves++;
    }

    public void setCountMoves(int num)
    {
        countMoves = countMoves + num;
    }


    private List<TileScript> tq = new List<TileScript>();
    private List<TileScript> temp = new List<TileScript>();
    private List<TileScript> path = new List<TileScript>();
    private TileScript start, end;
    
    int findPath(AbstractUnit p1, AbstractUnit p2)
    {
        //clear all nodes from past comuting

        TileScript[] tile = gc.getTiles();

        for (int i = 0; i < tile.Length; i++)
        {
            tile[i].clear();
        }
        for (int i = 0; i < tile.Length; i++)
        {
            if(p1.transform.position.x == tile[i].transform.position.x && p1.transform.position.z == tile[i].transform.position.z)
            {
                start = tile[i];
                print("setting start to " + i);
            }
            if(p2.transform.position.x == tile[i].transform.position.x && p2.transform.position.z == tile[i].transform.position.z)
            {
                end = tile[i];
                print("setting end to " + i);
            }
        }
        TileScript current;

        //start this dikjkstra
        start.setDistance(0);
        tq.Clear();
        tq.Add(start);

        while (tq.Count > 0)
        {
            //find least tile
            int smallestIndex = 0;
            for (int i = 1; i < tq.Count; i++)
            {
                if (tq[i].getDistance() < tq[smallestIndex].getDistance())
                {
                    smallestIndex = i;
                }
            }

            current = tq[smallestIndex];
            tq.RemoveAt(smallestIndex);

            current.setVisited(true);

            if (current == end)
            {
                break;
            }

            for (int i = 0; i < current.getNeighbors().Count; i++)
            {
                if (!current.getNeighbors()[i].getVisited())
                {
                    //not previously seen
                    if (current.getNeighbors()[i].getDistance() == float.MaxValue)
                    {
                        tq.Add(current.getNeighbors()[i]);
                    }

                    float discreteDistance = current.getDistance() + 2;
                    if (current.getNeighbors()[i].getDistance() > discreteDistance)
                    {
                        current.getNeighbors()[i].setDistance(discreteDistance);
                        current.getNeighbors()[i].setBackPointer(current);
                    }
                }
            }
        }

        temp.Clear();
        path.Clear();
        current = end;
        while (current != start && current != null) 
        {
            //current is the path from each node starting from the back to the front
            temp.Add(current);
            current = current.getBackPointer();
        }
        temp.Add(start);
        for (int i=temp.Count-1; i>=0; i--)
        {
            path.Add(temp[i]);
        }
        instructions.text = path.Count.ToString();
        return path.Count;
    }
    
}
