using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkelHorseUnit : AbstractUnit
{
    public GameObject A, B, arm;
    public bool activateAttack;
    private static SkelHorseUnit shu;
    private int SkHLoc;

    GameObject target;
    Camera cam;

    //List<string> damage = new List<string>(){"d6", "d6", "4"};
    public SkelHorseUnit() : base(22, 13, 12, "SkH", 0, 0, 0){
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
   /* public void setTile(int tileNum)
    {
        SkHLoc = tileNum;
    }
    public int getTileNum()
    {
        return SkHLoc;
    }*/
    public static SkelHorseUnit getInstance()
    {
        return shu;
    }
    // Update is called once per frame
    void Update()
    {
        if (activateAttack)
        {
            attack();
        }
    }

    public void startAttack(GameObject targetInput, Camera camInput)
    {
        target = targetInput;
        cam = camInput;
        cam.transform.position = new Vector3(transform.position.x, 4, transform.position.z - 3);
        cam.transform.rotation = Quaternion.Euler(28, 0, 0);
        activateAttack = true;
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

        cam.transform.position = new Vector3(8, 24, 12);
        cam.transform.rotation = Quaternion.Euler(90, -90, 0);
    }

    public override void die() {
        Destroy(gameObject);
    }
}
