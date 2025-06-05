using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSupportCtrl : MonoBehaviour
{
    public GameObject sub_Bullet;
    public float coolTime = 1f;

    public Vector3 thisFirePos;

    private Transform playerPos;
    private Transform playerFirePos;
    private AudioSource gunSound;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.Find("Player_Sprite").transform;
        playerFirePos = GameObject.Find("FireStartPos").transform;
    }
    // Update is called once per frame
    void Update()
    {
        this.transform.position = playerPos.position + thisFirePos;
        this.transform.rotation = playerFirePos.rotation;

        if (timer >= coolTime)
        {
            AttackSupportShoot();
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
    void AttackSupportShoot()
    {
        
        //gunSound.Play();
        GameObject bullet = Instantiate(sub_Bullet, transform.position, transform.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(playerFirePos.up * 20, ForceMode2D.Impulse);
    }
}
