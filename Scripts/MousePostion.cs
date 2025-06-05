using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePostion : MonoBehaviour
{
    public GameObject player;

    public GameObject selectedObject;
    public LayerMask layerMask;
    public LayerMask layerMask_Tile;

    bool isMove = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isMove)
        {
            player.transform.position = Vector2.MoveTowards(player.transform.position, selectedObject.transform.position, Time.deltaTime*4f);
        }

        RaycastSensor();

       
    }
    void RaycastSensor()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hitData = Physics2D.Raycast(new Vector2(worldPosition.x, worldPosition.y), Vector2.zero, 0, layerMask);
            {
                if (hitData)
                {
                    selectedObject = hitData.transform.gameObject;
                    //selectedObject.transform.GetComponent<SpriteRenderer>().color = Color.red;
                    isMove = true;
                    if (worldPosition.x < 0)
                    {
                        player.transform.localScale = new Vector3(-1,1,1);
                    }
                    else
                    {
                        player.transform.localScale = new Vector3(1,1,1);
                    }
                    Debug.Log("click");
                }
               
            }
        }
    }
    void Move_Anim()
    {

    }
}
