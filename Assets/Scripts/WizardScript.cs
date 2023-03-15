using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WizardScript : MonoBehaviour
{
    public GameObject fireBolt;
    public Transform wandEnd;
    [SerializeField] TextMeshProUGUI attackText;
    [SerializeField] List<TextMeshProUGUI> AD;
    DiceRoll attackRoll;

    public GameObject wizResult;
    public GameObject wizDice;
    public GameObject wizRollBut;

   // private GameControllerScript gs = new GameControllerScript();

    // Start is called before the first frame update
    void Start()
    {
    }
   /* public void wizAttack()
    {
        attackRoll = new DiceRoll();
		attackRoll.addDice(20); //add a D20
		attackRoll.Roll();
		UpdateText();
        int rollResult = attackRoll.dice[0].rollNum + 3; 
		if(rollResult > 12 ) //greater than whatever the amor class is
        {
           attackText.text = "rolled a " + rollResult + " and hit";
        }
		else
        {
            attackText.text = "rolled a " + rollResult + " and didn't hit";
        }
    }
  private void UpdateText()
  {
		for(int i = 0; i < AD.Count; i++)
		{
			if(i < attackRoll.dice.Count)
			{
				AD[i].text = attackRoll.dice[i].rollNum.ToString();
			}
		}
         //Destroy(wizDice, 2.0f);
         //Destroy(wizResult,2.0f);
         //Destroy(wizRollBut, 2.0f);
  }*/
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.T))
        {
            print("shooting fire bolt" + wandEnd.position + wandEnd.rotation);
            GameObject shot = Instantiate(fireBolt, wandEnd.position, wandEnd.rotation);
            shot.GetComponent<Rigidbody>().AddForce(transform.forward * 1000);
        }
    }
}
