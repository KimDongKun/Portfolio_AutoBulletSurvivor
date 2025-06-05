using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShelterPlayerCtrl : MonoBehaviour
{
    public ShelterUIManager UI_Manager;
    private GameObject player;
    private Animator anim;

    public Transform cameraPos;

    public float player_MoveSpeed;
    public float moveLimit_x;
    public float moveLimit_y;

    public LayerMask shop_NPCLayer;

    public bool isShoping = false;

    // Start is called before the first frame update
    void Start()
    {
        player = this.gameObject;
        anim = player.GetComponent<Animator>();
        isShoping = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isShoping)
        {
            Movement();
        }
        CameraMovement();


        if (Input.GetMouseButtonUp(0)&& !isShoping)
        {
            ShopNPC_Click();
        }
        
    }

    void CameraMovement()
    {
        Vector3 camPos = new Vector3(this.transform.position.x,this.transform.position.y,-10);

        cameraPos.position = Vector3.Lerp(cameraPos.position, camPos, Time.deltaTime * 5);
    }
    void Movement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

       
        if (x != 0 || y != 0)
        {


            Vector3 move = (transform.right * x + transform.up * y) * Time.deltaTime * player_MoveSpeed;
            player.transform.position += move;
            player.transform.position = new Vector2(Mathf.Clamp(player.transform.position.x, -moveLimit_x, moveLimit_x), Mathf.Clamp(player.transform.position.y, -moveLimit_y, moveLimit_y));

            if (move.x > 0)
            {
                this.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (move.x < 0)
            {
                this.GetComponent<SpriteRenderer>().flipX = true;
            }

            anim.SetInteger("Animate", 2);
        }
        else
        {
            anim.SetInteger("Animate", 0);
        }

    }
    private void Find_ShopNPC()
    {
        Collider2D[] npcOpenShopUI = Physics2D.OverlapCircleAll(this.transform.position, 1, shop_NPCLayer);

        foreach (Collider2D ShopNPC in npcOpenShopUI)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                switch (npcOpenShopUI[0].name)
                {
                    case "SkinHead":
                        break;
                    case "Helmet":
                        break;
                    case "Red_Lady":
                        break;
                    case "":
                        break;
                }
            }
        }
    }
    private void ShopNPC_Click()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

        if (Physics2D.Raycast(pos,Vector2.zero, 0f) && (hit != null))
        {
            Debug.Log(hit.transform.tag);
            if (hit.transform.gameObject.tag == "SHOP_NPC" && !isShoping)
            {
                switch (hit.transform.gameObject.name)
                {
                    //__Button_OpenShop((int형)); (int형)에 따라 아이템을 취급하는 NPC 종류 구분.
                    case "Shop1":
                        UI_Manager.__Button_OpenShop(0);
                        break;
                    case "Shop2":
                        UI_Manager.__Button_OpenShop(1);
                        break;
                    case "Shop3":
                        UI_Manager.__Button_OpenShop(2);
                        break;
                    case "Shop4":
                        UI_Manager.__Button_OpenShop(3);
                        break;
                }
                isShoping = true;
            }
            if (hit.transform.gameObject.tag == "UPGRADE_NPC" && !isShoping)
            {
                UI_Manager.__Button_Upgrade_Active(true);
            }
        }
       
       
    }
}
