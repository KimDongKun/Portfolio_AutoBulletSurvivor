using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrrowCtrl : MonoBehaviour 
{
    public float speed = 1f;
    void Update()
    {
        this.transform.Rotate(0, 0, -speed);
    }
}
