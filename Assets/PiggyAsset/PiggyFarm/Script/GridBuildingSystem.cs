using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class GridBuildingSystem : MonoBehaviour
{
    public static GridBuildingSystem current;
    
    public GridLayout gridLayout;
    public Tilemap MainTilemap;
    public Tilemap TempTilemap;

    private static Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();

    public Building temp; 
    private BoundsInt prevArea;

    public Transform buildingTrans, plotTrans;
    public bool isPlaced;
    
    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        //todo change the initialization method if you want to
        string tilePath = @"Tiles\";
        tileBases.Add(TileType.Empty, null);
        tileBases.Add(TileType.White, Resources.Load<TileBase>(tilePath + "white"));
        tileBases.Add(TileType.Green, Resources.Load<TileBase>(tilePath + "green"));
        tileBases.Add(TileType.Red, Resources.Load<TileBase>(tilePath + "red"));
        isPlaced = true;
    }

    private void Update()
    {
        if (!temp)
        {
            return;
        }
        
        //I chose Space bar to confirm house placement
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     if (temp.CanBePlaced())
        //     {
        //         temp.Place();
        //     }
        // }
        // else if (Input.GetKeyDown(KeyCode.Escape))
        // {
        //     ClearArea();
        //     Destroy(temp.gameObject);
        // }
    }

    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;
        
        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }
        
        return array;
    }

    private static void SetTilesBlock(BoundsInt area, TileType type, Tilemap tilemap)
    {
        TileBase[] tileArray = new TileBase[area.size.x * area.size.y * area.size.z];
        FillTiles(tileArray, type);
        tilemap.SetTilesBlock(area, tileArray);
    }

    private static void FillTiles(TileBase[] arr, TileType type)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = tileBases[type];
        }
    }

    public void InitializeWithBuilding(GameObject building)
    {
        if(isPlaced)
        {
            isPlaced = false;
            MainTilemap.gameObject.SetActive(true);
            // Vector3 position = gridLayout.CellToLocalInterpolated(new Vector3(.5f, .5f, 0f)); 
            Vector3 cameraPosition = Camera.main.transform.position;
            Vector3 position = new Vector3(cameraPosition.x, cameraPosition.y, 0f);
            temp = Instantiate(building, position, Quaternion.identity, buildingTrans).GetComponent<Building>();
            FollowBuilding();
            CameraControl.Instance.canPan = false;
            FarmManager.Instance.isPressItemsButton = true;
        }
    }

    public void InitializeWithPlot(GameObject plot, Vector3 pos, String type)
    {
        if(isPlaced)
        {
            isPlaced = false;
            MainTilemap.gameObject.SetActive(true);
            // Vector3 position = gridLayout.CellToLocalInterpolated(new Vector3(.5f, .5f, 0f)); 
            // Vector3 cameraPosition = Camera.main.transform.position;
            // pos.z = 0;
            // pos.y -= plot.GetComponentInChildren<SpriteRenderer>().bounds.size.y / 2f;
            Vector3Int cellPos = gridLayout.WorldToCell(pos);
            Vector3 poss = gridLayout.CellToLocalInterpolated(cellPos);
            //Vector3 position = new Vector3(pos.x, pos.y, 0f);
            if(type == "Plot")
            {
                temp = Instantiate(plot, poss, Quaternion.identity, plotTrans).GetComponent<Building>();
            }
            if(type == "Decoration")
            {
                temp = Instantiate(plot, poss, Quaternion.identity, buildingTrans).GetComponent<Building>();
            }
            FollowBuilding();
            CameraControl.Instance.canPan = false;
            FarmManager.Instance.isPressItemsButton = true;
        }
    }

    public void ClearArea()
    {
        TileBase[] toClear = new TileBase[prevArea.size.x * prevArea.size.y * prevArea.size.z];
        FillTiles(toClear, TileType.Empty);
        TempTilemap.SetTilesBlock(prevArea, toClear);
    }

    public void FollowBuilding()
    {
        ClearArea();

        temp.area.position = gridLayout.WorldToCell(temp.gameObject.transform.position);
        BoundsInt buildingArea = temp.area;
        BoundsInt realCoor = temp.realcoordinate;

        Vector3Int PlotSize = new Vector3Int(-15, 17, 0);
        Vector3Int CompareSize = new Vector3Int(1, 1, 1);
        Vector3Int minus = new Vector3Int (1, 1, 0);
        if (temp.area.size != CompareSize)
        {
            temp.realcoordinate.position = temp.area.position;
            temp.area.position = PlotSize - temp.area.position;
            temp.area.position -= temp.area.size - new Vector3Int(minus.x, minus.y, temp.area.size.z);
        }
        else
        {
            temp.realcoordinate.position = temp.area.position;
            temp.area.position = PlotSize - temp.area.position;
        }

        TileBase[] baseArray = GetTilesBlock(buildingArea, MainTilemap);

        int size = baseArray.Length;
        TileBase[] tileArray = new TileBase[size];

        for (int i = 0; i < baseArray.Length; i++)
        {
            if (baseArray[i] == tileBases[TileType.White])
            {
                tileArray[i] = tileBases[TileType.Green];
            }
            else
            {
                FillTiles(tileArray, TileType.Red);
                break;
            }
        }
        
        TempTilemap.SetTilesBlock(buildingArea, tileArray);
        prevArea = buildingArea;
    }

    public bool CanTakeArea(BoundsInt area)
    {
        TileBase[] baseArray = GetTilesBlock(area, MainTilemap);
        foreach (var b in baseArray)
        {
            if (b != tileBases[TileType.White])
            {
                Debug.Log("Cannot place here");
                return false;
            }
        }

        return true;
    }

    public void TakeArea(BoundsInt area)
    {
        SetTilesBlock(area, TileType.Empty, TempTilemap);
        SetTilesBlock(area, TileType.Green, MainTilemap);
    }
    
}

//types of tiles
public enum TileType
{
    Empty, //empty
    White, //available
    Green, //can place
    Red    //can't place
}