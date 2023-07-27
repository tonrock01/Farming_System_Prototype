using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;
    public RectTransform uiElement;
    public Camera mainCamera;
    public Transform plotPosition;
    public Image waterButton, fertilizingButton;

    void Awake() 
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    
    void Start()
    {
        mainCamera = GameObject.FindObjectOfType<Camera>();
        uiElement = GetComponent<RectTransform>();
        plotPosition = GetComponent<Transform>();
        // waterButton.gameObject.SetActive(false);
        // fertilizingButton.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void ShowTooltip(Transform plotpos, bool isplotavaliable, bool iswater, bool isfertilizing)
    {
        Vector3 positionObjectOnScreen = mainCamera.WorldToScreenPoint(plotpos.transform.position);
        this.transform.position = positionObjectOnScreen;
        if (!isplotavaliable)
        {
            gameObject.SetActive(true);
            waterButton.gameObject.SetActive(true);
            fertilizingButton.gameObject.SetActive(true);
            if (iswater)
            {
                waterButton.color = Color.red;
            }
            else
            {
                waterButton.color = Color.white;
            }
            
            if (isfertilizing)
            {
                fertilizingButton.color = Color.red;
            }
            else
            {
                fertilizingButton.color = Color.white;
            }
        }
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
