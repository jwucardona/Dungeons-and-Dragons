using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceScript : MonoBehaviour
{
	public int sides;
	public int rollNum;

	public DiceScript(int sides)
	{
		this.sides = sides;
	}
	public void roll()
	{
		rollNum = UnityEngine.Random.Range(1, sides + 1);
	}
}
public class DiceRoll
{
	public List<DiceScript> dice;
	public DiceRoll()
	{
		dice = new List<DiceScript>();
	}
	public void addDice(int sides)
	{
		dice.Add(new DiceScript(sides));
	}
	public void Roll()
	{
		for (int i = 0; i < dice.Count; i++)
		{
			dice[i].roll();
		}
	}
	public int total()
	{
		int t = 0;
		for (int i = 0; i < dice.Count; i++)
		{
			t += dice[i].rollNum;
		}
		return t;
	}
}