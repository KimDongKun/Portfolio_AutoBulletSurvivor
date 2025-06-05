using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorCtrl : MonoBehaviour
{
    public GameObject GameStartUI;
    public Transform playerPos;
    public Animator doorAnim;
    public bool isOpen;
    // Start is called before the first frame update
    void Start()
    {
        doorAnim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        doorAnim.SetBool("isPlayingRaid", isOpen);
        GameStartUI.SetActive(isOpen);
        float dis = Vector3.Distance(this.transform.position, playerPos.position);
        if (dis < 7f)
        {
            isOpen = true;
            
        }
        else
        {
            isOpen = false;
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.gameObject.name);
        if (col.gameObject.name == "Shelter_Player")
        {
            //SceneManager.LoadScene(0);
            //UI로 플레이 스테이지로 이동할거냐고 묻고 확인시 이동.
        }
    }
    public void __GameStartPanel(bool _active)
    {
        GameStartUI.SetActive(_active);
    }
    public void __GameStart()
    {
        SceneManager.LoadScene("SampleScene");
        
    }
}
