using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconFollowMouse : MonoBehaviour
{
    public static IconFollowMouse Instance;
    private Camera mainCamera;
    // Start is called before the first frame update
    void Awake() 
    {
        Instance = this;
    }

    void Start()
    {
        mainCamera = Camera.main;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 0f;

        transform.position = worldPosition;
    }

    // void FixedUpdate() 
    // {
    //     Vector3 mousePosition = Input.mousePosition;
    //     Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
    //     worldPosition.z = 0f;

    //     transform.position = worldPosition;
    // }

    public void SetShowIcon(Sprite spr)
    {
        Image img = gameObject.GetComponent<Image>();
        img.sprite = spr;
        gameObject.SetActive(true);
    }

    public void HideIcon()
    {
        gameObject.SetActive(false);
    }
}
