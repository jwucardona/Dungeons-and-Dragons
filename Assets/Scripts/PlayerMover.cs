using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    private GameControllerScript gc;

    public GameObject currCharacter;
    private TileScript currTile;
    private TileScript startTile;

    private TileScript[] tilesCopy;

    private GameObject moveToTile;
    private static bool startMoving;
    private static bool startChoosing;
    private bool isEnemy;

    private static int counter = 1;
    private Material newColor;

    private List<TileScript> possibleMoves =  new List<TileScript>();

    System.Random rnd = new System.Random();
    public TurnControl turnControl;

    // Start is called before the first frame update
    void Start()
    {
        gc = GameControllerScript.getInstance();
        startMoving = false;
        startChoosing = false;
    }

    // Update is called once per frame
    void Update()
    {
        tilesCopy = gc.getTiles();

        if (turnControl.rollDone())
        {
            currCharacter = turnControl.getCurrentPlayer();
            for (int i = 0; i < tilesCopy.Length; i++)
            {
                tilesCopy[i].setColor(Color.white);
                if (currCharacter.transform.position.x == tilesCopy[i].transform.position.x && currCharacter.transform.position.z == tilesCopy[i].transform.position.z)
                {
                    currTile = tilesCopy[i];
                }
            }
        }
        
        if(startChoosing){


            currCharacter = turnControl.getCurrentPlayer();

            //get the current tile that the game object is standing on



            // add if enemy or if ally based on tag
            if(currCharacter.tag == "Skel" || currCharacter.tag == "SkelHorse"){
                isEnemy = true;
            } else {
                isEnemy = false;

            }

            //get the current tile that the game object is standing on
            for (int i = 0; i<tilesCopy.Length; i++){
                tilesCopy[i].setColor(Color.white);
                if(currCharacter.transform.position.x == tilesCopy[i].transform.position.x && currCharacter.transform.position.z == tilesCopy[i].transform.position.z){
                    currTile = tilesCopy[i];
                    //save starting tile so it can be set to "not taken" later
                    startTile = currTile;
                }
            }


            // create random move(select random tile) for enemies
            // check if tile is taken
            // set that tile to moveToTile
            // startMoving = true;
            if (isEnemy) {
                possibleMoves.Clear();
                possibleMoves = RangeCalculator.calculateRange(currTile, currCharacter.GetComponent<AbstractUnit>().getMove()-1);
                int rand = 0;
                while(true){
                    rand = rnd.Next(0, possibleMoves.Count - 1);
                    if (!possibleMoves[rand].getTaken())
                    {
                        break;
                    }
                    
                }
                moveToTile = possibleMoves[rand].gameObject;
                newColor = moveToTile.transform.gameObject.GetComponent<Renderer>().material;
                startMoving = true;
                startChoosing = false;
                movePlayer();
            }

            //if choosing has commenced?!!
            if(!isEnemy){
                possibleMoves.Clear();
                possibleMoves = RangeCalculator.calculateRange(currTile, currCharacter.GetComponent<AbstractUnit>().getMove()-1);
                for(int i=0; i<possibleMoves.Count ; i++){
                    possibleMoves[i].setColor(Color.blue * 2);
                }

                getMoveToTile(); // this line is the issue
            
                if(moveToTile != null){
                    newColor.color = Color.blue;
                    if(Input.GetKeyDown(KeyCode.Return)){
                        startMoving = true;
                        startChoosing = false;
                    }
                }
            }
        }
        //if moving has commenced !!! :0
        if(startMoving == true){
            gc.computerPath(currTile.GetComponent<TileScript>(), moveToTile.GetComponent<TileScript>());
            newColor.color = Color.gray;
            movePlayer();
        }

    }

    public static void startMovement()
    {
        startChoosing = true;
    }


    void getMoveToTile(){
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform != null && hit.transform.tag == "Tile")
                {
                    if(!hit.transform.gameObject.GetComponent<TileScript>().getTaken() && possibleMoves.Contains(hit.transform.gameObject.GetComponent<TileScript>()))
                    {
                        if(moveToTile != null){ //change color back to gray
                            newColor.color = Color.gray;
                        }
                        moveToTile = hit.transform.gameObject;
                        newColor = moveToTile.transform.gameObject.GetComponent<Renderer>().material;
                    }
                }
            }
        }
    }


    void calculateAllMoves(){
       //need to account for tiles ovr gapssssssss
       possibleMoves.Clear();
        for (int i = 0; i < tilesCopy.Length; i++)
        {
           if (Vector3.Distance(currTile.transform.position, tilesCopy[i].transform.position) < currCharacter.GetComponent<AbstractUnit>().getMove()) {
                possibleMoves.Add(tilesCopy[i]);
           }
        }
        for(int i = 0; i < possibleMoves.Count ; i++){
            gc.computerPath(currTile, possibleMoves[i]);
        } 
    }

    void movePlayer(){
        float speed = 5;
        List<TileScript> myPath = gc.getPath();
        if(counter < myPath.Count){
            float x = myPath[counter].transform.position.x;
            float z = myPath[counter].transform.position.z;
            currCharacter.transform.position = Vector3.MoveTowards(currCharacter.transform.position, new Vector3(x, 1.25f, z), speed * Time.deltaTime);
            if(currTile.transform.position == myPath[counter].transform.position){
                counter++;
            }
        }

        if(currTile.transform.position == moveToTile.transform.position){
            //setting taken status of both tiles
            moveToTile.GetComponent<TileScript>().setTaken(true);
            startTile.GetComponent<TileScript>().setTaken(false);

            startMoving = false;
            startChoosing = false;
            moveToTile = null;
            startTile = null;

            //turnControl.setInstructions();
            turnControl.setCountMoves(1);
            //this is where turn in considered over... we can call to a function in
            //turn control to continue the game?
        }
    }

}
