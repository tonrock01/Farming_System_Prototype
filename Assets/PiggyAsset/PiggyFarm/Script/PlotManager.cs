using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlotManager : MonoBehaviour
{
    public bool isWater, isPlotavaliable, isFertilizing, isBuilded, readyToHarvest;
    public Sprite plotDefault, plotAvailable, plotUnavailable, plotWet;
    public Transform productParent;
    private Building building;
    private float timeToGrow;
    public Timer timer;

    [SerializeField]
    public GameObject[] seedPrefab;
    string productName;
    SpriteRenderer plotsprite;
    Animator productAnimator;

    private void Start()
    {
        isPlotavaliable = true;
        plotsprite = GetComponentInChildren<SpriteRenderer>();
        building = GetComponent<Building>();
    }

    void Update() 
    {
        if(Input.GetMouseButtonUp(0))
        {
            if (building.Placed && GridBuildingSystem.current.isPlaced && isBuilded)
            {
                if (!FarmManager.Instance.isPressToolsButton && !FarmManager.Instance.isPressItemsButton)
                {
                    CameraControl.Instance.canPan = true;
                }
                TooltipManager.Instance.HideTooltip();
                FarmManager.Instance.CancelUseItems();
                FarmManager.Instance.CancelUseTools();
                IconFollowMouse.Instance.HideIcon();
            }
        }
    }

    void FixedUpdate() 
    {
        if(!isPlotavaliable && !readyToHarvest)
        {
            if (timer.secondsLeft <= timeToGrow / 2 )
            {
                productAnimator.SetInteger("Plant_Level", 2);
            }
            if (timer.secondsLeft <= 0)
            {
                productAnimator.SetInteger("Plant_Level", 3);
                readyToHarvest = true;
                Debug.Log("Ready to harvest");
            }
        }
    }

    private void OnMouseOver()
    {
        if (building.Placed && GridBuildingSystem.current.isPlaced && isBuilded)
        {
            if (FarmManager.Instance.isDrag)
            {
                if (FarmManager.Instance.waterTool || FarmManager.Instance.fertilizingTool)
                {
                    IsUseTools();
                }
                if (FarmManager.Instance.cornItem || FarmManager.Instance.tomatoItem)
                {
                    IsUseSeed();
                }
            }
            if (FarmManager.Instance.isPressToolsButton || FarmManager.Instance.isPressItemsButton)
            {
                CheckPlot();
            }
        }
    }

    private void OnMouseDown() 
    {
        if (building.Placed && GridBuildingSystem.current.isPlaced && isBuilded)
        {
            if (!FarmManager.Instance.isPressToolsButton && !FarmManager.Instance.isPressItemsButton)
            {
                plotsprite.sprite = plotAvailable;
                CameraControl.Instance.canPan = false;
                TooltipManager.Instance.ShowTooltip(this.transform, isPlotavaliable, isWater, isFertilizing);
            }
        }
    }

    private void OnMouseUpAsButton()
    {
        TimerTooltip.ShowTimer_Static(gameObject);
    }

    private void OnMouseExit()
    {
        if (building.Placed && GridBuildingSystem.current.isPlaced && isBuilded)
        {
            if (isWater)
            {
                plotsprite.sprite = plotWet;
            }
            else
            {
                plotsprite.sprite = plotDefault;
            }
        }
    }

    public void IsUseTools()
    {
        if (FarmManager.Instance.waterTool && !isWater && !isPlotavaliable)
        {
            plotsprite.sprite = plotWet;
            isWater = true;
        }
        if (FarmManager.Instance.fertilizingTool && !isFertilizing && !isPlotavaliable)
        {
            isFertilizing = true;
        }
    }

    public void IsUseSeed()
    {
        if (isPlotavaliable)
        {
            Building temp = GetComponent<Building>();
            if (FarmManager.Instance.cornItem)
            {
                Instantiate(seedPrefab[0], productParent);
                isPlotavaliable = false;
                productName = "Corn";
                timeToGrow = 10f;
            }
            if (FarmManager.Instance.tomatoItem)
            {
                Instantiate(seedPrefab[1], productParent);
                isPlotavaliable = false;
                productName = "Tomato";
                timeToGrow = 16f;
            }

            timer = gameObject.AddComponent<Timer>();
            timer.Initialize(productName , DateTime.Now, TimeSpan.FromMinutes(timeToGrow / 60f));
            //start the timer
            timer.StartTimer();
            //when the timer finished destroy it
            timer.TimerFinishedEvent.AddListener(delegate
            {
                Destroy(timer);
            });

            productAnimator = transform.GetComponentInChildren<Animator>();
        }
    }

    public void CheckPlot()
    {
        if (FarmManager.Instance.isPressItemsButton && !FarmManager.Instance.isPressToolsButton)
        {
            if (isPlotavaliable)
            {
                if (FarmManager.Instance.cornItem || FarmManager.Instance.tomatoItem)
                {
                    plotsprite.sprite = plotAvailable;
                    Debug.Log("วางเมล็ดได้");
                }
                else
                {
                    plotsprite.sprite = plotUnavailable;
                    Debug.Log("วางไม่ได้");
                }
            }
            else
            {
                plotsprite.sprite = plotUnavailable;
                Debug.Log("วางไม่ได้");
            }
        }
        if (!FarmManager.Instance.isPressItemsButton && FarmManager.Instance.isPressToolsButton)
        {
            if (!isPlotavaliable)
            {
                if (FarmManager.Instance.waterTool)
                {
                    if (!isWater)
                    {
                        plotsprite.sprite = plotAvailable;
                        Debug.Log("ใส่น้ำได้");
                    }
                    else
                    {
                        plotsprite.sprite = plotUnavailable;
                        Debug.Log("ใส่น้ำแล้ว");
                    }
                }

                if (FarmManager.Instance.fertilizingTool)
                {
                    if (!isFertilizing)
                    {
                        plotsprite.sprite = plotAvailable;
                        Debug.Log("ใส่ปุ่ยได้");
                    }
                    else
                    {
                        plotsprite.sprite = plotUnavailable;
                        Debug.Log("ใส่ปุ่ยแล้ว");
                    }
                }
            }
            else
            {
                plotsprite.sprite = plotUnavailable;
                Debug.Log("วางไม่ได้");
            }
        }
    }
}
