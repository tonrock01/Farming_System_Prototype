using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemHolder : MonoBehaviour
{
    private ShopItem Item;

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI priceText;

    public void Initialize(ShopItem item)
    {
        Item = item;

        //initialize UI
        iconImage.sprite = Item.Icon;
        titleText.text = Item.Name;
        priceText.text = Item.Price.ToString();
        UnlockItem();
    }

    public void UnlockItem()
    {
        iconImage.gameObject.AddComponent<ShopItemDrag>().Initialize(Item);
    }
}
