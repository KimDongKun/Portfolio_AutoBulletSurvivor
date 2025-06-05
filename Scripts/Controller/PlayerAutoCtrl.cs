using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAutoCtrl : MonoBehaviour
{
    public GameObject player;

    //Move Route
    public Transform[] move_Points;
    public int posNum;

    //now Player_State
    public float player_HP;
    public float player_Food;
    public float player_Water;
    public float player_Stamina;

    public bool isRaid = false;  //인레이드
    public bool isDie = false; //생사유무

    public bool isCombat = false;//전투 여부
    public bool isSearching = false;//찾는 중
   
    public enum player_State
    {
        NONE,
        MOVE,
        SEARCHING,
        COMBAT
    }

    public void __StartRaid_Button()
    {
        isRaid = true;
        isSearching = false;
        isDie = false;
    }

   
    void Player_Point()
    {
        for(int i = 0; i< move_Points.Length; i++)
        {
            if(player.transform.position == move_Points[i].position)
            {
                //현재 Pos좌표 : move_Points[i].position
                posNum = i;
            }
        }
    }
    void Move_Point()
    {
        player.transform.position = Vector3.Lerp(player.transform.position, move_Points[posNum].position, Time.deltaTime);
    }
    void Set_Combat_State(player_State player_State)
    {

    }

    void Done_Raid()
    {
        isRaid = false;
        player_Stamina = 100f;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
    }

    // Update is called once per frame
    void Update()
    {
        if (isRaid)
        {
           
            ConsumeEnergy_Raid();

            Debug.Log("\n음식 수치:"+player_Food.ToString("F2") +"\n물 수치"+ player_Water.ToString("F2"));
        }
    }
    
    void ConsumeEnergy_Raid() //매 초마다 음식,물,스테미너 소모
    {
        player_Food -= Time.deltaTime * 0.2f;
        player_Water -= Time.deltaTime * 0.3f;
        player_Stamina -= Time.deltaTime * 0.5f;

        if (player_Food <= -10)
        {
            Debug.Log("end");
        }
        if(player_Stamina <= 0)
        {
            Done_Raid();
            Debug.Log("Raid_Over");
        }
    }
    
}
