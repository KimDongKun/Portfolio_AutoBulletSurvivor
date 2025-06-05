using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeCtrl : MonoBehaviour
{
    public float bomb_damage = 1f;
    public float impactRange;
    public float force;
    public LayerMask enemyLayer;
    public GameObject explosionEffect;

    private FollowCamera damageData;
    private AudioLimit audioLimit;
    private bool isDestroy = false;

    public int SoundNum = 0;
    public enum GrenadeType
    {
        Grenade,
        AirBomb,
        ThrrowBomb
    }
    private GrenadeType type;

    // Start is called before the first frame update
    void Start()
    {
        damageData = Camera.main.GetComponent<FollowCamera>();
        audioLimit = GameObject.Find("GameManager").GetComponent<AudioLimit>();
        StartCoroutine(Explode());
    }

    // Update is called once per frame
    void Update()
    {
        if (isDestroy)
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(2f);
        Collider2D[] hitEnemy = Physics2D.OverlapCircleAll(transform.position, impactRange, enemyLayer);

        float explode_damage = damageData.damage * 1.5f;

        foreach (Collider2D enemy in hitEnemy)
        {
            Vector2 dir = enemy.transform.position - transform.position;
            enemy.GetComponent<Rigidbody2D>().AddForce(dir * force);
            if (enemy.GetComponent<EnemyCtrl>())
            {
                enemy.GetComponent<EnemyCtrl>().ExplosionDamage(explode_damage);
            }
            if (enemy.GetComponent<EnemyBugCtrl>())
            {
                enemy.GetComponent<EnemyBugCtrl>().ExplosionDamage(explode_damage);
            }
            if (enemy.GetComponent<EnemyHiveCtrl>())
            {
                enemy.GetComponent<EnemyHiveCtrl>().ExplosionDamage(explode_damage);
            }
        }
        GameObject _effect = Instantiate(explosionEffect, this.transform.position, Quaternion.identity);
        //_effect.GetComponent<AudioSource>().volume = 0.2f ;

        audioLimit.ExplodePlaySound(SoundNum);
        //audioLimit.PlayOneShotSound(_effect.GetComponent<AudioSource>(), _effect.GetComponent<AudioSource>().clip, _effect.GetComponent<AudioSource>().volume);

        Destroy(_effect,5);
        Destroy(this.gameObject);
        isDestroy = true;
    }

    
   /* private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, impactRange);
    }*/
}
