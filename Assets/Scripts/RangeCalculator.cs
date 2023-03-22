using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeCalculator : MonoBehaviour
{
    private TileScript allTiles;
    void Start()
    {
        gc = GameControllerScript.getInstance();
    }
    void Update () 
    {
        allTiles = gc.getTiles();
    }

    // this function will calculate the range of the current character by taking in a tile and a range and returning a list of tiles
    // that are within the range of the current characters movement or attack
    // I want to do this by creating a map for tile given by using the neighbors of the tile and then using a BFS to find the tiles

    // this function can be changed so that it doesn't return anything, but instead just highlights the tiles that are within the range(but that would be harder)
    // right now this code is coded to work with the movement of the character, but it can be changed to work with the attack range of the character
    // because the move range is by tiles and the attack range is by distance, the attack range will have to be calculated differently
    // this code wasn't tested yet, it is more of a shell that could work for our situation
    public List<TileScript> calculateRange(TileScript tile, int range) {
        List<TileScript> rangeTiles = new List<TileScript>();
        List<TileScript> visited = new List<TileScript>();
        Queue<TileScript> queue = new Queue<TileScript>();
        queue.Enqueue(tile);
        visited.Add(tile);
        while (queue.Count != 0) {
            TileScript currTile = queue.Dequeue();
            if (currTile.getDistance() <= range) {
                rangeTiles.Add(currTile);
            }
            foreach (TileScript neighbor in currTile.getNeighbors()) {
                if (!visited.Contains(neighbor)) {
                    neighbor.setDistance(currTile.getDistance() + 1);
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }
        }
        return rangeTiles;
    }
}
