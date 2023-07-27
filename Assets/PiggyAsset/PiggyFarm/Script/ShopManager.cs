using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    //singletone pattern
    public static ShopManager Instance;
    [SerializeField] private List<Sprite> sprites;
    private bool opened;

    //prefab for displaying shop info
    [SerializeField] private GameObject itemPrefab;

    private Dictionary<ObjectType, List<ShopItem>> shopItems = new Dictionary<ObjectType, List<ShopItem>>(3);
    [SerializeField] public TabGroup shopTabs;
    public GameObject ShopButton;

    private void Awake()
    {
        //initialize fields
        Instance = this;
    }

    private void Start()
    {
        Load();
        Initialize();
        gameObject.SetActive(false);
    }

    private void Load()
    {
        //load every shop item from resources
        ShopItem[] items = Resources.LoadAll<ShopItem>("Shop");
        
        //initialize the dictionary
        shopItems.Add(ObjectType.Plot, new List<ShopItem>());
        shopItems.Add(ObjectType.Seed, new List<ShopItem>());
        shopItems.Add(ObjectType.Decorations, new List<ShopItem>());

        //add all shop items to the dictionary
        foreach (var item in items)
        {
            shopItems[item.Type].Add(item);
        }
    }

    private void Initialize()
    {
        for (int i = 0; i < shopItems.Keys.Count; i++)
        {
            foreach (var item in shopItems[(ObjectType)i])
            {
                GameObject itemObject = Instantiate(itemPrefab, shopTabs.objectsToSwap[i].transform);
                itemObject.GetComponent<ShopItemHolder>().Initialize(item);   
            }
        }
    }
    
    public void ShopButton_Click()
    {
        if (!opened)
        {
            opened = true;
            ShopButton.SetActive(false);
            gameObject.SetActive(true);
        }
        else
        {
            ShopButton.SetActive(true);
            gameObject.SetActive(false);
            opened = false;
        }
    }

    private bool dragging;

    public void OnBeginDrag()
    {
        dragging = true;
    }

    public void OnEndDrag()
    {
        dragging = false;
    }

    public void OnPointerClick()
    {
        if (!dragging)
        {
            ShopButton_Click();
        }
    }
}
