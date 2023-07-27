using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    public Vector3 startPos;
    public float deltaX, deltaY;
    public Building building;
    // Start is called before the first frame update
    void Start()
    {
        building = GetComponent<Building>();

        startPos = Input.mousePosition;
        startPos = Camera.main.ScreenToWorldPoint(startPos);
        
        deltaX = startPos.x - transform.position.x;
        deltaY = startPos.y - transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 pos = new Vector3(mousePos.x - deltaX, mousePos.y - deltaY, 0);

        Vector3Int cellPosition = GridBuildingSystem.current.gridLayout.WorldToCell(pos);
        transform.position = GridBuildingSystem.current.gridLayout.CellToLocalInterpolated(cellPosition + new Vector3(.5f, .5f, 0f));
    }

    void FixedUpdate() 
    {
        if(Input.GetMouseButtonUp(0))
        {
            // gameObject.GetComponent<Building>().CheckPlacement();
            //destroy drag since we don't need it
            Destroy(this);
        }
    }
}
