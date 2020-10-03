using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment : MonoBehaviour
{
    public int SegId { set; get; }
    public bool transition;

    public int lenght;  //how long is our segment
    public int beginY1, beginY2, beginY3;
    public int endY1, endY2, endY3;

    private PieceSpawner[] pieces; //Our objects which are inside of each segment

    private void Awake()
    {
        pieces = gameObject.GetComponentsInChildren<PieceSpawner>();
    }

    public void Spawn()
    {
        gameObject.SetActive(true);
        
    }

    public void deSpawn()
    {
        gameObject.SetActive(false);
        
    }

}
