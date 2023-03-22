using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public enum TurnState { start, cleric, wizard, skelHorse, skeleton, win, lose}; //change states to wizard cleric skeleton etc
/*
    This is how turns should work.
    1. create a list of units in order by turn(turns determined by a d20 roll for each unit at the beginning of the game)
    2. during each units turn they have 3 scenarios: move then attack, attack then move, move then move(again)
    3. units can only move within their movement range(accessable by getMovement() in the AbstractUnit class)
    4. units cannot move to a tile with another unit on it, we need to implement a checker which should be isMovePossible(TileScript tileToMoveTo)
    5. units cannot attack allies, we need to implemet a checker for this as well isAttackPossible(string attackName, AbstractUnit enemy)
        This should check the team of the enemey unit
    6. units can only attack based on the range of their spells/melee attacks, we need to implement a checker for this as well inAttackRange(string attackName).
        This function can be called inside of isAttackPossible().
    7. if a unit chose to attack for their first move, remove attack from the list of options, we could use a function called attackedFirst() or just use a boolean
    8. when a unit chooses to attack, if they have spells, give them a list of spells avaliable to use, once they choose a spell(or melee attack) give them a range
        based off of that attack
    9. once the unit does 2 interactions, iterate to the next unit in the list of turns
    10. when all the units have done their turn, restart the list
    11. if a unit dies, remove it from the list of turns, we can impleemt this in a function public void removeTurn() and call it in the die() function in the AbstractUnit class
    12. everytime a move is made, check the list if all 'good' or 'bad' units are dead, if this is true then end the game
*/


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
   List<AbstractUnit> turnOrder = new List<AbstractUnit>();

    public GameObject wizardParentButton;
    public GameObject clericParentButton;
    public GameObject wizSpellSlotsParent;
    public GameObject cleSpellSlotsParent;
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
        Dice = new RollScript();
        state = TurnState.start;
        StartCoroutine(SettupGame()); //will go to Start battle
        gc = GameControllerScript.getInstance();
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
        //print order 
        /*for(int i = 0; i < turnOrder.Count; i++)
        {
            //instructions.text = "order " + turnOrder[i].tag;    
           // yield return new WaitForSeconds(1f);
        }*/
        IsrollDone = true;
        switchTurn();
        
    }
    public bool rollDone()
    {
        return IsrollDone;
    }
    void switchTurn()
    {
        findPath(turnOrder[0], turnOrder[1]);
       // instructions.text = turnOrder[turnCount].tag + " turn";
        if(turnOrder[turnCount].tag.Equals("Cleric"))
        {
            state = TurnState.cleric;
            //clericAction();
        }
        else if(turnOrder[turnCount].tag.Equals("Skel"))
        {
            state = TurnState.skeleton;
           // SkeletonAction();
        }
        else if(turnOrder[turnCount].tag.Equals("Wiz"))
        {
            state = TurnState.wizard;
           // WizardAction();
        }
        else if(turnOrder[turnCount].tag.Equals("SkelHorse"))
        {
            state = TurnState.skelHorse;
           // SkeletonHorseAction();
        }
        turnCount++; //incriment the count after it is switched 
    }
    
   /* IEnumerator clericAction()
    {
        //working with turnOrder[count] object -- so can call cleric methods on this object
        //if count == turnOrder.Count --> count = 0 //start from beginning again
        //do the if statements and looking at tags again to see which turn to switch to next
    }*/
    IEnumerator SkeletonHorseAction()
    {
        //always moves towards an enemy unless it can attack immedietly
        instructions.text = "SkeletonHorse's turn ";
        yield return new WaitForSeconds(1f);
       // switchTurn(turnCount);

    }
    void SkeletonAction()
    {
        //TurnControl
    }
    //each tile is 5 feet

    /*public GameObject getCurrentPlayer() //returns the players turn
    {
        return turnOrder[turnCount].gameObject;
    }*/

   /* void ActionButtonTask()
    {
        if (getCurrentPlayer().tag.Equals("Wiz"))
        {
            wizardParentButton.SetActive(true);
        }
        else if (getCurrentPlayer().tag.Equals("Cleric"))
        {
            clericParentButton.SetActive(true);
        }
    }*/


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
