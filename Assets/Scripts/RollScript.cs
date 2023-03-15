using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RollScript : MonoBehaviour
{
   [SerializeField] TextMeshProUGUI D4Text;
   [SerializeField] TextMeshProUGUI D6Text;
   [SerializeField] TextMeshProUGUI D20Text;
   //[SerializeField] List<TextMeshProUGUI> AD;
   //[SerializeField] TextMeshProUGUI winText;
   [SerializeField] TextMeshProUGUI attackText;

//allow the player to choose move then action move move action action
  DiceRoll D20;
  DiceRoll D6;
  DiceRoll D4;
  public GameObject D4Dice;

  private void Start()
  {
   	D20 = new DiceRoll();
	D4 = new DiceRoll();
	D6 = new DiceRoll();
	D20.addDice(20);
	D6.addDice(6);
	D4.addDice(4);
  }
  public int rollD4() //abstractunits calls this function 
  {
	//roll a D4 dice 
	D4.Roll();
	//show result
	D4Text.text = D4.dice[0].rollNum.ToString();
	//return an int of the result 
	return D4.dice[0].rollNum;
  }
  public int rollD6()
  {
	D6.Roll();
	//show result
	D6Text.text = D6.dice[0].rollNum.ToString();
	//return an int of the result 
	return D6.dice[0].rollNum;
  }
  public int rollD20()
  {
	D20.Roll();
	//show result
	D20Text.text = D20.dice[0].rollNum.ToString();
	//return an int of the result 
	return D20.dice[0].rollNum;
  }

}


