using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletCtrl : MonoBehaviour
{
    public float enemyBullet_Damage;

    void Start()
    {
        StartCoroutine(DestroyAmmo());
    }
    IEnumerator DestroyAmmo()
    {
        yield return new WaitForSeconds(10f);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D player)
    {
        if(player.gameObject.layer == 10)
        {
            Debug.Log("플레이어가 총에 맞음");
            player.transform.GetComponent<PlayerCtrl>().GetAttackDamage(enemyBullet_Damage);
            Destroy(this.gameObject);
        }
    }
}
