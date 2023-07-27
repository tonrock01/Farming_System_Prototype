using UnityEngine;
using System;

public class Building : MonoBehaviour
{
    public  bool Placed { get; private set; }
    [SerializeField]
    public float timeToBuild;
    public bool isDecorationObject;
    public BoundsInt area;
    public BoundsInt realcoordinate;
    private Vector3 startPos;
    private float deltaX, deltaY;
    public Timer timer;

    public int id;
    public int object_id;
    public string name;

    void Start() 
    {
        // GridBuildingSystem.current.isPlaced = false;
        if (!Placed)
        {
            startPos = Input.mousePosition;
            startPos = Camera.main.ScreenToWorldPoint(startPos);
        
            deltaX = startPos.x - transform.position.x;
            deltaY = startPos.y - transform.position.y;
        }
    }

    void Update() 
    {
        if (!Placed)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 pos = new Vector3(mousePos.x - deltaX, mousePos.y - deltaY, 0);

            Vector3Int cellPosition = GridBuildingSystem.current.gridLayout.LocalToCell(pos);
            transform.position = GridBuildingSystem.current.gridLayout.CellToLocalInterpolated(cellPosition + new Vector3(.5f, .5f, 0f));
            GridBuildingSystem.current.FollowBuilding();
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (!Placed)
            {
                Vector3Int cellPosition = GridBuildingSystem.current.gridLayout.LocalToCell(transform.position);
                transform.localPosition = GridBuildingSystem.current.gridLayout.CellToLocalInterpolated(cellPosition + new Vector3(.5f, .5f, 0f));
            }
            if (CanBePlaced())
            {
                Place();
                ShopManager.Instance.ShopButton_Click();
                GridBuildingSystem.current.isPlaced = true;
                GridBuildingSystem.current.MainTilemap.gameObject.SetActive(false);
                CameraControl.Instance.canPan = true;
                FarmManager.Instance.isPressItemsButton = false;
                Debug.Log("วางแล้ว");
            }
        }
    }

    // void FixedUpdate() 
    // {
    //     if (Input.GetMouseButtonUp(0))
    //     {
    //         if (!Placed)
    //         {
    //             Vector3Int cellPosition = GridBuildingSystem.current.gridLayout.LocalToCell(transform.position);
    //             transform.localPosition = GridBuildingSystem.current.gridLayout.CellToLocalInterpolated(cellPosition + new Vector3(.5f, .5f, 0f));
    //         }
    //         if (CanBePlaced())
    //         {
    //             Place();
    //             ShopManager.Instance.ShopButton_Click();
    //             GridBuildingSystem.current.isPlaced = true;
    //             GridBuildingSystem.current.MainTilemap.gameObject.SetActive(false);
    //             CameraControl.Instance.canPan = true;
    //             FarmManager.Instance.isPressItemsButton = false;
    //             Debug.Log("วางแล้ว");
    //         }
    //     }
    // }

    private void OnMouseDown()
    {
        // if (!Placed)
        // {
        //     startPos = Input.mousePosition;
        //     startPos = Camera.main.ScreenToWorldPoint(startPos);
        
        //     deltaX = startPos.x - transform.position.x;
        //     deltaY = startPos.y - transform.position.y;
        // }
    }

    private void OnMouseDrag()
    {
        // if (!Placed)
        // {
        //     Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //     transform.position = new Vector3(mousePos.x - deltaX, mousePos.y - deltaY, 0);
        //     GridBuildingSystem.current.FollowBuilding();
        // }
    }

    private void OnMouseUp()
    {
        // if (!Placed)
        // {
        //     Vector3Int cellPosition = GridBuildingSystem.current.gridLayout.LocalToCell(transform.position);
        //     transform.localPosition = GridBuildingSystem.current.gridLayout.CellToLocalInterpolated(cellPosition + new Vector3(.5f, .5f, 0f));
        // }
        // if (CanBePlaced())
        // {
        //     Place();
        //     GridBuildingSystem.current.isPlaced = true;
        //     GridBuildingSystem.current.MainTilemap.gameObject.SetActive(false);
        //     CameraControl.Instance.canPan = true;
        //     FarmManager.Instance.isPressItemsButton = false;
        //     Debug.Log("วางแล้ว");
        // }
    }

    private void OnMouseUpAsButton()
    {
        if (isDecorationObject)
        {
            TimerTooltip.ShowTimer_Static(gameObject);
        }
    }
    
    public bool CanBePlaced()
    {
        Vector3Int positionInt = GridBuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        if (GridBuildingSystem.current.CanTakeArea(areaTemp))
        {
            return true;
        }

        return false;
    }

    public void Place()
    {
        Vector3Int positionInt = GridBuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;
        realcoordinate.position = areaTemp.position;
        Placed = true;
        
        GridBuildingSystem.current.TakeArea(areaTemp);

        Vector3Int PlotSize = new Vector3Int(-15, 17, 0);
        Vector3Int CompareSize = new Vector3Int(1, 1, 1);
        Vector3Int minus = new Vector3Int (1, 1, 0);
        
        if (area.size != CompareSize)
        {
            areaTemp.position = PlotSize - positionInt;
            areaTemp.position -= area.size - new Vector3Int(minus.x, minus.y, area.size.z);
        }
        else
        {
            areaTemp.position = PlotSize - positionInt;
        }
        
        Debug.Log("API Grid Coordinate : " + areaTemp);
        Debug.Log("Real Grid Coordinate : " + realcoordinate.position);

        

        // Timer timer = gameObject.AddComponent<Timer>();
        // //initialize timer - name of the process, starting time now, duration 3 minutes
        // timer.Initialize("Building", DateTime.Now, TimeSpan.FromMinutes(timeToBuild));
        // //start the timer
        // timer.StartTimer();
        // //when the timer finished destroy it
        // timer.TimerFinishedEvent.AddListener(delegate
        // {
        //     Destroy(timer);
        //     if (!isDecorationObject)
        //     {
        //         PlotManager plotManager = GetComponent<PlotManager>();
        //         plotManager.isBuilded = true;
        //     }
        // });
        if (!isDecorationObject)
        {
            PlotManager plotManager = GetComponent<PlotManager>();
            plotManager.isBuilded = true;
        }
    }

    public void SetValue(int id, int obj_id, Vector3Int pos, Vector3Int size, int time, string name)
    {
        this.id = id;
        this.object_id = obj_id;
        area = new BoundsInt(pos, size);
        ConvertCoordinate(area);
        this.timeToBuild = time;
        this.name = name;
        this.transform.localPosition = GridBuildingSystem.current.gridLayout.CellToLocalInterpolated(area.position + new Vector3(.5f, .5f, 0f));
        Placed = true;
        GridBuildingSystem.current.TakeArea(area);

        SpriteRenderer alphasprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        alphasprite.color = new Color(1, 1, 1, 0.5f);
        timer = gameObject.AddComponent<Timer>();
        timer.Initialize(this.name, DateTime.Now, TimeSpan.FromMinutes(timeToBuild / 60f));
        //start the timer
        timer.StartTimer();
        //when the timer finished destroy it
        timer.TimerFinishedEvent.AddListener(delegate
        {
            Destroy(timer);
            alphasprite.color = new Color(1, 1, 1, 1);
            if (!isDecorationObject)
            {
                PlotManager plotManager = GetComponent<PlotManager>();
                plotManager.isBuilded = true;
            }
        });
        GridBuildingSystem.current.isPlaced = true;
    }
    
    public void ConvertCoordinate(BoundsInt possize)
    {
        Vector3Int PlotSize = new Vector3Int(-15, 17, 0);
        Vector3Int CompareSize = new Vector3Int(1, 1, 1);
        Vector3Int minus = new Vector3Int (1, 1, 0);
        realcoordinate.position = possize.position;
        if (possize.size != CompareSize)
        {
            area.position = PlotSize - area.position;
            area.position -= area.size - new Vector3Int(minus.x, minus.y, area.size.z);
        }
        else
        {
            area.position = PlotSize - area.position;
        }

        Debug.Log("API Grid Coordinate : " + possize);
        Debug.Log("Real Grid Coordinate : " + area.position);
    }
}
