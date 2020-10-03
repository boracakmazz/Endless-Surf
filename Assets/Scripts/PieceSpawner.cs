using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    public PieceType type;
    private Piece currentPiece;

    public void Spawn()
    {
        /*int amountObj = 0;
        switch (type)
        {
            case PieceType.jump:
                amountObj = LevelManager.Instance.jumps.Count;
                break;

            case PieceType.longblock:
                amountObj = LevelManager.Instance.longBlocks.Count;
                break;

            case PieceType.ramp:
                amountObj = LevelManager.Instance.ramps.Count;
                break;
        }*/

        currentPiece = LevelManager.Instance.getPiece(type, 0); //gets a new piece from the object pool (obstacle, coin, etc.)
        currentPiece.gameObject.SetActive(true);
        currentPiece.transform.SetParent(transform, false);
    }

    public void deSpawn()
    {
        currentPiece.gameObject.SetActive(false);
    }
}
