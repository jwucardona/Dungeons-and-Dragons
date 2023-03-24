using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameControllerScript : MonoBehaviour
{
    TileScript[] tiles;

    private TileScript start, end;

    public GameObject wizardPrefab;
    public GameObject clericPrefab;

    public GameObject skeletonPrefab;
    public GameObject warhorsePrefab;

    public GameObject wizardParentButton;
    public GameObject clericParentButton;
    public GameObject wizSpellSlotsParent;
    public GameObject cleSpellSlotsParent;

    public List<TileScript> temp = new List<TileScript>();
    public List<TileScript> path = new List<TileScript>();
    public TurnControl tc = new TurnControl();
    
    private static GameControllerScript theGameController;

    public static GameControllerScript getInstance()
    {
        return theGameController;
    }

System.Random rnd = new System.Random();

    List<GameObject> enemyPrefabList = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {

        theGameController = this;
        
        wizardParentButton.SetActive(false);
        clericParentButton.SetActive(false);
        wizSpellSlotsParent.SetActive(false);
        cleSpellSlotsParent.SetActive(false);


        tiles = FindObjectsOfType<TileScript>();
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = i + 1; j < tiles.Length; j++)
            {
                if (Vector3.Distance(tiles[i].transform.position, tiles[j].transform.position) < 2.1f) {
                    tiles[i].getNeighbors().Add(tiles[j]);
                    tiles[j].getNeighbors().Add(tiles[i]);
                }
            }
        }

        //fill enemy prefab list for random selection
        enemyPrefabList.Add(skeletonPrefab);
        enemyPrefabList.Add(warhorsePrefab);

        // code to instantiate wizards on run
        int howManyWizards = CharacterMenuScript.getWizardInput();
        
        for(int i = 0; i < howManyWizards; i++){
            
            bool validPosition = false;
            int x;
            int z;
            do{
                x = rnd.Next(19/2) * 2; //ensure that random int is even
                z = rnd.Next(11/2) * 2; //ensure that random int is even

                for(int j = 0; j < tiles.Length; j++){
                    if(tiles[j].transform.position.x == x && tiles[j].transform.position.z == z && !tiles[j].getTaken()){
                        validPosition = true;
                        tiles[j].setTaken(true);
                        //wizardPrefab.GetComponent<AbstractUnit>().setTile(j);
                    }
                }

            }while(!validPosition);
            
            GameObject wizObj = Instantiate(wizardPrefab, new Vector3(x, 1.25f, z), Quaternion.identity);
            tc.addWiz(wizObj);
        }
        int howManyClerics = CharacterMenuScript.getClericInput();
       
        for(int i = 0; i < howManyClerics; i++){
            bool validPosition = false;
            int x;
            int z;
            do{
                x = rnd.Next(19/2) * 2; //ensure that random int is even
                z = rnd.Next(11/2) * 2; //ensure that random int is even

                for(int j = 0; j < tiles.Length; j++){
                    if(tiles[j].transform.position.x == x && tiles[j].transform.position.z == z && !tiles[j].getTaken()){
                        validPosition = true;
                        tiles[j].setTaken(true);
                        //clericPrefab.GetComponent<AbstractUnit>().setTile(j);
                    }
                }

            }while(!validPosition);

            GameObject newCleric = Instantiate(clericPrefab, new Vector3(x, 1.25f, z), Quaternion.identity);
            tc.addCleric(newCleric);
        }
        int howManyEnemies = CharacterMenuScript.getEnemyInput();
        //int enemyTile = 0;
        for(int i = 0; i < howManyEnemies; i++){
            int prefabIndex = rnd.Next(2);
            bool validPosition = false;
            int x;
            int z;
            do{
                x = rnd.Next(19/2) * 2; //ensure that random int is even
                z = rnd.Next(12/2,25/2) * 2; //ensure that random int is even

                for(int j = 0; j < tiles.Length; j++){
                    if(tiles[j].transform.position.x == x && tiles[j].transform.position.z == z && !tiles[j].getTaken()){
                        validPosition = true;
                        tiles[j].setTaken(true);
                        //enemyTile = j;
                    }
                }

            }while(!validPosition);
            
           GameObject enemy = Instantiate(enemyPrefabList[prefabIndex], new Vector3(x, 1.25f, z), transform.rotation * Quaternion.Euler (0f, 180f, 0f));
           if(enemy.tag.Equals("Skel"))
           {
               //enemy.GetComponent<AbstractUnit>().setTile(enemyTile);
               tc.addSkel(enemy);
           }
           if(enemy.tag.Equals("SkelHorse"))
           {
              //enemy.GetComponent<AbstractUnit>().setTile(enemyTile);
              tc.addSkelHorse(enemy);
           }
          //if tag skel add skel if tag skel horse add skel horse 
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(start != null && end != null){
            computerPath(start, end);
        }
    }

    // void setStart(TileScript start){
    //     this.start = start;
    // }

    // void setEnd(TileScript end){
    //     this.end = end;
    // }

List<TileScript> tileQueue = new List<TileScript>();

    public void computerPath(TileScript start, TileScript end)
    {
        //clear all nodes from past comuting
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].clear();
        }

        TileScript current;

        //start this dikjkstra
        start.setDistance(0);
        tileQueue.Clear();
        tileQueue.Add(start);

        while (tileQueue.Count > 0)
        {
            //find least tile
            int smallestIndex = 0;
            for (int i = 1; i < tileQueue.Count; i++)
            {
                if (tileQueue[i].getDistance() < tileQueue[smallestIndex].getDistance())
                {
                    smallestIndex = i;
                }
            }

            current = tileQueue[smallestIndex];
            tileQueue.RemoveAt(smallestIndex);

            current.setVisited(true);

            if (current == end)
            {
                break;
            }

            for (int i = 0; i < current.getNeighbors().Count; i++)
            {
                if (!current.getNeighbors()[i].getVisited())
                {
                    //not previously seen
                    if (current.getNeighbors()[i].getDistance() == float.MaxValue)
                    {
                        tileQueue.Add(current.getNeighbors()[i]);
                    }

                    float discreteDistance = current.getDistance() + 2;
                    if (current.getNeighbors()[i].getDistance() > discreteDistance)
                    {
                        current.getNeighbors()[i].setDistance(discreteDistance);
                        current.getNeighbors()[i].setBackPointer(current);
                    }
                }
            }
        }

        temp.Clear();
        path.Clear();
        current = end;
        while (current != start && current != null) 
        {
            //current is the path from each node starting from the back to the front
            temp.Add(current);
            current = current.getBackPointer();
        }
        temp.Add(start);

        for (int i=temp.Count-1; i>=0; i--)
        {
            temp[i].setColor(Color.green * 3);
            path.Add(temp[i]);
        }
    }

    public TileScript[] getTiles(){
        return tiles;
    }
    public List<TileScript> getPath(){
        return path;
    }
}
