using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardUnit : AbstractUnit
{
    public GameObject fireBolt;
    public GameObject rayOfFrost;
    public Transform wandEnd;

    private static WizardUnit theWizard;

    public static WizardUnit getInstance()
    {
        return theWizard;
    }

    //List<string> damage = new List<string>(){"d4"};
    public WizardUnit() : base(75, 12, 6, "Wiz"){
    }

    // Start is called before the first frame update
    void Start()
    {
        theWizard = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.K))
        {
            takeDamage(100);
        }
    }

    public void FireBolt()
    {
        GameObject shot = Instantiate(fireBolt, wandEnd.position, wandEnd.rotation);
        //print(shot.transform.position);
        shot.GetComponent<Rigidbody>().AddForce(transform.forward * 1000);
    }

    public void RayOfFrost()
    {
        GameObject shot = Instantiate(rayOfFrost, wandEnd.position, wandEnd.rotation);
        //print(shot.transform.position);
        shot.GetComponent<Rigidbody>().AddForce(transform.forward * 1000);
    }
}