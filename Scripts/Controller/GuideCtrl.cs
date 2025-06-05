using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideCtrl : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(true);
        StartCoroutine(GuideClose());
    }
    IEnumerator GuideClose()
    {
        
        yield return new WaitForSeconds(10f);
        this.gameObject.SetActive(false);
    }
}
