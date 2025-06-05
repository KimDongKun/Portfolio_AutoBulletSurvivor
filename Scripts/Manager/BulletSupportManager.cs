using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSupportManager : MonoBehaviour
{
    public int skill_Level = 1;
    public GameObject[] supportObjects;
    public static bool isMaster = false;
    public float coolTime;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < supportObjects.Length; i++)
        {
            supportObjects[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < supportObjects.Length; i++)
        {
            supportObjects[i].GetComponent<BulletSupportCtrl>().coolTime = coolTime;
        }
    }
    public void ActiveSkill()// 총 4개
    {
        for (int i = 0; i < supportObjects.Length; i++)
        {
            if (!supportObjects[i].activeSelf)
            {
                supportObjects[i].SetActive(true);
                return;
            }
        }
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
            coolTime -= 0.02f;
            if(skill_Level%3 == 0)
            {
                ActiveSkill();
            }

            if (skill_Level >= 10)
            {
                isMaster = true;
            }
        }
        //데미지 상승
        //레벨 10일때 만렙 신호 전달.
    }

}
