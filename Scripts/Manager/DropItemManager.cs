using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemManager : MonoBehaviour
{
    public List<GameObject> class_A_ItemList;
    public List<GameObject> class_B_ItemList;
    public List<GameObject> class_C_ItemList;
    public List<GameObject> class_D_ItemList;

    public int itemScale;
    public EnemySpawn enemySpawn;

   
    public void __Random_DropItem(Vector3 spawnPositon)
    {
        Vector3 spawnPos = spawnPositon;
        int roundValue = enemySpawn.GameStageManager.round;
        itemScale = roundValue > 15 ? roundValue > 30 ? 3 : 2 : 1;

        switch (itemScale)
        {
            case 1:
                int random_1 = Random.Range(0, 1000);

                if (random_1 <= 800)
                {
                    DropItem(class_D_ItemList, spawnPos);
                }
                else
                {
                    DropItem(class_C_ItemList, spawnPos);
                }
                break;
            case 2:
                int random_2 = Random.Range(0, 1000);

                if (random_2 <= 500)
                {
                    DropItem(class_D_ItemList, spawnPos);
                }
                else if (500 < random_2 && random_2 <= 800)
                {
                    DropItem(class_C_ItemList, spawnPos);
                }
                else if (800 < random_2 && random_2 <= 995)
                {
                    DropItem(class_B_ItemList, spawnPos);
                }
                else
                {
                    DropItem(class_A_ItemList, spawnPos);
                }
                break;
            case 3:
                int random_3 = Random.Range(0, 1000);

                if (random_3 <= 400)
                {
                    DropItem(class_D_ItemList, spawnPos);
                }
                else if (400 < random_3 && random_3 <= 700)
                {
                    DropItem(class_C_ItemList, spawnPos);
                }
                else if (700 < random_3 && random_3 <= 990)
                {
                    DropItem(class_B_ItemList, spawnPos);
                }
                else
                {
                    DropItem(class_A_ItemList, spawnPos);
                }
                break;
        }

        /*if (roundValue > 300)
        {
            int _random = Random.Range(0, 1000);
            
            if (_random <= 500)
            {
                DropItem(class_D_ItemList, spawnPos);
            }
            else if (500 < _random && _random <= 800)
            {
                DropItem(class_C_ItemList, spawnPos);
            }
            else if (800 < _random && _random <= 995)
            {
                DropItem(class_B_ItemList, spawnPos);
            }
            else
            {
                DropItem(class_A_ItemList, spawnPos);
            }
        }
        else
        {
            int _random = Random.Range(0, 1000);

            if (_random <= 800)
            {
                DropItem(class_D_ItemList, spawnPos);
            }
            else
            {
                DropItem(class_C_ItemList, spawnPos);
            }
        }*/
    }
    public void DropItem(List<GameObject> itemList, Vector3 spawnPos)
    {
        int _random = Random.Range(0, itemList.Count);

        GameObject _item = Instantiate(itemList[_random], spawnPos, Quaternion.identity);
        //_item.GetComponent<>


    }
}
