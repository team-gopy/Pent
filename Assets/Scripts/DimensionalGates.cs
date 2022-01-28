using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DimensionalGates : MonoBehaviour
{
    // Colors
    private bool switchingColors = false;
    private float lerpControl = 0f;
    private Color currentMapColor;
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
        DefaultGateState();
    }
    public void FlipGateColor()
    {
        blueGate = !blueGate;
        UpdateGateCollision(GameController.Instance.GetCurrentDimension());
        UpdateColors(GameController.Instance.GetCurrentDimension());
    }
    void DefaultGateState()
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
    public void UpdateColors(int dimension)
    {
        if(dimension == 0)
        {
            if(blueGate)
            {
                currentMapColor = blueMapPassable;
            }
            else
            {
                currentMapColor = blueMapSolid;
            }
        }
        else
        {
            if(blueGate)
            {
                currentMapColor = redMapSolid;
            }
            else
            {
                currentMapColor = redMapPassable;
            }
        }
        switchingColors = true;
        lerpControl = 0;

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
        gateTilemap.color = Color.Lerp(gateTilemap.color,currentMapColor,lerpControl);
        if (lerpControl < 1)
        {
            lerpControl += Time.deltaTime/8f;
        }
        if(gateTilemap.color == currentMapColor)
        {
           switchingColors = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(switchingColors)
        {
            LerpMapColors();
        }
    }
}
