using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EscapeZoneCtrl : MonoBehaviour
{
    public SaveJSonData JsonDataManager;
    public float escapeTiming;
    public GameObject toast;
    public Text toastMsg;
    public Text toastTimer;
    public bool isFisrt = false;

    
    private float timer = 6;

    bool isEscape = false;

    

    private void OnEnable()
    {
        if (isFisrt)
        {
            escapeTiming = 120;
            toastMsg.text = "FIRST OPEN";
            
        }
        else
        {
            escapeTiming = 120;
            toastMsg.text = "ESCAPE OPEN";
        }
        
    }
    private void Update()
    {
        if (isFisrt) 
        {
            FirstEscape();
        }
        else
        {
            StageEscape();
        }

    }
    private void FirstEscape()
    {
        if (isEscape)
        {
            toast.SetActive(false);
            Save();
        }
        EscapeZoneUseTime();
    }
    private void StageEscape()
    {
        if (isEscape)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                Save();
            }
        }
        EscapeZoneUseTime();
    }

    string EscapeTimer(float _timer)
    {
        string sec = ((int)(_timer % 60)).ToString();
        string ms = string.Format("{0:00}",(_timer%1));
        ms = ms.Replace(".", "");
        string result = _timer.ToString("00.00");

        return result;
    }
    void EscapeZoneUseTime()
    {
         if (this.gameObject.activeSelf)
        {
            toast.SetActive(true);

            escapeTiming -= Time.deltaTime;
            toastTimer.text = ((int)escapeTiming / 60 % 60).ToString("D2") + ":" + ((int)escapeTiming % 60).ToString("D2");
            if (escapeTiming <= 0)
            {
                if (isFisrt)
                {
                    isFisrt = false;
                }
                this.gameObject.SetActive(false);
                toast.SetActive(false);
            }
        }
    }
    void Save()
    {
        this.gameObject.SetActive(false);
        toast.SetActive(false);

        JsonDataManager.ItemJsonData();
        GameStageManager.playerAlive = true;

        //SceneManager.LoadScene(1);
    }
    private void OnTriggerStay2D(Collider2D _player)
    {
        if (_player.gameObject.layer == 10)
        {
            isEscape = true;
            _player.GetComponent<PlayerCtrl>().GetEscapeTimer(EscapeTimer(timer), isEscape);
        }
    }
    private void OnTriggerExit2D(Collider2D _player)
    {
        if (_player.gameObject.layer == 10)
        {
            isEscape = false;
            timer = 6;
            _player.GetComponent<PlayerCtrl>().GetEscapeTimer(EscapeTimer(timer), isEscape);
        }
    }
}
