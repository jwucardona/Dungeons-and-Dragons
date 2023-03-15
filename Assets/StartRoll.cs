using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartRoll : MonoBehaviour
{

  [SerializeField] List<TextMeshProUGUI> texts;
  [SerializeField] TextMeshProUGUI winText;

  DiceRoll diceRoll;
  public GameObject startTxt;
  public GameObject d1;
  public GameObject d2;
  public GameObject startButton;	
  private GameControllerScript gcs = new GameControllerScript();

  private void Start()
  {
  	diceRoll = new DiceRoll();
	for(int i = 0; i < 2; i++)
	{
		diceRoll.addDice(20); //adding 5 6 sided dice can change
	}
  }
  public void RollStart()
  {
	diceRoll.Roll();
	UpdateText();
  }
  private void UpdateText()
  {
		for(int i = 0; i < texts.Count; i++)
		{
			if(diceRoll.dice.Count > 0)
			{
				texts[i].text = diceRoll.dice[i].rollNum.ToString();
			}
		}
		if(diceRoll.dice[0].rollNum > diceRoll.dice[1].rollNum)
		{
			winText.text = "Player starts";
			gcs.setPlayer(1);
			Destroy(startTxt, 2.0f);
			Destroy(startButton, 2.0f);
		    Destroy(startButton, 2.0f);
			Destroy(d1, 2.0f);
			Destroy(d2, 2.0f);
		}
		else if(diceRoll.dice[0].rollNum < diceRoll.dice[1].rollNum)
		{
			winText.text = "enemy starts";
			gcs.setPlayer(2);
			Destroy(startTxt, 2.0f);
			Destroy(startButton, 2.0f);
			Destroy(d1, 2.0f);
			Destroy(d2, 2.0f);
		}
		else if(diceRoll.dice[0].rollNum == diceRoll.dice[1].rollNum)
		{
			winText.text = "Tie re roll";
		}
		
  }
}