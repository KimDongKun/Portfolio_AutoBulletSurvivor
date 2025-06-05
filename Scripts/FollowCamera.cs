using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowCamera : MonoBehaviour
{
    public GameObject mainCam;
    public GameObject playerObject;

    public AudioSource gunSound;
    public GameObject bulletPrefab;
    public GameObject railGunPrefab;
    public GameObject grenadePrefab;
    public Transform gunPos;
    public Transform fireStart;

    public float distance = 10;
    public float moveSpeed = 5;
    public float fireTime = 0;
    public float fireLate = 0.5f;
    public float damage = 10f;
    public int criticalValue = 10;

    public float railGun_coolTime;
    public Image railGun_UI;

    private bool isRailGunShoot;
    private bool isShoot;
    private Vector3 camPos;
    private AudioLimit audioLimit;
    // Start is called before the first frame update
    void Start()
    {
        isRailGunShoot = true;
        isShoot = false;

        camPos = new Vector3(playerObject.transform.position.x, playerObject.transform.position.y, -0);
        audioLimit = GameObject.Find("GameManager").GetComponent<AudioLimit>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!GameStageManager.isGamePause)
        {
            CameraMoveSetting();

            if (Input.GetMouseButtonDown(0))
            {
                if (isShoot)
                {
                    isShoot = false;
                }
                else
                {
                    isShoot = true;
                }
            }
            if (Input.GetMouseButtonUp(1))
            {
                if (isRailGunShoot)
                RailGun_Shoot();
            }

            //if (Input.GetButton("Fire1") && fireTime <= 0)
            if (isShoot && fireTime <= 0)
            {
                fireTime = fireLate;
                fireTime -= Time.deltaTime;
                Shoot();
                //Grenade();
            }
            else
            {
                if (fireTime <= 0)
                {
                    fireTime = 0;
                }
                else
                    fireTime -= Time.deltaTime;
            }

            if (!isRailGunShoot)
            {
                railGun_coolTime -= Time.deltaTime;
                railGun_UI.fillAmount = railGun_coolTime / 10f;
                railGun_UI.color = Color.black;
                if (railGun_coolTime <= 0)
                {
                    isRailGunShoot = true;
                    railGun_coolTime = 10f;
                    railGun_UI.color = Color.white;
                }
            }
        }
        
    }

    void CameraMoveSetting()
    {
        Vector3 mousePos = mainCam.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);

        float result_x = (playerObject.transform.position.x + mousePos.x)/2;
        float result_y = (playerObject.transform.position.y + mousePos.y)/2;
        Vector3 resultPos = new Vector3(Mathf.Clamp(result_x, -3f + playerObject.transform.position.x, 3f + playerObject.transform.position.x), Mathf.Clamp(result_y, -3f + playerObject.transform.position.y, 3f + playerObject.transform.position.y), 0);

        camPos = Vector3.Lerp(mainCam.transform.position, new Vector3(resultPos.x, resultPos.y, -distance), Time.deltaTime * moveSpeed);
        mainCam.transform.position = camPos;

        //mainCam.transform.position =new Vector3(resultPos.x, resultPos.y, -distance);
        if (result_x > playerObject.transform.position.x)
        {
            playerObject.transform.localScale = new Vector2(0.4f, playerObject.transform.localScale.y);
            //playerObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            playerObject.transform.localScale = new Vector2(-0.4f, playerObject.transform.localScale.y);
            //playerObject.GetComponent<SpriteRenderer>().flipX = true;
        }

        Vector2 lookDir = mousePos - playerObject.transform.position;
        float angle = Mathf.Atan2(lookDir.y,lookDir.x) * Mathf.Rad2Deg - 90f;
        gunPos.eulerAngles = new Vector3(0, 0, angle);
    }
    void Shoot()
    {
        //반동 (아래 주석처리)
        //mainCam.transform.position = new Vector3(mainCam.transform.position.x, mainCam.transform.position.y + 0.5f, mainCam.transform.position.z);

        gunSound.Play();
        GameObject bullet = Instantiate(bulletPrefab, fireStart.position, fireStart.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(fireStart.up * 30, ForceMode2D.Impulse);//20
    }
    void RailGun_Shoot()
    {
        //반동 (아래 주석처리)
        //mainCam.transform.position = new Vector3(mainCam.transform.position.x, mainCam.transform.position.y + 0.5f, mainCam.transform.position.z);
        isRailGunShoot = false;
        audioLimit.ExplodePlaySound(5);
        GameObject bullet = Instantiate(railGunPrefab, fireStart.position, fireStart.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(fireStart.up * 120, ForceMode2D.Impulse);//20
    }
    void Grenade()
    {
        GameObject grenade = Instantiate(grenadePrefab, fireStart.position, fireStart.rotation);
        Rigidbody2D rb = grenade.GetComponent<Rigidbody2D>();
        rb.AddForce(fireStart.up * 10, ForceMode2D.Impulse);

    }
}
