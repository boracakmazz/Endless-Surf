﻿using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { set; get; }

    

    //Level Spawning
    private const float DISTANCE_BEFORE_SPAWN = 100.0F;
    private const int INITIAL_SEGMENTS = 10;
    private const int INITIAL_TRANSITION_SEGMENTS = 2;
    private const int MAX_SEGMENTS_ON_SCREEN = 15;
    private Transform cameraContainer;
    private int amountOfActiveSegments;
    private int continiousSegments;
    private int currentSpawnZ;
    private int currentLevel;
    private int y1, y2, y3;

    //List of pieces

    public List<Piece> ramps = new List<Piece>();
    public List<Piece> longBlocks = new List<Piece>();
    public List<Piece> jumps = new List<Piece>();
    [HideInInspector]
    public List<Piece> pieces = new List<Piece>();  //All the pieces in the pool

    //List of segments

    public List<Segment> availableSegments = new List<Segment>();
    public List<Segment> availableTransitions = new List<Segment>();
    [HideInInspector]
    public List<Segment> segments = new List<Segment>();

    private bool isPlayerMoving = false;

    public Piece getPiece(PieceType pt, int visualIndex)
    {
        Piece p = pieces.Find(x => x.type == pt && x.visualIndex == visualIndex && !x.gameObject.activeSelf);

        if(p == null)
        {
            GameObject gameObject = null;

            if(pt == PieceType.ramp)
                gameObject = ramps[visualIndex].gameObject;

            else if(pt == PieceType.longblock)
                gameObject = longBlocks[visualIndex].gameObject;

            else if(pt == PieceType.jump)
                gameObject = jumps[visualIndex].gameObject;

            gameObject = Instantiate(gameObject);
            p = gameObject.GetComponent<Piece>();
        }

        return p;
    }

    private void Awake()
    {
        Instance = this;
        cameraContainer = Camera.main.transform;
        currentSpawnZ = 0;
        currentLevel = 0;
    }

    private void Start()
    {
        for ( int i = 0; i< INITIAL_SEGMENTS; i++)
        { 
            if (i < INITIAL_TRANSITION_SEGMENTS)
                spawnTransition();
            else
                generateSegment();
        }
    }

    private void Update()
    {
        if(currentSpawnZ - cameraContainer.position.z < DISTANCE_BEFORE_SPAWN)
        {
            generateSegment();
        }

        if(amountOfActiveSegments >= MAX_SEGMENTS_ON_SCREEN)
        {
            segments[amountOfActiveSegments - 1].deSpawn();
            amountOfActiveSegments--;
        }
    }

    private void generateSegment()
    {
        spawnSegment();

        if(Random.Range(0f,1f) < (continiousSegments * 0.25f))
        {
            continiousSegments = 0;
            spawnTransition();
        }
        else
        {
            continiousSegments++;
        }
        
    }

    private void spawnSegment()
    {
        //Checks if possible segments available
        List<Segment> possibleSeg = availableSegments.FindAll(x => x.beginY1 == y1 || x.beginY2 == y2 || x.beginY3 == y3);
        int id = Random.Range(0, possibleSeg.Count);

        Segment s = getSegment(id, false);

        y1 = s.endY1;
        y2 = s.endY2;
        y3 = s.endY3;

        s.transform.SetParent(transform);
        s.transform.localPosition = Vector3.forward * currentSpawnZ;

        currentSpawnZ += s.lenght;
        amountOfActiveSegments++;
        s.Spawn();

    }

    public Segment getSegment(int id, bool transition)
    {
        Segment s = null;
        s = segments.Find(x => x.SegId == id && x.transition == transition && !x.gameObject.activeSelf);

        if (s == null)
        {
            GameObject gameObject = Instantiate((transition) ? availableTransitions[id].gameObject : availableSegments[id].gameObject) as GameObject;
            s = gameObject.GetComponent<Segment>();

            s.SegId = id;
            s.transition = transition;

            segments.Insert(0, s);
        }
        else
        {
            segments.Remove(s);
            segments.Insert(0, s);
        }
        return s;
    }

    private void spawnTransition()
    {
        List<Segment> possibleTransition = availableTransitions.FindAll(x => x.beginY1 == y1 || x.beginY2 == y2 || x.beginY3 == y3);
        int id = Random.Range(0, possibleTransition.Count);

        Segment s = getSegment(id, true);

        y1 = s.endY1;
        y2 = s.endY2;
        y3 = s.endY3;

        s.transform.SetParent(transform);
        s.transform.localPosition = Vector3.forward * currentSpawnZ;

        currentSpawnZ += s.lenght;
        amountOfActiveSegments++;
        s.Spawn();
    }
}
