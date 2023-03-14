using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkelHorseUnit : AbstractUnit
{
    public GameObject A, B, arm;
    public bool activateAttack;

    //List<string> damage = new List<string>(){"d6", "d6", "4"};
    public SkelHorseUnit() : base(22, 13, 12, "SkH"){
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.T))
        {
            activateAttack = true;
        }
        if (activateAttack)
        {
            attack();
        }
    }

    void attack()
    {
        float time = Mathf.PingPong(Time.time * 2f, 1);
        arm.transform.rotation = Quaternion.Lerp(A.transform.rotation, B.transform.rotation, time);

        StartCoroutine(resetCoroutine());

    }

    IEnumerator resetCoroutine()
    {
        yield return new WaitForSeconds(1f);
        activateAttack = false;
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }
}
