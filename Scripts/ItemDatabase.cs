using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public Sprite itemSptrie; //아이템 이미지
    public string itemName;   //아이템 이름(영문)
    public string itemCode;   //아이템 코드 예시) D-0 ,A-2 와 같이 "등급 - 순서"
    public string itemDescription; //설명- 굳이필요할까?

    private Transform player;
    private void Start()
    {
        player = GameObject.Find("Player_Sprite").transform;
    }
    private void Update()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, player.position, Time.deltaTime * 15);
    }
}
