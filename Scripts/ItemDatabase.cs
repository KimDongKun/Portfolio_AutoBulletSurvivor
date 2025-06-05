using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public Sprite itemSptrie; //������ �̹���
    public string itemName;   //������ �̸�(����)
    public string itemCode;   //������ �ڵ� ����) D-0 ,A-2 �� ���� "��� - ����"
    public string itemDescription; //����- �����ʿ��ұ�?

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
