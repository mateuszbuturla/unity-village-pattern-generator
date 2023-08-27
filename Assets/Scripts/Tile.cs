using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int pos;
    public List<Direction> emptyDirections = new List<Direction>();

    void Start() {
        pos = new Vector2Int((int)transform.position.x, (int)transform.position.y);
    }

    public void RemoveElemetn(Direction direction) {
        emptyDirections.Remove(direction);
    }

    public bool CheckIfTileFits(Direction direction, Tile tile) {
        if (direction == Direction.Up) {
            if (tile.emptyDirections.Contains(Direction.Down) && !emptyDirections.Contains(Direction.Up)) {
                return false;
            }
            if (!tile.emptyDirections.Contains(Direction.Down) && emptyDirections.Contains(Direction.Up)) {
                return false;
            }
        }

        if (direction == Direction.Down) {
            if (tile.emptyDirections.Contains(Direction.Up) && !emptyDirections.Contains(Direction.Down)) {
                return false;
            }
            if (!tile.emptyDirections.Contains(Direction.Up) && emptyDirections.Contains(Direction.Down)) {
                return false;
            }
        }

        if (direction == Direction.Right) {
            if (tile.emptyDirections.Contains(Direction.Left) && !emptyDirections.Contains(Direction.Right)) {
                return false;
            }
            if (!tile.emptyDirections.Contains(Direction.Left) && emptyDirections.Contains(Direction.Right)) {
                return false;
            }
        }

        if (direction == Direction.Left) {
            if (tile.emptyDirections.Contains(Direction.Right) && !emptyDirections.Contains(Direction.Left)) {
                return false;
            }
            if (!tile.emptyDirections.Contains(Direction.Right) && emptyDirections.Contains(Direction.Left)) {
                return false;
            }
        }

        return true;
    }
}
