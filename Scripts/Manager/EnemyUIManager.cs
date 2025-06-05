using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUIManager : MonoBehaviour
{
    public GameObject damageText;


    public void ActiveDamageText(float _damage, bool _critcal)
    {
        GameObject _damageText = Instantiate(damageText, this.transform.position,Quaternion.identity);

        if (_critcal)
        {
            _damageText.GetComponentInChildren<Image>().enabled = true;
        }
        else
        {
            _damageText.GetComponentInChildren<Image>().enabled = false;
        }

        _damageText.GetComponentInChildren<Text>().text = _damage.ToString("F0");
        _damageText.GetComponentInChildren<Rigidbody2D>().AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        Destroy(_damageText, 0.6f);
        
    }
}
