using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

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

    public GameObject wizardParentButton, clericParentButton, wizSpellSlotsParent, cleSpellSlotsParent, actionParent, moveParent, SS1Parent, SS2Parent, SS3Parent, MMParent, SRParent, HWParent, MHWParent, AParent;
    string spellChoice = "";
    private bool IsrollDone = false;

    List<AbstractUnit> playersInRange = new List<AbstractUnit>();
    List<TileScript> playersInRangeTiles = new List<TileScript>();
    bool lightUp;
    TileScript tileTarget;
    AbstractUnit playerTarget;

    public Camera cam;

    int goodCount = 0;
    int badCount = 0;
    bool nextTurnText = true;
    
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
   bool PMoveDone = false;
   public void setPMove(bool pMove)
   {
      PMoveDone = pMove;
   }
   void addRandWeapon(AbstractUnit unit)
   {
       int rand = Random.Range(0, 3);
       if (rand == 0)
       {
           unit.addWeapon("Club");
       }
       else if (rand == 1)
       {
           unit.addWeapon("HandAxe");
       }
       else if (rand == 2)
       {
           unit.addWeapon("GreatClub");
       }
   }
   void addRandArmor(AbstractUnit unit)
   {
       int rand = Random.Range(0, 3);
       if (rand == 0)
       {
           unit.addArmor("Padded");
       }
       else if (rand == 1)
       {
           unit.addArmor("Studded Leather");
       }
       else if (rand == 2)
       {
           unit.addArmor("Leather");
       }
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
        if (IsrollDone)
        {
            for (int i = 0; i < turnOrder.Count; i++)
            {
                if (turnOrder[i] == null)
                {
                    turnOrder.RemoveAt(i);
                    turnCount = turnCount - 1;
                }
                if (turnOrder[i].tag.Equals("Cleric") || turnOrder[i].tag.Equals("Wiz"))
                {
                    goodCount++;
                }
                else if (turnOrder[i].tag.Equals("Skel") || turnOrder[i].tag.Equals("SkelHorse"))
                {
                    badCount++;
                }
            }
            if (goodCount == 0)
            {
                state = TurnState.lose;
                SceneManager.LoadScene("LossEnding");
            }
            else if (badCount == 0)
            {
                state = TurnState.win;
                SceneManager.LoadScene("WinEnding");
            }
            else
            {
                goodCount = 0;
                badCount = 0;
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
        if(countMoves == 1 && nextTurnText)
        {
           if( turnOrder[turnCount].tag.Equals("Cleric") || turnOrder[turnCount].tag.Equals("Wiz"))
           {
              //StartCoroutine(wait2Sec("choose next move"));
              instructions.text = "choose next move";
           }
        }
        if (countMoves >= 2)
        {
            nextTurnText = true;
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
                        instructions.text = "select a red tile and hit ENTER to activate";
                        tiles[i].setColor(Color.red * 2);
                        if (tileTarget != null)
                        {
                            tileTarget.setColor(Color.blue * 2);
                        }
                    }
                }
            }
             if (playersInRange.Count == 0)
             {
                    nextTurnText = false;
                    instructions.text = "No units in range! Select move for second turn";
                    countMoves++;
                    lightUp = false;
             }
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    if (hit.transform.tag == "Tile")
                    {
                        if (playersInRangeTiles.Contains(hit.transform.gameObject.GetComponent<TileScript>()))
                        {
                            tileTarget = hit.transform.gameObject.GetComponent<TileScript>();
                        }
                    }
                }
            }
            if (tileTarget != null && Input.GetKeyDown(KeyCode.Return))
            {
                nextTurnText = false;
                instructions.text = "Rolling for attack ";
                int turnRoll = Dice.rollD("D20");

                DiceText.text = turnRoll.ToString();

                for (int j = 0; j < playersInRange.Count; j++)
                {
                    if (playersInRange[j].transform.position.x == tileTarget.transform.position.x && playersInRange[j].transform.position.z == tileTarget.transform.position.z)
                    {
                        playerTarget = playersInRange[j];
                    }
                }
                if (turnOrder[turnCount].tag.Equals("Wiz"))
                {
                    StartCoroutine(spellText());
                    WizardUnit tempWizard = (WizardUnit)turnOrder[turnCount];
                    if (turnRoll + 3 > playerTarget.getArmorC())
                    {
                        //cast spell or attack
                        if (spellChoice.Equals("Attack"))
                        {
                            tempWizard.startAttack(playerTarget.gameObject, cam);
                            turnRoll = Dice.rollD(turnOrder[turnCount].getDamageDice());
                            DiceText.text = turnRoll.ToString();
                            instructions.text = "Melee attack for " + turnRoll.ToString() + " damage!";
                            StartCoroutine(playerTarget.takeDamage(turnRoll));
                            countMoves++;
                            StartCoroutine(nextMove());
                        }
                        else if (spellChoice.Equals("FB"))
                        {
                            tempWizard.FireBolt(playerTarget.gameObject, cam);
                            turnRoll = Dice.rollD("D10");
                            DiceText.text = turnRoll.ToString();
                            instructions.text = "FireBolt cast for " + turnRoll.ToString() + " damage.";
                            StartCoroutine(playerTarget.takeDamage(turnRoll));
                            spellChoice = "";
                            countMoves++;
                            StartCoroutine(nextMove());
                        }
                        else if (spellChoice.Equals("ROF"))
                        {
                            tempWizard.RayOfFrost(playerTarget.gameObject, cam);
                            turnRoll = Dice.rollD("D8");
                            DiceText.text = turnRoll.ToString();
                            instructions.text = "Ray Of Frost cast for " + turnRoll.ToString() + " damage.";
                            StartCoroutine(playerTarget.takeDamage(turnRoll));
                            spellChoice = "";
                            countMoves++;
                            StartCoroutine(nextMove());
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
                        nextTurnText = false;
                        instructions.text = "Attack did not penetrate armor.";
                        //StartCoroutine(wait2Sec("Attack did not penetrate armor"));
                        countMoves++;
                        StartCoroutine(nextMove());
                    }
                }
                else if (turnOrder[turnCount].tag.Equals("Cleric"))
                {
                    StartCoroutine(spellText());
                    nextTurnText = false;
                    ClericUnit tempCleric = (ClericUnit)turnOrder[turnCount];
                    if (spellChoice.Equals("Attack"))
                    {
                        if (turnRoll + 3 > playerTarget.getArmorC())
                        {
                            tempCleric.startAttack(playerTarget.gameObject, cam);
                            turnRoll = Dice.rollD(turnOrder[turnCount].getDamageDice());
                            DiceText.text = turnRoll.ToString();
                            turnRoll += Dice.rollD(turnOrder[turnCount].getDamageDice());
                            instructions.text = "Melee attack for " + turnRoll.ToString() + " damage!";
                            StartCoroutine(playerTarget.takeDamage(turnRoll));
                            countMoves++;
                            StartCoroutine(nextMove());
                        }
                        else
                        {
                            instructions.text = "Attack did not penetrate armor.";
                            //StartCoroutine(wait2Sec("Attack did not penetrate armor"));
                            countMoves++;
                            StartCoroutine(nextMove());
                        }
                    }
                    else if (turnRoll > 5)
                    {
                        //cast spell or attack
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
                        //StartCoroutine(wait2Sec("Did not roll high enough for healing spell."));
                        instructions.text = "Did not roll high enough for healing spell.";
                        countMoves++;
                        StartCoroutine(nextMove());
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
            turnDict.Add(cleric[i], turnRoll);
            addRandArmor(cleric[i]);
            addRandWeapon(cleric[i]);
            yield return new WaitForSeconds(1f);
        }
        for(int i = 0; i< wiz.Count; i++)
        {
            int turnRoll = Dice.rollD("D20");
            instructions.text = "Wizard " + i + " rolls " + turnRoll;
            DiceText.text = turnRoll.ToString();
            string whichUnit = "Wizard" + i.ToString();
            turnDict.Add(wiz[i], turnRoll);
            addRandArmor(wiz[i]);
            addRandWeapon(wiz[i]);
            yield return new WaitForSeconds(1f);
        }
        for(int i = 0; i< skel.Count; i++)
        {
            int turnRoll = Dice.rollD("D20");
            instructions.text = "Skeleton " + i + " rolls " + turnRoll;
            DiceText.text = turnRoll.ToString();
            string whichUnit = "Skeleton" + i.ToString();
            turnDict.Add(skel[i], turnRoll);
            addRandArmor(skel[i]);
            addRandWeapon(skel[i]);
            yield return new WaitForSeconds(1f);
        }
        for(int i = 0; i< skelHorse.Count; i++)
        {
            int turnRoll = Dice.rollD("D20");
            instructions.text = "SkeletonHorse " + i + " rolls " + turnRoll;
            DiceText.text = turnRoll.ToString();
            string whichUnit = "SkeletonHorse" + i.ToString();
            turnDict.Add(skelHorse[i], turnRoll);
            addRandArmor(skelHorse[i]);
            addRandWeapon(skelHorse[i]);
            yield return new WaitForSeconds(1f);
        }
        //clear dice 
        DiceText.text = " ";
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
        playersInRange.Clear();
        playersInRangeTiles.Clear();
        tileTarget = null;
        //findPath(turnOrder[0], turnOrder[1]);
        if (turnCount == turnOrder.Count) //start from beginning
        {
              turnCount = 0;
        }
        if(turnOrder[turnCount].tag.Equals("Cleric"))
        {
            actionParent.SetActive(true);
            moveParent.SetActive(true);
            state = TurnState.cleric;
            StartCoroutine(clericAction());
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
    }
    public void onMoveButton()
    {
        nextTurnText = false;
        StartCoroutine(MoveText());
    }

    IEnumerator MoveText()
    {
        instructions.text = "Select a tile and hit enter to move! ";
        yield return new WaitForSeconds(2f);
        nextTurnText = true;
    }
    IEnumerator wait2Sec(string outText)
    {
        yield return new WaitForSeconds(2f);
        instructions.text = outText;
    }
    IEnumerator spellText()
    {
        yield return new WaitForSeconds(2f);
        DiceText.text = " ";
        instructions.text = "roll is greater than armor class, select spell to cast";
    }
    IEnumerator nextMove()
    {
        yield return new WaitForSeconds(2f);
        DiceText.text = " ";
        if(countMoves == 1)
            instructions.text = "Select move for your next turn!";
    }
    IEnumerator clericAction()
    {
        //working with turnOrder[count] object -- so can call cleric methods on this object
        instructions.text = "Cleric's turn select move or action ";
        hud.UpdateHealth(turnOrder[turnCount].getHp(), turnOrder[turnCount].getMaxHp());
        hud.createStats(turnOrder[turnCount].getClass(), turnOrder[turnCount].getArmor(), turnOrder[turnCount].getWeapon(), turnOrder[turnCount].getArmorC(), turnOrder[turnCount].getMove());
        yield return new WaitForSeconds(1f);
        //do the if statements and looking at tags again to see which turn to switch to next
    }
    IEnumerator SkeletonHorseAction()
    {
        //always moves towards an enemy unless it can attack immedietly
        instructions.text = "SkeletonHorse's turn ";
        hud.UpdateHealth(turnOrder[turnCount].getHp(), turnOrder[turnCount].getMaxHp());
        hud.createStats(turnOrder[turnCount].getClass(), turnOrder[turnCount].getArmor(), turnOrder[turnCount].getWeapon(), turnOrder[turnCount].getArmorC(), turnOrder[turnCount].getMove());
        yield return new WaitForSeconds(1f);
        countMoves = 0;
        //determine if there is a player in range
        bool attackedFirst = false;
        while(countMoves < 2 && turnOrder[turnCount].tag == "SkelHorse")
        {
            SkelHorseUnit tempSkelHorse = (SkelHorseUnit)turnOrder[turnCount];
            List<AbstractUnit> enemiesToAttack = getPlayersInRange(turnOrder[turnCount], turnOrder, 10);
            //print("enemies to attack: " + enemiesToAttack.Count);
            if(enemiesToAttack.Count > 0 && !attackedFirst){
                 if(countMoves == 0)
                    instructions.text = "SkeletonHorse attacks for first turn";
                else if(countMoves == 1)
                    instructions.text = "SkeletonHorse attacks for its second turn";
                // attack first enemy in list
                // attack code here
                tempSkelHorse.startAttack(enemiesToAttack[0].gameObject, cam);
                yield return new WaitForSeconds(2f);
                instructions.text = "rolling for damage";
                yield return new WaitForSeconds(2f);
                int turnRoll = Dice.rollD(turnOrder[turnCount].getDamageDice());
                DiceText.text = turnRoll.ToString();
                turnRoll += Dice.rollD(turnOrder[turnCount].getDamageDice());
                turnRoll += 4;
                instructions.text = "Melee attack for " + turnRoll.ToString() + " damage!";
                StartCoroutine(enemiesToAttack[0].takeDamage(turnRoll));
                yield return new WaitForSeconds(3f);
                DiceText.text = " ";
                attackedFirst = true;
                countMoves++;
            }
            else {
                if(countMoves == 0)
                    instructions.text = "SkeletonHorse moves for first turn";
                else if(countMoves == 1)
                    instructions.text = "SkeletonHorse moves for its second turn";
                PlayerMover.startMovement();
                yield return new WaitForSeconds(6f);
                //don't need countMoves++ because it is already counted in playermover
            }
        }

        //countMoves = 2;
        //switchTurn();
    }
    IEnumerator SkeletonAction()
    {
        instructions.text = "Skeleton's turn ";
        hud.UpdateHealth(turnOrder[turnCount].getHp(), turnOrder[turnCount].getMaxHp());
        hud.createStats(turnOrder[turnCount].getClass(), turnOrder[turnCount].getArmor(), turnOrder[turnCount].getWeapon(), turnOrder[turnCount].getArmorC(), turnOrder[turnCount].getMove());
        yield return new WaitForSeconds(1f);
        countMoves = 0;
        // need to fix the attacking
        // for some reason it always attacks first
        // this could be because in the enemiesToAttack list, the first enemy is the player
        bool attackedFirst = false;
        while(countMoves < 2 && turnOrder[turnCount].tag == "Skel"){
            SkeletonUnit tempSkel = (SkeletonUnit)turnOrder[turnCount];
            List<AbstractUnit> enemiesToAttack = getPlayersInRange(turnOrder[turnCount], turnOrder, 10);
            //print("enemies to attack: " + enemiesToAttack.Count);
            if(enemiesToAttack.Count > 0 && !attackedFirst){
                if(countMoves == 0)
                    instructions.text = "Skeleton attacks for first turn";
                else if(countMoves == 1)
                    instructions.text = "Skeleton attacks for its second turn";
                // attack first enemy in list
                // attack code here
                yield return new WaitForSeconds(2f);
                tempSkel.startAttack(enemiesToAttack[0].gameObject, cam);
                instructions.text = "rolling for damage!";
                int turnRoll = Dice.rollD(turnOrder[turnCount].getDamageDice());
                DiceText.text = turnRoll.ToString();
                turnRoll += 2;
                yield return new WaitForSeconds(2f);
                instructions.text = "Melee attack for " + turnRoll.ToString() + " damage!";
                StartCoroutine(enemiesToAttack[0].takeDamage(turnRoll));
                yield return new WaitForSeconds(3f);
                attackedFirst = true;
                DiceText.text = " ";
                countMoves++;
            }
            else {
                if(countMoves == 0)
                    instructions.text = "Skeleton moves for first turn";
                else if(countMoves == 1)
                    instructions.text = "Skeleton moves for its second turn";
                PlayerMover.startMovement();
                yield return new WaitForSeconds(6f);
                //don't need countMoves++ because it is already counted in playermover
            }
        }
    }
    IEnumerator WizardAction()
    {
        instructions.text = "Wizard's turn select move or action ";
        hud.UpdateHealth(turnOrder[turnCount].getHp(), turnOrder[turnCount].getMaxHp());
        hud.createStats(turnOrder[turnCount].getClass(), turnOrder[turnCount].getArmor(), turnOrder[turnCount].getWeapon(), turnOrder[turnCount].getArmorC(), turnOrder[turnCount].getMove());
        yield return new WaitForSeconds(1f);
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
            tempWizard.MagicMissile(playerTarget.gameObject, cam);
            int turnRoll = Dice.rollD("D4");
            int temp = (turnRoll + 3);
            turnRoll = Dice.rollD("D4");
            temp += (turnRoll + 3);
            turnRoll = Dice.rollD("D4");
            temp += (turnRoll + 3);
            DiceText.text = turnRoll.ToString();
            instructions.text = "Magic Missile cast for " + temp.ToString() + " damage.";
            StartCoroutine(playerTarget.takeDamage(temp));
            spellChoice = "";
            StartCoroutine(nextMove());
        }
        else if (turnOrder[turnCount].tag.Equals("Cleric"))
        {
            ClericUnit tempCleric = (ClericUnit)turnOrder[turnCount];
            tempCleric.HealingWord(playerTarget.gameObject, cam);
            int turnRoll = Dice.rollD("D4");
            DiceText.text = turnRoll.ToString();
            instructions.text = "Healing Word cast for " + turnRoll.ToString() + " healing.";
            playerTarget.addHealth(turnRoll);
            StartCoroutine(nextMove());
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
                tempWizard.MagicMissile(playerTarget.gameObject, cam);
                int turnRoll = Dice.rollD("D4");
                int temp = (turnRoll + 3);
                turnRoll = Dice.rollD("D4");
                temp += (turnRoll + 3);
                turnRoll = Dice.rollD("D4");
                temp += (turnRoll + 3);
                DiceText.text = turnRoll.ToString();
                instructions.text = "Magic Missile cast for " + temp.ToString() + " damage.";
                StartCoroutine(playerTarget.takeDamage(temp));
                spellChoice = "";
                StartCoroutine(nextMove());
            }
            else if (spellChoice.Equals("SR"))
            {
                tempWizard.ScorchingRay(playerTarget.gameObject, cam);
                int turnRoll = Dice.rollD("D6");
                int temp = turnRoll;
                turnRoll = Dice.rollD("D6");
                temp += turnRoll;
                turnRoll = Dice.rollD("D6");
                temp += turnRoll;
                turnRoll = Dice.rollD("D6");
                temp += turnRoll;
                turnRoll = Dice.rollD("D6");
                temp += turnRoll;
                turnRoll = Dice.rollD("D6");
                temp += turnRoll;
                DiceText.text = turnRoll.ToString();
                instructions.text = "Scorching Ray cast for " + temp.ToString() + " damage.";
                StartCoroutine(playerTarget.takeDamage(temp));
                spellChoice = "";
                StartCoroutine(nextMove());
            }
        }
        else if (turnOrder[turnCount].tag.Equals("Cleric"))
        {
            ClericUnit tempCleric = (ClericUnit)turnOrder[turnCount];
            if (spellChoice.Equals("HW"))
            {
                tempCleric.HealingWord(playerTarget.gameObject, cam);
                int turnRoll = Dice.rollD("D4");
                DiceText.text = turnRoll.ToString();
                instructions.text = "Healing Word cast for " + turnRoll.ToString() + " healing.";
                playerTarget.addHealth(turnRoll);
                spellChoice = "";
                StartCoroutine(nextMove());
            }
            else if (spellChoice.Equals("A"))
            {
                tempCleric.Aid(playerTarget.gameObject, cam);
                instructions.text = "Aid cast for 5 healing.";
                playerTarget.addHealth(5);
                spellChoice = "";
                StartCoroutine(nextMove());
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
                tempWizard.MagicMissile(playerTarget.gameObject, cam);
                int turnRoll = Dice.rollD("D4");
                int temp = (turnRoll + 3);
                turnRoll = Dice.rollD("D4");
                temp += (turnRoll + 3);
                turnRoll = Dice.rollD("D4");
                temp += (turnRoll + 3);
                DiceText.text = turnRoll.ToString();
                instructions.text = "Magic Missile cast for " + temp.ToString() + " damage.";
                StartCoroutine(playerTarget.takeDamage(temp));
                spellChoice = "";
                StartCoroutine(nextMove());
            }
            else if (spellChoice.Equals("SR"))
            {
                tempWizard.ScorchingRay(playerTarget.gameObject, cam);
                int turnRoll = Dice.rollD("D6");
                int temp = turnRoll;
                turnRoll = Dice.rollD("D6");
                temp += turnRoll;
                turnRoll = Dice.rollD("D6");
                temp += turnRoll;
                turnRoll = Dice.rollD("D6");
                temp += turnRoll;
                turnRoll = Dice.rollD("D6");
                temp += turnRoll;
                turnRoll = Dice.rollD("D6");
                temp += turnRoll;
                DiceText.text = turnRoll.ToString();
                instructions.text = "Scorching Ray cast for " + temp.ToString() + " damage.";
                StartCoroutine(playerTarget.takeDamage(temp));
                spellChoice = "";
                StartCoroutine(nextMove());
            }
        }
        else if (turnOrder[turnCount].tag.Equals("Cleric"))
        {
            ClericUnit tempCleric = (ClericUnit)turnOrder[turnCount];
            if (spellChoice.Equals("HW"))
            {
                tempCleric.HealingWord(playerTarget.gameObject, cam);
                int turnRoll = Dice.rollD("D4");
                DiceText.text = turnRoll.ToString();
                instructions.text = "Healing Word cast for " + turnRoll.ToString() + " healing.";
                playerTarget.addHealth(turnRoll);
                spellChoice = "";
                StartCoroutine(nextMove());
            }
            else if (spellChoice.Equals("A"))
            {
                tempCleric.Aid(playerTarget.gameObject, cam);
                instructions.text = "Aid cast for 5 healing.";
                playerTarget.addHealth(5);
                spellChoice = "";
                StartCoroutine(nextMove());
            }
            else if (spellChoice.Equals("MHW"))
            {
                tempCleric.MassHealingWord(playerTarget.gameObject, cam);
                int turnRoll = Dice.rollD("D4");
                DiceText.text = turnRoll.ToString();
                instructions.text = "Mass Healing Word cast for " + turnRoll.ToString() + " healing.";
                for (int i = 0; i < playersInRange.Count; i++)
                {
                    playersInRange[i].addHealth(turnRoll);
                }
                spellChoice = "";
                StartCoroutine(nextMove());
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
            wizardParentButton.SetActive(true);

            if (turnOrder[turnCount].getSS1() == 0 && turnOrder[turnCount].getSS2() == 0 && turnOrder[turnCount].getSS3() == 0)
            {
                MMParent.SetActive(false);
            }
            if (turnOrder[turnCount].getSS2() == 0 && turnOrder[turnCount].getSS3() == 0)
            {
                SRParent.SetActive(false);
            }
        }
        else if (turnOrder[turnCount].tag.Equals("Cleric"))
        {
            clericParentButton.SetActive(true);
            if (turnOrder[turnCount].getSS1() == 0 && turnOrder[turnCount].getSS2() == 0 && turnOrder[turnCount].getSS3() == 0)
            {
                HWParent.SetActive(false);
            }
            if (turnOrder[turnCount].getSS2() == 0 && turnOrder[turnCount].getSS3() == 0)
            {
                AParent.SetActive(false);
            }
            if (turnOrder[turnCount].getSS3() == 0)
            {
                MHWParent.SetActive(false);
            }
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
            }
            if(p2.transform.position.x == tile[i].transform.position.x && p2.transform.position.z == tile[i].transform.position.z)
            {
                end = tile[i];
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
        
        return path.Count*5;
    }

    List<AbstractUnit> getPlayersInRange(AbstractUnit player, List<AbstractUnit> others, int range)
    {
        List<AbstractUnit> playersInRange = new List<AbstractUnit>();;
	    for(int i = 0; i < others.Count; i++)
        {
            if(findPath(player, others[i]) <= range && others[i] != player)
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
