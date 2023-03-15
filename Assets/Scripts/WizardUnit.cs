using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardUnit : AbstractUnit
{
    public GameObject fireBolt;
    public GameObject rayOfFrost;
    public GameObject magicMissile;
    public GameObject scorchingRay;
    public Transform wandEnd;

    public GameObject A, B, arm, target, wand, sword;
    private bool activateAttack, shotFired;

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
        if (Input.GetKey(KeyCode.T))
        {
            activateAttack = true;
        }
        if (activateAttack)
        {
            attack();
        }
        if (shotFired)
        {
            destroyShot();
        }
    }

    void attack()
    {
        wand.SetActive(false);
        sword.SetActive(true);
        float time = Mathf.PingPong(Time.time * 2f, 1);
        arm.transform.rotation = Quaternion.Lerp(A.transform.rotation, B.transform.rotation, time);

        StartCoroutine(resetCoroutine());

    }

    IEnumerator resetCoroutine()
    {
        yield return new WaitForSeconds(1f);
        activateAttack = false;
        wand.SetActive(true);
        sword.SetActive(false);
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    void destroyShot()
    {
        if (shot.transform.position.x < (target.transform.position.x + 1) && shot.transform.position.x > (target.transform.position.x - 1) && shot.transform.position.z < (target.transform.position.z + 1) && shot.transform.position.z > (target.transform.position.z - 1))
        {
            Destroy(shot);
        }
    }

    GameObject shot;
    public void FireBolt()
    {
        shot = Instantiate(fireBolt, wandEnd.position, wandEnd.rotation);
        shot.GetComponent<Rigidbody>().AddForce((target.transform.position - transform.position) * 50);
        shotFired = true;
    }

    public void RayOfFrost()
    {
        shot = Instantiate(rayOfFrost, wandEnd.position, wandEnd.rotation);
        shot.GetComponent<Rigidbody>().AddForce((target.transform.position - transform.position) * 50);
        shotFired = true;
    }

    public void MagicMissile()
    {
        shot = Instantiate(magicMissile, wandEnd.position, wandEnd.rotation);
        shot.GetComponent<Rigidbody>().AddForce((target.transform.position - transform.position) * 50);
        shotFired = true;
    }

    public void ScorchingRay()
    {
        shot = Instantiate(scorchingRay, wandEnd.position, wandEnd.rotation);
        shot.GetComponent<Rigidbody>().AddForce((target.transform.position - transform.position) * 50);
        shotFired = true;
    }
}