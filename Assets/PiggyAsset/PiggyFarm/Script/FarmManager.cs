using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Threading.Tasks;

public class FarmManager : MonoBehaviour
{
    public static FarmManager Instance { get; private set; }
    public bool waterTool,
        fertilizingTool,
        isPressToolsButton,
        plotItem,
        cornItem,
        tomatoItem,
        isPressItemsButton,
        isDrag;
    public Button plotButton;
    public GameObject canvas;
    public GameObject[] GamePrefab;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        ShopItemDrag.canvas = canvas.GetComponent<Canvas>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (isPressToolsButton || isPressItemsButton)
            {
                isDrag = true;
            }
        }
        else
        {
            isDrag = false;
        }
    }

    public void UseTools(int toolsnumber)
    {
        if (isPressToolsButton)
        {
            isPressToolsButton = false;

            CameraControl.Instance.canPan = true;
            waterTool = false;
            fertilizingTool = false;

            Debug.Log("Cancel Tools");
        }
        else
        {
            if (toolsnumber == 1 && !waterTool && !isPressItemsButton)
            {
                TooltipManager.Instance.HideTooltip();
                CameraControl.Instance.canPan = false;
                isPressToolsButton = true;
                waterTool = true;
                fertilizingTool = false;
                Debug.Log("Use Water");
            }
            if (toolsnumber == 2 && !fertilizingTool && !isPressItemsButton)
            {
                TooltipManager.Instance.HideTooltip();
                CameraControl.Instance.canPan = false;
                isPressToolsButton = true;
                waterTool = false;
                fertilizingTool = true;
                Debug.Log("Use Fertilizing");
            }
        }
    }

    public void UseItems(int itemsnumber)
    {
        if (isPressItemsButton)
        {
            isPressItemsButton = false;

            plotItem = false;
            cornItem = false;
            tomatoItem = false;
            CameraControl.Instance.canPan = true;
            Debug.Log("Cancel Items");
        }
        else
        {
            if (itemsnumber == 1 && !plotItem && !isPressToolsButton)
            {
                TooltipManager.Instance.HideTooltip();
                CameraControl.Instance.canPan = false;
                isPressItemsButton = true;
                plotItem = true;
                Debug.Log("Use Plot");
            }
            if (itemsnumber == 2 && !cornItem && !isPressToolsButton)
            {
                TooltipManager.Instance.HideTooltip();
                CameraControl.Instance.canPan = false;
                isPressItemsButton = true;
                cornItem = true;
                Debug.Log("Use Corn Seed");
            }
            if (itemsnumber == 3 && !tomatoItem && !isPressToolsButton)
            {
                TooltipManager.Instance.HideTooltip();
                CameraControl.Instance.canPan = false;
                isPressItemsButton = true;
                tomatoItem = true;
                Debug.Log("Use Tomato Seed");
            }
        }
    }

    public void CancelUseItems()
    {
        if (isPressItemsButton)
        {
            isPressItemsButton = false;

            plotItem = false;
            cornItem = false;
            tomatoItem = false;
            CameraControl.Instance.canPan = true;
            Debug.Log("Cancel Items");
        }
    }

    public void CancelUseTools()
    {
        if (isPressToolsButton)
        {
            isPressToolsButton = false;

            CameraControl.Instance.canPan = true;
            waterTool = false;
            fertilizingTool = false;

            Debug.Log("Cancel Tools");
        }
    }
}
