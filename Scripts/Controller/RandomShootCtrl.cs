using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ThrrowKnife Skill = ThrrowGrenade Skill 같은 스크립트 사용.
/// </summary>
public class RandomShootCtrl : MonoBehaviour
{
    public int skill_Level = 1;
    public float thrrowPower = 10f;
    public float coolTime = 3f;
    
    public GameObject bulletPrefab;
    public static bool isMaster = false;
    private Transform playerPos;

    float count;
    float Timer;
    // Start is called before the first frame update
    void Start()
    {
        isMaster = false;
        playerPos = GameObject.Find("Player_Sprite").transform;
    }

    // Update is called once per frame
    void Update()
    {
        count = skill_Level;

        if (Timer >= coolTime)
        {
            for(int i = 0; i< count*2; i++)
            RandomFirePostion();

            Timer = 0;
        }
        else
        {
            Timer += Time.deltaTime;
        }
    }
    void RandomFirePostion()
    {
        Vector3 _randomPos = new Vector3(Random.Range(-3f + playerPos.position.x, 3f + playerPos.position.x), Random.Range(-3f + playerPos.position.y, 3f + playerPos.position.y), 0);
        Vector2 lookDir = _randomPos - playerPos.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;

        this.transform.position = _randomPos;

       
        //gunSound.Play();
        GameObject bullet = Instantiate(bulletPrefab, playerPos.position, Quaternion.Euler(0, 0, angle));
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(lookDir.normalized * thrrowPower, ForceMode2D.Impulse);
        //Debug.Log(lookDir.normalized);
    }
    public void ActiveSkill_LevelUP()
    {
        if (skill_Level >= 10)
        {
            isMaster = true;
        }
        else
        {
            skill_Level += 1;
            coolTime -= 0.1f;
        }
        //데미지 상승
        //레벨 10일때 만렙 신호 전달.
    }
}
