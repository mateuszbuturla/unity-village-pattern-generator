using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeneratorHelper
{
    public static Dictionary<Direction, Vector2Int> directions = new Dictionary<Direction, Vector2Int>
    {
        {Direction.Up, new Vector2Int(0, 1)},
        {Direction.Down, new Vector2Int(0, -1)},
        {Direction.Left, new Vector2Int(-1, 0)},
        {Direction.Right, new Vector2Int(1, 0)}
    };

    public static VillagePatterGanerator villageGenerator;

    public static void Initialize(VillagePatterGanerator generator) {
        villageGenerator = generator;
    }

    public static List<GameObject> GetPossibleTiles(Direction dir) {
        List<GameObject> toReturn = new List<GameObject>();

        Direction dirRev = ReverseDirection(dir);

        for (int i = 0; i < villageGenerator.tileOptions.Count; i++) {
            if (villageGenerator.tileOptions[i].GetComponent<Tile>().emptyDirections.Contains(dirRev)) {
                toReturn.Add(villageGenerator.tileOptions[i]);
            }
        }

        return toReturn;
    }

    public static GameObject GetHouseTile(Direction dir) {
        Direction dirRev = ReverseDirection(dir);

        for (int i = 0; i < villageGenerator.tileHouseOptions.Count; i++) {
            if (villageGenerator.tileHouseOptions[i].GetComponent<Tile>().emptyDirections.Contains(dirRev)) {
                return villageGenerator.tileHouseOptions[i];
            }
        }

        return null;
    }

    public static Direction ReverseDirection(Direction dir) {
        if (dir == Direction.Up) return Direction.Down;
        if (dir == Direction.Down) return Direction.Up;
        if (dir == Direction.Left) return Direction.Right;
        if (dir == Direction.Right) return Direction.Left;

        return dir;
    }

    public static Vector2Int GetNewPosition(Vector2Int currentTilePosition, Direction dir) {
        return new Vector2Int(currentTilePosition.x + directions[dir].x, currentTilePosition.y + directions[dir].y);
    }

    public static GameObject GetCorrectTilePrefab (List<GameObject> possibleTiles, Vector2Int newPos) {
        List<GameObject> tempTiles = new List<GameObject>(possibleTiles);

        for(int i = 0; i < tempTiles.Count; i++) {
            int random = Random.Range(0, tempTiles.Count);

            if (CheckIfTilesFits(tempTiles[random], newPos)) {
                if (tempTiles[random].GetComponent<Tile>().emptyDirections.Count == 4) {
                    villageGenerator.IncreaseCrossCount();
                }
                return tempTiles[random];
            }
            else {
                tempTiles.RemoveAt(random);
            }
        }

        return null;
    }

    public static void RemoveDirectionFromCurrentTile(Tile currentTile, Direction dir) {
        foreach (KeyValuePair<Vector2Int, Tile> keyValue in villageGenerator.tiles) {
            if (keyValue.Key == currentTile.pos) {
                villageGenerator.tiles[keyValue.Key].RemoveElemetn(dir);
            }
        }
    }

    public static void RemoveDirectionsOfNeighboringTileFromNewTile(GameObject newTile, Vector2Int newPos) {
        RemoveDirectionOfNeighboringTileFromNewTile(newTile, newPos, Direction.Right);
        RemoveDirectionOfNeighboringTileFromNewTile(newTile, newPos, Direction.Left);
        RemoveDirectionOfNeighboringTileFromNewTile(newTile, newPos, Direction.Up);
        RemoveDirectionOfNeighboringTileFromNewTile(newTile, newPos, Direction.Down);
    }

    public static void RemoveDirectionFromNeighboringTiles(Vector2Int newPos) {
        RemoveDirectionFromNeighboringTile(newPos, Direction.Right);
        RemoveDirectionFromNeighboringTile(newPos, Direction.Left);
        RemoveDirectionFromNeighboringTile(newPos, Direction.Up);
        RemoveDirectionFromNeighboringTile(newPos, Direction.Down);
    }

    private static void RemoveDirectionFromNeighboringTile(Vector2Int newPos, Direction dir) {
        Vector2Int pos = new Vector2Int(newPos.x + directions[dir].x, newPos.y + directions[dir].y);
        if (villageGenerator.tiles.ContainsKey(pos)) {
            villageGenerator.tiles[pos].emptyDirections.Remove(ReverseDirection(dir));
        }
    }

    private static void RemoveDirectionOfNeighboringTileFromNewTile(GameObject newTile, Vector2Int newPos, Direction dir) {
        if (villageGenerator.tiles.ContainsKey(new Vector2Int(newPos.x + directions[dir].x, newPos.y + directions[dir].y))) {
            newTile.GetComponent<Tile>().emptyDirections.Remove(dir);
        }
    }

    private static bool ValidNeighboringTile(GameObject tile,  Vector2Int newPos, Direction dir) {
        Vector2Int pos = new Vector2Int(newPos.x + directions[dir].x, newPos.y + directions[dir].y);
        if (villageGenerator.tiles.ContainsKey(pos)) {
            bool result = villageGenerator.tiles[pos].CheckIfTileFits(ReverseDirection(dir), tile.GetComponent<Tile>());
            if (!result) {
                return false;
            }
        }

        return true;
    }

    private static bool CheckIfTilesFits (GameObject tile,  Vector2Int newPos) {
        if (tile.GetComponent<Tile>().emptyDirections.Count == 4 && villageGenerator.crossCount >= villageGenerator.maxCross) {
            return false;
        }

        if (!ValidNeighboringTile(tile, newPos, Direction.Right) || 
            !ValidNeighboringTile(tile, newPos, Direction.Left) || 
            !ValidNeighboringTile(tile, newPos, Direction.Up) || 
            !ValidNeighboringTile(tile, newPos, Direction.Down)) {
            return false;
        }

        return true;
    }
}
