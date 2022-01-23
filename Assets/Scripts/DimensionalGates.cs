using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DimensionalGates : MonoBehaviour
{
    // Colors
    private Color blueMapSolid = new Color (10f/255f,17f/255f,40f/255f);
    private Color blueMapPassable = new Color(0f/255f,159f/255f,255f/255f,0.5f);

    private Color redMapSolid = new Color (72.0f/255.0f,35.0f/255.0f,60.0f/255.0f);
    private Color redMapPassable = new Color (210.0f/255.0f,50.0f/255.0f,50.0f/255.0f,0.5f);

    public bool blueGate;
    private TilemapCollider2D gateCollider;
    private Tilemap gateTilemap;
    void Start()
    {
        gateTilemap = GetComponent<Tilemap>();
        UpdateGateCollision(0);
        UpdateColors(0);
    }

    public void UpdateColors(int dimension)
    {
        if(dimension == 0)
        {
            if(blueGate)
            {
                gateTilemap.color = blueMapPassable;
            }
            else
            {
                gateTilemap.color = blueMapSolid;
            }
        }
        else
        {
            if(blueGate)
            {
                gateTilemap.color = redMapSolid;
            }
            else
            {
                gateTilemap.color = redMapPassable;
            }
        }
    }
    public void UpdateGateCollision(int dimension)
    {
        gateCollider =  GetComponent<TilemapCollider2D>();

        if(dimension == 0)
        {
            if(blueGate)
            {
                gateCollider.enabled = false;
            }
            else
            {
                gateCollider.enabled = true;
            }
        }
        else
        {
            if(blueGate)
            {
                gateCollider.enabled = true;
            }
            else
            {
                gateCollider.enabled = false;
            }
        }
    }
    void LerpMapColors()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
