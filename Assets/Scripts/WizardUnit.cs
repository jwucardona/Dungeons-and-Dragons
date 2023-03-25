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
    private int WizLoc;

    public GameObject A, B, arm, wand, sword;
    private bool activateAttack, shotFired;

    private static WizardUnit theWizard;

    private GameObject MMParent, SRParent;

    public static WizardUnit getInstance()
    {
        return theWizard;
    }
   /* public void setTile(int tileNum)
    {
        WizLoc = tileNum;
    }
    public int getTileNum()
    {
        return WizLoc;
    }*/
    //List<string> damage = new List<string>(){"d4"};
    public WizardUnit() : base(75, 12, 6, "Wiz" , 2, 2, 1){
    }

    // Start is called before the first frame update
    void Start()
    {
        theWizard = this;
    }

    public void startAttack(GameObject targetInput, Camera camInput)
    {
        target = targetInput;
        cam = camInput;
        cam.transform.position = new Vector3(transform.position.x, 4, transform.position.z - 3);
        cam.transform.rotation = Quaternion.Euler(28, 0, 0);
        activateAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.K))
        {
            takeDamage(100);
        }
        if (activateAttack)
        {
            attack();
        }
        if (shotFired)
        {
            destroyShot();
        }

        if (getSS1() == 0 && getSS2() == 0 && getSS3() == 0)
        {
            MMParent.SetActive(false);
        }
        if (getSS2() == 0 && getSS3() == 0)
        {
            SRParent.SetActive(false);
        }
    }

    public void setMMParent(GameObject input)
    {
        MMParent = input;
    }
    public void setSRParent(GameObject input)
    {
        SRParent = input;
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

        cam.transform.position = new Vector3(8, 24, 12);
        cam.transform.rotation = Quaternion.Euler(90, -90, 0);
    }

    void destroyShot()
    {
        if (shot.transform.position.x < (target.transform.position.x + 1) && shot.transform.position.x > (target.transform.position.x - 1) && shot.transform.position.z < (target.transform.position.z + 1) && shot.transform.position.z > (target.transform.position.z - 1))
        {
            cam.transform.position = new Vector3(8, 24, 12);
            cam.transform.rotation = Quaternion.Euler(90, -90, 0);
            Destroy(shot);
            shotFired = false;
        }
    }

    GameObject shot, target;
    Camera cam;
    public void FireBolt(GameObject targetInput, Camera camInput)
    {
        target = targetInput;
        cam = camInput;
        cam.transform.position = new Vector3(transform.position.x, 4, transform.position.z - 3);
        cam.transform.rotation = Quaternion.Euler(28, 0, 0);
        shot = Instantiate(fireBolt, wandEnd.position, wandEnd.rotation);
        shot.GetComponent<Rigidbody>().AddForce((target.transform.position - transform.position) * 50);
        shotFired = true;
    }

    public void RayOfFrost(GameObject targetInput, Camera camInput)
    {
        target = targetInput;
        cam = camInput;
        cam.transform.position = new Vector3(transform.position.x, 4, transform.position.z - 3);
        cam.transform.rotation = Quaternion.Euler(28, 0, 0);
        shot = Instantiate(rayOfFrost, wandEnd.position, wandEnd.rotation);
        shot.GetComponent<Rigidbody>().AddForce((target.transform.position - transform.position) * 50);
        shotFired = true;
    }

    public void MagicMissile(GameObject targetInput, Camera camInput)
    {
        target = targetInput;
        cam = camInput;
        cam.transform.position = new Vector3(transform.position.x, 4, transform.position.z - 3);
        cam.transform.rotation = Quaternion.Euler(28, 0, 0);
        shot = Instantiate(magicMissile, wandEnd.position, wandEnd.rotation);
        shot.GetComponent<Rigidbody>().AddForce((target.transform.position - transform.position) * 50);
        shotFired = true;
    }

    public void ScorchingRay(GameObject targetInput, Camera camInput)
    {
        target = targetInput;
        cam = camInput;
        cam.transform.position = new Vector3(transform.position.x, 4, transform.position.z - 3);
        cam.transform.rotation = Quaternion.Euler(28, 0, 0);
        shot = Instantiate(scorchingRay, wandEnd.position, wandEnd.rotation);
        shot.GetComponent<Rigidbody>().AddForce((target.transform.position - transform.position) * 50);
        shotFired = true;
    }

    public override void die() {
        Destroy(gameObject);
    }
}