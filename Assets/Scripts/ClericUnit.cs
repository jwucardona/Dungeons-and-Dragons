using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClericUnit : AbstractUnit
{
    public GameObject healingWord;
    public GameObject massHealingWord;
    public GameObject aid;
    public Transform staffEnd;
    TileScript ClerLoc;

    public GameObject A, B, arm, target, sword1, sword2;
    private bool activateAttack, shotFired;

    private static ClericUnit theCleric;

    private int ss1, ss2, ss3;

    public static ClericUnit getInstance()
    {
        return theCleric;
    }
    public void setTile(TileScript tile)
    {
        ClerLoc = tile;
    }
    public TileScript getTile()
    {
        return ClerLoc;
    }
    //List<string> damage = new List<string>(){"d6", "d6"};
    public ClericUnit() : base(60, 10, 5, "Cle"){
    }
    
    // Start is called before the first frame update
    void Start()
    {
        theCleric = this;
        ss1 = 3;
        ss2 = 2;
        ss3 = 1;
    }

    public int getSS1()
    {
        return ss1;
    }
    public void setSS1(int input)
    {
        ss1 = input;
    }

    public int getSS2()
    {
        return ss2;
    }
    public void setSS2(int input)
    {
        ss2 = input;
    }

    public int getSS3()
    {
        return ss3;
    }
    public void setSS3(int input)
    {
        ss3 = input;
    }

    public void startAttack()
    {
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
        sword1.SetActive(false);
        sword2.SetActive(true);
        float time = Mathf.PingPong(Time.time * 2f, 1);
        arm.transform.rotation = Quaternion.Lerp(A.transform.rotation, B.transform.rotation, time);

        StartCoroutine(resetCoroutine());

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
            Destroy(shot);
            shotFired = false;
        }
    }

    GameObject shot;
    public void HealingWord()
    {
        shot = Instantiate(healingWord, staffEnd.position, staffEnd.rotation);
        shot.GetComponent<Rigidbody>().AddForce((target.transform.position - transform.position) * 50);
        shotFired = true;
    }

    public void MassHealingWord()
    {
        shot = Instantiate(massHealingWord, staffEnd.position, staffEnd.rotation);
        shot.GetComponent<Rigidbody>().AddForce((target.transform.position - transform.position) * 50);
        shotFired = true;
    }

    public void Aid()
    {
        shot = Instantiate(aid, staffEnd.position, staffEnd.rotation);
        shot.GetComponent<Rigidbody>().AddForce((target.transform.position - transform.position) * 50);
        shotFired = true;
    }

    public override void die() {
        Destroy(gameObject);
    }
}
