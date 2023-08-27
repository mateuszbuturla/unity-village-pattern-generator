using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagePatterGanerator : MonoBehaviour
{
    public int maxCross;
    public int crossCount = 0;
    public Tile startObj;
    public Dictionary<Vector2Int, Tile> tiles = new Dictionary<Vector2Int, Tile>();
    public List<GameObject> tileOptions = new List<GameObject>();
    public List<GameObject> tileHouseOptions = new List<GameObject>();

    private void Start()
    {
        tiles.Add(new Vector2Int(0,0), startObj);

        StartCoroutine(Generate());
    }

    public void IncreaseCrossCount() {
        crossCount++;
    }

    IEnumerator Generate () {
        List<Tile> tempTiles = new List<Tile>();

        foreach (KeyValuePair<Vector2Int, Tile> keyValue in tiles) {
            if (keyValue.Value.emptyDirections.Count > 0)
                tempTiles.Add(keyValue.Value);
        }
        
        tempTiles.Sort((a, b) => { return a.emptyDirections.Count - b.emptyDirections.Count; });


        if (tempTiles.Count > 0) {
            GeneratorHelper.Initialize(this);
            Tile currentTile = tempTiles[0];
            Direction dir = currentTile.emptyDirections[0];
            List<GameObject> possibleTiles = GeneratorHelper.GetPossibleTiles(dir);

            Vector2Int newPos = GeneratorHelper.GetNewPosition(new Vector2Int(currentTile.pos.x, currentTile.pos.y), dir);


            GameObject tilePrefab = GeneratorHelper.GetCorrectTilePrefab(possibleTiles, newPos);
            
            if (tilePrefab == null) { 
                tilePrefab = GeneratorHelper.GetHouseTile(dir);
            }

            if (tilePrefab != null) {
                GameObject newTile = Instantiate(tilePrefab, new Vector3(newPos.x, newPos.y, 0), Quaternion.identity);
                newTile.GetComponent<Tile>().emptyDirections.Remove(GeneratorHelper.ReverseDirection(dir));
                
                tiles.Add(newPos, newTile.GetComponent<Tile>());

                GeneratorHelper.RemoveDirectionFromNeighboringTiles(newPos);
                GeneratorHelper.RemoveDirectionsOfNeighboringTileFromNewTile(newTile, newPos);
            }
            GeneratorHelper.RemoveDirectionFromCurrentTile(currentTile, dir);
        }

        yield return new WaitForSeconds(0f);
        StartCoroutine(Generate());
    }
}
