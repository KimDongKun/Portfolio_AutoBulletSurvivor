using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBombCtrl : MonoBehaviour
{
    public int skill_Level = 1;
    public GameObject bombPrefab;
    public float airStrike_Bomb_Damage = 30f;
    public static bool isMaster = false;
    
    public AudioSource air_Sound;
    public float coolTime = 10f; //최종 쿨타임 5초
    private float timer;

    public bool isActive = false;

    private Transform playerPos;
    private Vector3 resultPos;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        isActive = false;
        isMaster = false;

        playerPos = GameObject.Find("Player_Sprite").transform;
        GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //this.transform.localPosition = new Vector3(Mathf.Clamp(this.transform.position.x, -35, 35), Mathf.Clamp(this.transform.position.x, -10, 10), 10);
        timer += Time.deltaTime;
        if (timer >= coolTime && !isActive)
        {
            ActiveBomb();
        }
        if (isActive)
        {
            //Vector3 endPos = new Vector3(-35, this.transform.position.y, 10);
            GetComponent<SpriteRenderer>().enabled = true;
            this.transform.position = Vector3.MoveTowards(this.transform.position, resultPos, Time.deltaTime*20);

            
            if (timer > coolTime+0.25f)
            {
                timer = coolTime;
                GameObject _bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
                //_bomb.GetComponent<GrenadeCtrl>().bomb_damage = airStrike_Bomb_Damage;
            }
            
            if (this.transform.position == resultPos)
            {
                GetComponent<SpriteRenderer>().enabled = false;
                timer = 0;
                isActive = false;
            }
        }
    }

    void ActiveBomb()
    {
        //int startPos_x = 35;
        //int startPos_y = Random.Range(-10, 11);
        air_Sound.Play();
        GetComponent<SpriteRenderer>().enabled = true;
        float startPos_x = playerPos.position.x + 35;
        float startPos_y = playerPos.position.y - Random.Range(-10, 11);

        resultPos = new Vector3(playerPos.position.x - 35, startPos_y, 10);

        this.transform.position = new Vector3(startPos_x, startPos_y, 10);
        isActive = true;
    }

    public void ActiveSkill_LevelUP()
    {
        if(skill_Level >= 10)
        {
            isMaster = true;
        }
        else
        {
            skill_Level += 1;
            coolTime -= 0.5f;
            airStrike_Bomb_Damage += 10;

            if (skill_Level >= 10)
            {
                isMaster = true;
            }
        }
        
        //데미지 상승
        //레벨 10일때 만렙 신호 전달.
    }
   
}
