using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelUpCtrl : MonoBehaviour
{
    public Transform m_PlayerTransform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = m_PlayerTransform.position;
    }
}
