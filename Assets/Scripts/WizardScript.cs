using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardScript : MonoBehaviour
{
    public GameObject fireBolt;
    public GameObject rayOfFrost;
    public Transform wandEnd;

    private static WizardScript theWizard;

    public static WizardScript getInstance()
    {
        return theWizard;
    }

    // Start is called before the first frame update
    void Start()
    {
        theWizard = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FireBolt()
    {
        GameObject shot = Instantiate(fireBolt, wandEnd.position, wandEnd.rotation);
        print(shot.transform.position);
        shot.GetComponent<Rigidbody>().AddForce(transform.forward * 1000);
    }

    public void RayOfFrost()
    {
        GameObject shot = Instantiate(rayOfFrost, wandEnd.position, wandEnd.rotation);
        print(shot.transform.position);
        shot.GetComponent<Rigidbody>().AddForce(transform.forward * 1000);
    }
}
