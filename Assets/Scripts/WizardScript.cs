using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardScript : MonoBehaviour
{
    public GameObject fireBolt;
    public Transform wandEnd;
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
