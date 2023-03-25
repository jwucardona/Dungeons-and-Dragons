using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClericUnit : AbstractUnit
{
    public GameObject healingWord;
    public GameObject massHealingWord;
    public GameObject aid;
    public Transform staffEnd;
    private int ClerLoc;

    public GameObject A, B, arm, sword1, sword2;
    private bool activateAttack, shotFired;

    private static ClericUnit theCleric;

    public static ClericUnit getInstance()
    {
        return theCleric;
    }
    //List<string> damage = new List<string>(){"d6", "d6"};
    public ClericUnit() : base(60, 10, 5, "Cle", 3, 2, 1){
    }
    
    // Start is called before the first frame update
    void Start()
    {
        theCleric = this;
    }

    public void startAttack(GameObject targetInput, Camera camInput)
    {
        target = targetInput;
        cam = camInput;
        activateAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
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
        cam.transform.position = new Vector3(transform.position.x, 4, transform.position.z - 3);
        cam.transform.rotation = Quaternion.Euler(28, 0, 0);
        sword1.SetActive(false);
        sword2.SetActive(true);
        float time = Mathf.PingPong(Time.time * 2f, 1);
        arm.transform.rotation = Quaternion.Lerp(A.transform.rotation, B.transform.rotation, time);

        StartCoroutine(resetCoroutine());

        cam.transform.position = new Vector3(8, 24, 12);
        cam.transform.rotation = Quaternion.Euler(90, -90, 0);
    }

    IEnumerator resetCoroutine()
    {
        yield return new WaitForSeconds(1f);
        activateAttack = false;
        sword1.SetActive(true);
        sword2.SetActive(false);
        transform.rotation = new Quaternion(0, 0, 0, 0);
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
    public void HealingWord(GameObject targetInput, Camera camInput)
    {
        target = targetInput;
        cam = camInput;
        cam.transform.position = new Vector3(transform.position.x, 4, transform.position.z - 3);
        cam.transform.rotation = Quaternion.Euler(28, 0, 0);
        shot = Instantiate(healingWord, staffEnd.position, staffEnd.rotation);
        shot.GetComponent<Rigidbody>().AddForce((target.transform.position - transform.position) * 50);
        shotFired = true;
    }

    public void MassHealingWord(GameObject targetInput, Camera camInput)
    {
        target = targetInput;
        cam = camInput;
        cam.transform.position = new Vector3(transform.position.x, 4, transform.position.z - 3);
        cam.transform.rotation = Quaternion.Euler(28, 0, 0);
        shot = Instantiate(massHealingWord, staffEnd.position, staffEnd.rotation);
        shot.GetComponent<Rigidbody>().AddForce((target.transform.position - transform.position) * 50);
        shotFired = true;
    }

    public void Aid(GameObject targetInput, Camera camInput)
    {
        target = targetInput;
        cam = camInput;
        cam.transform.position = new Vector3(transform.position.x, 4, transform.position.z - 3);
        cam.transform.rotation = Quaternion.Euler(28, 0, 0);
        shot = Instantiate(aid, staffEnd.position, staffEnd.rotation);
        shot.GetComponent<Rigidbody>().AddForce((target.transform.position - transform.position) * 50);
        shotFired = true;
    }

    public override void die() {
        Destroy(gameObject);
    }
}
