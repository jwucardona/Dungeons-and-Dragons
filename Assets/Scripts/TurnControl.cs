using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum TurnState { start, player, enemy, win, lose};

public class TurnControl : MonoBehaviour
{
    // Start is called before the first frame update
    //holds player wizard and cleric GameObjects

   [SerializeField] TextMeshProUGUI DiceText;
   [SerializeField] TextMeshProUGUI instructions;
   RollScript Dice;

   public TurnState state;
    
    void Start()
    {
        Dice = new RollScript();
        state = TurnState.start;
        StartCoroutine(SettupGame()); //will go to Start battle
    }
    IEnumerator SettupGame() //coroutine aka waits until switches turns etc
    {
       //can spawn/instantiate the player and the enemy here if needed

       int playerRoll = Dice.rollD("D20"); //first rollD20
       instructions.text = "player rolls " + playerRoll;
       DiceText.text = playerRoll.ToString();

       yield return new WaitForSeconds(2f); //wait 1 second before the enemy rolls 

       int enemyRoll = Dice.rollD("D20"); //enemy rollD
       instructions.text = "enemy rolls " + enemyRoll;
       DiceText.text = enemyRoll.ToString();

       yield return new WaitForSeconds(2f);

      if(playerRoll > enemyRoll)
       {
            instructions.text = "player starts";
            yield return new WaitForSeconds(2f);
            state = TurnState.player;
            PlayerTurn();
       }
       else if(enemyRoll > playerRoll)
       {
              instructions.text = "enemy starts";
              yield return new WaitForSeconds(2f);
              state = TurnState.enemy;
              EnemyTurn();
       }

       //when it switches state can call the PlayerTurn() or EnemyTurn() functions and do what is needed

    }
    //probably instead of player and enemy it will be like wizard / cleric turns 

    void PlayerTurn() //can make this a Corountine if needed
    {
        //what happens when it is a player's turn
        instructions.text = "Player choose move or attack ";
        //when done switch the state to the enemy's turn
        //state = TurnState.enemy
        //EnemyTurn() 
    }
    void EnemyTurn()
    {
        //do enemy stuff then call PlayerTurn again
    }
    public void onAttackButton() //if player clicks attack button do this stuff ...
    {

    }
    
}
