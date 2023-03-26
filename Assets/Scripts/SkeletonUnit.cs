using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonUnit : AbstractUnit
{
    public GameObject A, B, arm;
    public bool activateAttack;
    private static SkeletonUnit skel;
    private int SkelLoc;

    GameObject target;
    Camera cam;

    //List<string> damage = new List<string>(){"d6", "2"};
    public SkeletonUnit() : base(13, 13, 6, "Sk", 0, 0, 0){
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
   /* public void setTile(int tileNum)
    {
        SkelLoc = tileNum;
    }
    public int getTileNum()
    {
        return SkelLoc;
    }*/
    public static SkeletonUnit getInstance()
    {
        return skel;
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
        cam.transform.rotation = Quaternion.Euler(28, 180, 0);
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
