using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItemBoxCtrl : MonoBehaviour
{


    private Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.gameObject.layer == 10)
        {
            anim.SetBool("ItemBox_Open", true); //Open
           // GameObject.Find("EnemySpawnManager").GetComponent<DropItemManager>().__Random_DropItem(this.transform.position);
        }
    }
    private void DestroyItemBox()
    {
        Destroy(this.gameObject);
    }
    private void SpawnRandomItem()
    {
        GameObject.Find("EnemySpawnManager").GetComponent<DropItemManager>().__Random_DropItem(this.transform.position);
    }
}
