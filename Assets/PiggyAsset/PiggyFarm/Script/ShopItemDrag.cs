using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItemDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private ShopItem Item;
    public static Canvas canvas;

    private RectTransform rt;
    private CanvasGroup cg;
    private Image img;

    private Vector3 originPos;
    private bool drag;

    public void Initialize(ShopItem item)
    {
        Item = item;
    }

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        cg = GetComponent<CanvasGroup>();

        img = GetComponent<Image>();
        originPos = rt.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        drag = true;
        cg.blocksRaycasts = false;
        img.maskable = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rt.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        drag = false;
        cg.blocksRaycasts = true;
        img.maskable = true;
        rt.anchoredPosition = originPos;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Border"))
        {
            ShopManager.Instance.ShopButton_Click();

            Color c = img.color;
            c.a = 0f;
            img.color = c;

            Camera camera = Camera.main;
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = camera.transform.position.z;
            Vector3 worldPosition = camera.ScreenToWorldPoint(mousePosition);

            if (Item.Type == ObjectType.Plot)
            {
                GridBuildingSystem.current.InitializeWithPlot(Item.Prefab, worldPosition, "Plot");
            }
            if (Item.Type == ObjectType.Seed)
            {
                if (Item.Name == "Corn Seed")
                {
                    FarmManager.Instance.UseItems(2);
                    IconFollowMouse.Instance.SetShowIcon(Item.Icon);
                }
                if (Item.Name == "Tomato Seed")
                {
                    FarmManager.Instance.UseItems(3);
                    IconFollowMouse.Instance.SetShowIcon(Item.Icon);
                }
            }
            if (Item.Type == ObjectType.Decorations)
            {
                GridBuildingSystem.current.InitializeWithPlot(
                    Item.Prefab,
                    worldPosition,
                    "Decoration"
                );
            }
        }
    }

    private void OnEnable()
    {
        drag = false;
        cg.blocksRaycasts = true;
        img.maskable = true;
        rt.anchoredPosition = originPos;

        Color c = img.color;
        c.a = 1f;
        img.color = c;
    }
}
