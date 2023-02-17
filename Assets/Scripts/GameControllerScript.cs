using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour
{
    TileScript[] tiles;

    public TileScript start, end;

    // Start is called before the first frame update
    void Start()
    {
        tiles = FindObjectsOfType<TileScript>();
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = i + 1; j < tiles.Length; j++)
            {
                if (Vector3.Distance(tiles[i].transform.position, tiles[j].transform.position) < 1.1f) {
                    tiles[i].getNeighbors().Add(tiles[j]);
                    tiles[j].getNeighbors().Add(tiles[i]);
                }
            }
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        computerPath(start, end);
    }

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

                    float discreteDistance = current.getDistance() + 1;
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

    public List<TileScript> temp = new List<TileScript>();
    public List<TileScript> path = new List<TileScript>();
}
