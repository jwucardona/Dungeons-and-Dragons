using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RollScript : MonoBehaviour
{
//allow the player to choose move then action move move action action
	DiceRoll D20;
	DiceRoll D8;
	DiceRoll D6;
	DiceRoll D4;
	DiceRoll D10;
	DiceRoll D20start;
	public GameObject D4Dice;

  private void Start()
  {
  
  }
  public int rollD(string which) //abstractunits calls this function 
  {
	//roll a D4 dice 
	if(which.Equals("D4"))
	{
		D4 = new DiceRoll();
		D4.addDice(4);
		D4.Roll();
		return D4.dice[0].rollNum;
	}
	else if(which.Equals("D20"))
	{
		D20 = new DiceRoll();
		D20.addDice(20);
		D20.Roll();
		return D20.dice[0].rollNum;
	}
	else if(which.Equals("D6"))
	{
		D6 = new DiceRoll();
		D6.addDice(6);
		D6.Roll();
		return D6.dice[0].rollNum;
	}
	else if(which.Equals("D8"))
	{
		D8 = new DiceRoll();
		D8.addDice(8);
		D8.Roll();
		return D8.dice[0].rollNum;
	}
	else if(which.Equals("D10"))
	{
		D10 = new DiceRoll();
		D10.addDice(10);
		D10.Roll();
		return D10.dice[0].rollNum;
	}
	return -1;
  }


}


