using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCtrl : MonoBehaviour
{
    public float magnet_dis;
    public float magnet_Pow;
    private Transform player;
    float dis;
    public bool isGetPlayer = false;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player_Sprite").transform;
        
        isGetPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        dis = Vector3.Distance(player.position, this.transform.position);

        if (dis < magnet_dis)
        {
            isGetPlayer = true;
        }
        if (isGetPlayer)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, player.position, Time.deltaTime * magnet_Pow);
        }
    }
}
