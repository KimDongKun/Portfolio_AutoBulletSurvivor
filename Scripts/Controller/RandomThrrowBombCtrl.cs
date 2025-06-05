using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomThrrowBombCtrl : MonoBehaviour
{
    public int skill_Level = 1;
    public GameObject grenadePrefab;
    public static bool isMaster = false;
    public float bomb_damage;
    public float coolTime = 5f;
    public int _bombValue = 1;
   
    private float timer;
   
    // Update is called once per frame
    void Update()
    {
        if (timer >= coolTime && !GameStageManager.isGamePause)
        {
            for(int i = 0; i< _bombValue; i++)
            {
                ThrrowBomb();
            }
            
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
    void ThrrowBomb()
    {
        float ran_x = Random.Range(0.35f, 0.75f);
        float ran_y = Random.Range(0.35f, 0.75f);

        Vector2 resultPos = Camera.main.ViewportToWorldPoint(new Vector2(ran_x,ran_y));
        //Debug.Log("폭발 랜덤 좌표: "+resultPos);

        GameObject _bomb = Instantiate(grenadePrefab,resultPos,Quaternion.identity);
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
            coolTime -= 0.2f;
            _bombValue += 1;
            bomb_damage +=10;

            if (skill_Level >= 10)
            {
                isMaster = true;
            }
            //데미지 상승
            //레벨 10일때 만렙 신호 전달.
        }

    }
}
