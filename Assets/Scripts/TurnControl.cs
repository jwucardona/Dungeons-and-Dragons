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
    [SerializeField] private UnitStatsHud hud;


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

    List<AbstractUnit> playersInRange = new List<AbstractUnit>();
    List<TileScript> playersInRangeTiles = new List<TileScript>();
    bool lightUp;
    TileScript tileTarget;
    AbstractUnit playerTarget;

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
        // hud = GetComponent<UnitStatsHud>();
    }

    void Update()
    {
        for (int i = 0; i < turnOrder.Count; i++)
        {
            if (turnOrder[i] == null)
            {
                turnOrder.RemoveAt(i);
            }
        }
            //light up current character's tile
            TileScript[] tiles = gc.getTiles();
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = 0; j < turnOrder.Count; j++)
            {
                if (turnOrder[j] == turnOrder[turnCount])
                {
                    if (turnOrder[j].transform.position.x == tiles[i].transform.position.x && turnOrder[j].transform.position.z == tiles[i].transform.position.z)
                    {
                        tiles[i].setColor(Color.green * 2);
                    }
                }
            }
        }

        if (countMoves >= 2)
        {
            turnCount++;
            switchTurn();
        }
       if (lightUp)
        {
            tiles = gc.getTiles();
            for (int i = 0; i < tiles.Length; i++)
            {
                for (int j = 0; j < playersInRange.Count; j++)
                {
                    if (playersInRange[j].transform.position.x == tiles[i].transform.position.x && playersInRange[j].transform.position.z == tiles[i].transform.position.z)
                    {
                        tiles[i].setColor(Color.red * 2); //this needs to be called in an update / be active for a longer time
                        if (tileTarget != null)
                        {
                            tileTarget.setColor(Color.blue * 2);
                        }
                        //tiles[i].setColor(Color.red * 2); //this needs to be called in an update / be active for a longer time
                        //playersInRangeTiles.Add(tiles[i]);
                    }
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                //print("test");
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    if (hit.transform.tag == "Tile")
                    {
                        //print("test1");
                        print(playersInRangeTiles.Count);
                        if (playersInRangeTiles.Contains(hit.transform.gameObject.GetComponent<TileScript>()))
                        {
                            //print("test2");
                            tileTarget = hit.transform.gameObject.GetComponent<TileScript>();

                            //lightUp = false;
                        }
                    }
                }
            }
            if (tileTarget != null && Input.GetKeyDown(KeyCode.Return))
            {
                int turnRoll = Dice.rollD("D20");
                DiceText.text = turnRoll.ToString();

                if (playersInRange.Count == 0)
                {
                    instructions.text = "No units in range!";
                    countMoves++;
                }

                for (int j = 0; j < playersInRange.Count; j++)
                {
                    if (playersInRange[j].transform.position.x == tileTarget.transform.position.x && playersInRange[j].transform.position.z == tileTarget.transform.position.z)
                    {
                        playerTarget = playersInRange[j];
                    }
                }
                if (turnOrder[turnCount].tag.Equals("Wiz"))
                {
                    WizardUnit tempWizard = (WizardUnit)turnOrder[turnCount];
                    if (spellChoice.Equals("Attack"))
                    {
                        tempWizard.startAttack(playerTarget.gameObject);
                        instructions.text = "Melee attack!";
                        countMoves++;
                    }
                    else if (turnRoll + 3 > playerTarget.getArmorC())
                    {
                        //cast spell
                        if (spellChoice.Equals("FB"))
                        {
                            tempWizard.FireBolt(playerTarget.gameObject);
                            turnRoll = Dice.rollD("D10");
                            DiceText.text = turnRoll.ToString();
                            instructions.text = "FireBolt cast for " + turnRoll.ToString() + " damage.";
                            StartCoroutine(playerTarget.takeDamage(turnRoll));
                            spellChoice = "";
                            countMoves++;
                        }
                        else if (spellChoice.Equals("ROF"))
                        {
                            tempWizard.RayOfFrost(playerTarget.gameObject);
                            turnRoll = Dice.rollD("D8");
                            DiceText.text = turnRoll.ToString();
                            instructions.text = "Ray Of Frost cast for " + turnRoll.ToString() + " damage.";
                            StartCoroutine(playerTarget.takeDamage(turnRoll));
                            spellChoice = "";
                            countMoves++;
                        }
                        else if (spellChoice.Equals("MM"))
                        {
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
                        }
                        else if (spellChoice.Equals("SR"))
                        {
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
                        }
                    }
                    else
                    {
                        instructions.text = "Spell did not penetrate armor.";
                        countMoves++;
                    }
                }
                else if (turnOrder[turnCount].tag.Equals("Cleric"))
                {
                    ClericUnit tempCleric = (ClericUnit)turnOrder[turnCount];
                    if (spellChoice.Equals("Attack"))
                    {
                        tempCleric.startAttack(playerTarget.gameObject);
                        countMoves++;
                    }
                    else if (turnRoll > 5)
                    {
                        //cast spell
                        if (spellChoice.Equals("HW"))
                        {
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
                        }
                        else if (spellChoice.Equals("MHW"))
                        {
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
                        }
                        else if (spellChoice.Equals("A"))
                        {
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
                        }
                    }
                    else
                    {
                        instructions.text = "Did not roll high enough for healing spell.";
                        countMoves++;
                    }
                }
                lightUp = false;
            }
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
       // getPlayersInRange(wiz[0],turnOrder,600);
        switchTurn();
        
        
    }
    public bool rollDone()
    {
        return IsrollDone;
    }
    public void switchTurn()
    {
        countMoves = 0;
        playersInRange.Clear();
        playersInRangeTiles.Clear();
        tileTarget = null;
        //findPath(turnOrder[0], turnOrder[1]);
        if (turnCount == turnOrder.Count) //start from beginning
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
        hud.UpdateHealth(turnOrder[turnCount].getHp(), turnOrder[turnCount].getMaxHp());
        hud.createStats(turnOrder[turnCount].getClass(), turnOrder[turnCount].getArmor(), turnOrder[turnCount].getWeapon(), turnOrder[turnCount].getArmorC(), turnOrder[turnCount].getMove());
        yield return new WaitForSeconds(1f);
        //switchTurn();
        //do the if statements and looking at tags again to see which turn to switch to next
    }
    IEnumerator SkeletonHorseAction()
    {
        //always moves towards an enemy unless it can attack immedietly
        instructions.text = "SkeletonHorse's turn ";
        yield return new WaitForSeconds(1f);
        PlayerMover.startMovement();
        yield return new WaitForSeconds(6f);
        countMoves = 2;
        hud.UpdateHealth(turnOrder[turnCount].getHp(), turnOrder[turnCount].getMaxHp());
        hud.createStats(turnOrder[turnCount].getClass(), turnOrder[turnCount].getArmor(), turnOrder[turnCount].getWeapon(), turnOrder[turnCount].getArmorC(), turnOrder[turnCount].getMove());
        //switchTurn();
    }
    IEnumerator SkeletonAction()
    {
        instructions.text = "Skeleton's turn ";
        yield return new WaitForSeconds(1f);
        PlayerMover.startMovement();
        yield return new WaitForSeconds(6f);
        countMoves = 2;
        hud.UpdateHealth(turnOrder[turnCount].getHp(), turnOrder[turnCount].getMaxHp());
        hud.createStats(turnOrder[turnCount].getClass(), turnOrder[turnCount].getArmor(), turnOrder[turnCount].getWeapon(), turnOrder[turnCount].getArmorC(), turnOrder[turnCount].getMove());
        //switchTurn();
    }
    IEnumerator WizardAction()
    {
        instructions.text = "Wizard's turn select move or action ";
        hud.UpdateHealth(turnOrder[turnCount].getHp(), turnOrder[turnCount].getMaxHp());
        hud.createStats(turnOrder[turnCount].getClass(), turnOrder[turnCount].getArmor(), turnOrder[turnCount].getWeapon(), turnOrder[turnCount].getArmorC(), turnOrder[turnCount].getMove());
        yield return new WaitForSeconds(1f);
        //switchTurn();
    }
    
    public void FBTaskOnClick()
    {
        spellChoice = "FB";

        playersInRange = getPlayersInRange(turnOrder[turnCount], turnOrder, 120);
        lightUp = true;

        TileScript[] tiles = gc.getTiles();
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = 0; j < playersInRange.Count; j++)
            {
                if (playersInRange[j].transform.position.x == tiles[i].transform.position.x && playersInRange[j].transform.position.z == tiles[i].transform.position.z)
                {
                    playersInRangeTiles.Add(tiles[i]);
                }
            }
        }
        wizardParentButton.SetActive(false);
    }

    public void ROFTaskOnClick()
    {
        spellChoice = "ROF";

        playersInRange = getPlayersInRange(turnOrder[turnCount], turnOrder, 60);
        lightUp = true;

        TileScript[] tiles = gc.getTiles();
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = 0; j < playersInRange.Count; j++)
            {
                if (playersInRange[j].transform.position.x == tiles[i].transform.position.x && playersInRange[j].transform.position.z == tiles[i].transform.position.z)
                {
                    playersInRangeTiles.Add(tiles[i]);
                }
            }
        }

        wizardParentButton.SetActive(false);
    }

    public void MMTaskOnClick()
    {
        spellChoice = "MM";

        playersInRange = getPlayersInRange(turnOrder[turnCount], turnOrder, 120);
        lightUp = true;

        TileScript[] tiles = gc.getTiles();
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = 0; j < playersInRange.Count; j++)
            {
                if (playersInRange[j].transform.position.x == tiles[i].transform.position.x && playersInRange[j].transform.position.z == tiles[i].transform.position.z)
                {
                    playersInRangeTiles.Add(tiles[i]);
                }
            }
        }
        wizardParentButton.SetActive(false);
    }

    public void SRTaskOnClick()
    {
        spellChoice = "SR";

        playersInRange = getPlayersInRange(turnOrder[turnCount], turnOrder, 120);
        lightUp = true;

        TileScript[] tiles = gc.getTiles();
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = 0; j < playersInRange.Count; j++)
            {
                if (playersInRange[j].transform.position.x == tiles[i].transform.position.x && playersInRange[j].transform.position.z == tiles[i].transform.position.z)
                {
                    playersInRangeTiles.Add(tiles[i]);
                }
            }
        }
        wizardParentButton.SetActive(false);
    }

    public void AttackTaskOnClick()
    {
        spellChoice = "Attack";

        playersInRange = getPlayersInRange(turnOrder[turnCount], turnOrder, 10);
        lightUp = true;

        TileScript[] tiles = gc.getTiles();
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = 0; j < playersInRange.Count; j++)
            {
                if (playersInRange[j].transform.position.x == tiles[i].transform.position.x && playersInRange[j].transform.position.z == tiles[i].transform.position.z)
                {
                    playersInRangeTiles.Add(tiles[i]);
                }
            }
        }
        wizardParentButton.SetActive(false);
        clericParentButton.SetActive(false);
    }

    public void HWTaskOnClick()
    {
        print("test");
        spellChoice = "HW";

        playersInRange = getPlayersInRange(turnOrder[turnCount], turnOrder, 60);
        lightUp = true;

        TileScript[] tiles = gc.getTiles();
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = 0; j < playersInRange.Count; j++)
            {
                if (playersInRange[j].transform.position.x == tiles[i].transform.position.x && playersInRange[j].transform.position.z == tiles[i].transform.position.z)
                {
                    playersInRangeTiles.Add(tiles[i]);
                }
            }
        }
        clericParentButton.SetActive(false);
    }

    public void MHWTaskOnClick()
    {
        spellChoice = "MHW";

        playersInRange = getPlayersInRange(turnOrder[turnCount], turnOrder, 60);
        lightUp = true;

        TileScript[] tiles = gc.getTiles();
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = 0; j < playersInRange.Count; j++)
            {
                if (playersInRange[j].transform.position.x == tiles[i].transform.position.x && playersInRange[j].transform.position.z == tiles[i].transform.position.z)
                {
                    playersInRangeTiles.Add(tiles[i]);
                }
            }
        }
        clericParentButton.SetActive(false);
    }

    public void ATaskOnClick()
    {
        spellChoice = "A";

        playersInRange = getPlayersInRange(turnOrder[turnCount], turnOrder, 30);
        lightUp = true;

        TileScript[] tiles = gc.getTiles();
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = 0; j < playersInRange.Count; j++)
            {
                if (playersInRange[j].transform.position.x == tiles[i].transform.position.x && playersInRange[j].transform.position.z == tiles[i].transform.position.z)
                {
                    playersInRangeTiles.Add(tiles[i]);
                }
            }
        }
        clericParentButton.SetActive(false);
    }

    public void SS1TaskOnClick()
    {
        turnOrder[turnCount].setSS1(turnOrder[turnCount].getSS1() - 1);
        if (turnOrder[turnCount].tag.Equals("Wiz"))
        {
            WizardUnit tempWizard = (WizardUnit)turnOrder[turnCount];
            tempWizard.MagicMissile(playerTarget.gameObject);
            int turnRoll = Dice.rollD("D4");
            int temp = 3 * (turnRoll + 3);
            DiceText.text = turnRoll.ToString();
            instructions.text = "Magic Missile cast for " + temp.ToString() + " damage.";
            StartCoroutine(playerTarget.takeDamage(temp));
            spellChoice = "";
        }
        else if (turnOrder[turnCount].tag.Equals("Cleric"))
        {
            ClericUnit tempCleric = (ClericUnit)turnOrder[turnCount];
            tempCleric.HealingWord(playerTarget.gameObject);
            int turnRoll = Dice.rollD("D4");
            DiceText.text = turnRoll.ToString();
            instructions.text = "Healing Word cast for " + turnRoll.ToString() + " healing.";
            playerTarget.addHealth(turnRoll);
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
                tempWizard.MagicMissile(playerTarget.gameObject);
                int turnRoll = Dice.rollD("D4");
                int temp = 3 * (turnRoll + 3);
                DiceText.text = temp.ToString();
                instructions.text = "Magic Missile cast for " + temp.ToString() + " damage.";
                StartCoroutine(playerTarget.takeDamage(temp));
                spellChoice = "";
            }
            else if (spellChoice.Equals("SR"))
            {
                tempWizard.ScorchingRay(playerTarget.gameObject);
                int turnRoll = Dice.rollD("D6");
                int temp = turnRoll;
                turnRoll = Dice.rollD("D6");
                temp += turnRoll;
                temp = temp * 3;
                DiceText.text = temp.ToString();
                instructions.text = "Scorching Ray cast for " + temp.ToString() + " damage.";
                StartCoroutine(playerTarget.takeDamage(temp));
                spellChoice = "";
            }
        }
        else if (turnOrder[turnCount].tag.Equals("Cleric"))
        {
            ClericUnit tempCleric = (ClericUnit)turnOrder[turnCount];
            if (spellChoice.Equals("HW"))
            {
                tempCleric.HealingWord(playerTarget.gameObject);
                int turnRoll = Dice.rollD("D4");
                DiceText.text = turnRoll.ToString();
                instructions.text = "Healing Word cast for " + turnRoll.ToString() + " healing.";
                playerTarget.addHealth(turnRoll);
                spellChoice = "";
            }
            else if (spellChoice.Equals("A"))
            {
                tempCleric.Aid(playerTarget.gameObject);
                instructions.text = "Aid cast for 5 healing.";
                playerTarget.addHealth(5);
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
                tempWizard.MagicMissile(playerTarget.gameObject);
                int turnRoll = Dice.rollD("D4");
                int temp = 3 * (turnRoll + 3);
                DiceText.text = temp.ToString();
                instructions.text = "Magic Missile cast for " + temp.ToString() + " damage.";
                StartCoroutine(playerTarget.takeDamage(temp));
                spellChoice = "";
            }
            else if (spellChoice.Equals("SR"))
            {
                tempWizard.ScorchingRay(playerTarget.gameObject);
                int turnRoll = Dice.rollD("D6");
                int temp = turnRoll;
                turnRoll = Dice.rollD("D6");
                temp += turnRoll;
                temp = temp * 3;
                DiceText.text = temp.ToString();
                instructions.text = "Scorching Ray cast for " + temp.ToString() + " damage.";
                StartCoroutine(playerTarget.takeDamage(temp));
                spellChoice = "";
            }
        }
        else if (turnOrder[turnCount].tag.Equals("Cleric"))
        {
            ClericUnit tempCleric = (ClericUnit)turnOrder[turnCount];
            if (spellChoice.Equals("HW"))
            {
                tempCleric.HealingWord(playerTarget.gameObject);
                int turnRoll = Dice.rollD("D4");
                DiceText.text = turnRoll.ToString();
                instructions.text = "Healing Word cast for " + turnRoll.ToString() + " healing.";
                playerTarget.addHealth(turnRoll);
                spellChoice = "";
            }
            else if (spellChoice.Equals("A"))
            {
                tempCleric.Aid(playerTarget.gameObject);
                instructions.text = "Aid cast for 5 healing.";
                playerTarget.addHealth(5);
                spellChoice = "";
            }
            else if (spellChoice.Equals("MHW"))
            {
                tempCleric.MassHealingWord(playerTarget.gameObject);
                int turnRoll = Dice.rollD("D4");
                DiceText.text = turnRoll.ToString();
                instructions.text = "Mass Healing Word cast for " + turnRoll.ToString() + " healing.";
                playerTarget.addHealth(turnRoll);
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
        return path.Count*5;
    }

    List<AbstractUnit> getPlayersInRange(AbstractUnit player, List<AbstractUnit> others, int range)
    {
        List<AbstractUnit> playersInRange = new List<AbstractUnit>();;
	    for(int i = 0; i < others.Count; i++)
        {
            if(findPath(player, others[i]) <= range)
	        {
		        playersInRange.Add(others[i]);
	        }
        }	
        //prob add a light up bool or move this to update
        TileScript[] tiles = gc.getTiles();
        for (int i = 0; i < tiles.Length; i++)
        {
	        for(int j = 0; j < playersInRange.Count; j++)
            {
                if(playersInRange[j].transform.position.x == tiles[i].transform.position.x && playersInRange[j].transform.position.z == tiles[i].transform.position.z)
                {
                    tiles[i].setColor(Color.red*2); //this needs to be called in an update / be active for a longer time
                }
            }
        }
	    return playersInRange;
    }
}
