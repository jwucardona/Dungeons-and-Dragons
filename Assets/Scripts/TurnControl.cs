using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum TurnState { start, cleric, wizard, skelHorse, skelton, win, lose}; //change states to wizard cleric skeleton etc
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

   [SerializeField] TextMeshProUGUI DiceText;
   [SerializeField] TextMeshProUGUI instructions;
   RollScript Dice;
   //5 good players
   List<WizardUnit> wiz = new List<WizardUnit>();
   List<ClericUnit> cleric = new List<ClericUnit>();
   List<SkelHorseUnit> skelHorse = new List<SkelHorseUnit>();
   List<SkeletonUnit> skel = new List<SkeletonUnit>();
   List<AbstractUnit> allUnits = new List<AbstractUnit>();
   
   public void addSkelHorse(GameObject skH)
   {

   }
   public void addCleric(GameObject cleric)
   {

   }
   public void addSkel(GameObject Sk)
   {

   }
   public void addWiz(GameObject wiz)
   {

   }
   public TurnState state;
    
    void Start()
    {
        Dice = new RollScript();
        state = TurnState.start;
       // StartCoroutine(SettupGame()); //will go to Start battle
    }
    //roll D20 for all abstract Units and sort the list to determine the order

    //names use .getType() and they are"Wiz" "SkH" "Sk" "Cle"
  /*  IEnumerator SettupGame() //coroutine aka waits until switches turns etc
    {
       //can spawn/instantiate the player and the enemy here if needed

       int playerRoll = Dice.rollD("D20"); //first rollD20
       instructions.text = "player rolls " + playerRoll;
       DiceText.text = playerRoll.ToString();

       yield return new WaitForSeconds(1f); //wait 1 second before the enemy rolls 

       int enemyRoll = Dice.rollD("D20"); //enemy rollD
       instructions.text = "enemy rolls " + enemyRoll;
       DiceText.text = enemyRoll.ToString();

       yield return new WaitForSeconds(1f);

      if(playerRoll > enemyRoll)
       {
            instructions.text = "player starts";
            yield return new WaitForSeconds(1f);
            state = TurnState.player;
            PlayerTurn();
       }
       else if(enemyRoll > playerRoll)
       {
              instructions.text = "enemy starts";
              yield return new WaitForSeconds(1f);
              state = TurnState.enemy;
              EnemyTurn();
       }
       else if(enemyRoll == playerRoll)
       {
              instructions.text = "rerolling..."; //need to add the re reroll here
       }

       //when it switches state can call the PlayerTurn() or EnemyTurn() functions and do what is needed

    }
    //probably instead of player and enemy it will be like wizard / cleric turns 

    IEnumerator PlayerAttack()
    {
        //roll the dice to see results
        int attackRoll = Dice.rollD("D20");
        DiceText.text = attackRoll.ToString();
        attackRoll+=3; //add 3 for attack
        instructions.text = "Attack Roll is " + attackRoll;
        yield return new WaitForSeconds(2f);

        //either return or yield return new WaitForSeconds and then do something else
        //can check if the enemy is dead and change to win state if necessary
        //if skeleton
    }

    void PlayerTurn() //can make this a Corountine if needed
    {
        //what happens when it is a player's turn
        DiceText.text = " "; //remove the last roll so it is blank
        instructions.text = "Player choose move or action ";

        //when done switch the state to the enemy's turn
        //state = TurnState.enemy
        //EnemyTurn() 
    }
    void EnemyTurn()
    {
        //do enemy stuff then call PlayerTurn again
    }
    public void onActionButton() //if player clicks attack button do this stuff ...
    {
        if(state != TurnState.player) //basically if action shouldn't be clicked
        {
            return; //do not do anything else
        }
        instructions.text = "Player choose to attack or cast a spell "; //ask player to attack or cast spell
    }
    public void onAttackButton()
    {
        //do whatever attack is 
    }
    public void onSpellButton()
    {
        StartCoroutine(PlayerSpell());
    }
    IEnumerator PlayerSpell()
    {
        instructions.text = "Choose spell from spell slot ";
        yield return new WaitForSeconds(2f);

    }*/
    
}
