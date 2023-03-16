using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{

    public GameObject currCharacter;
    private GameObject moveToTile;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform != null && hit.transform.tag == "Tile")
                {
                    if(!hit.transform.gameObject.GetComponent<TileScript>().getTaken())
                    {
                        moveToTile = hit.transform.gameObject;
                        print(hit.transform.name);
                    }
                }
            }
        }
        if(moveToTile != null){
             float x = moveToTile.transform.position.x;
             float z = moveToTile.transform.position.z;
            currCharacter.transform.position = new Vector3(x, 1.25f, z);
        }
    }
}
