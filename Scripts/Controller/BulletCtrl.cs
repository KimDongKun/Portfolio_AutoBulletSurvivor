using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyAmmo());
    }
    IEnumerator DestroyAmmo()
    {
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }
}
