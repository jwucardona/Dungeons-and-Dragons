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
