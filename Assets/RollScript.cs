/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RollScript : MonoBehaviour
{
   [SerializeField] List<TextMeshProUGUI> texts;

  DiceRoll diceRoll;
  private void Start()
  {
		diceRoll = new DiceRoll();
		for(int i = 0; i < 2 i++)
		{
			diceRoll.addDice(20); //2 D20s
		}
  }
  public void Roll()
  {
	diceRoll.Roll();
	UpdateText();
  }
  private void UpdateText()
  {
		for(int i = 0; i < texts.Count; i++)
		{
			if(i < diceRoll.dice.Count)
			{
				texts[i].text = diceRoll.dice[i].rollNum.ToString();
			}
		}
  }
}*/
