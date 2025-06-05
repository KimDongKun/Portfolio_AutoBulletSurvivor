using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemboxCtrl : MonoBehaviour
{
    public int costCoin;
    public Text costText;

    public bool isEpicBox = false;

    public int random_ItemCode1;
    public int random_ItemCode2;
    public int random_ItemCode3;

    private GameStageManager stageManager;
    private SkillManager skill_LevelState;

    List<int> itemList = new List<int> { 0, 1, 2, 3, 4 };

    // Start is called before the first frame update
    void Start()
    {
        //GameStageManager.OnStageItemBox = true;

        stageManager = GameObject.Find("GameManager").GetComponent<GameStageManager>();
        skill_LevelState = GameObject.Find("Player_ActiveSkill").GetComponent<SkillManager>();

        /*costCoin = ((int)GameStageManager.timer / 60 % 60) * 5;

        if (costCoin == 0)
        {
            costCoin = 3;
            //costText.text = "Press \"E\"\nFREE BOX";
            costText.text = "Press \"E\"\nCOST: " + costCoin.ToString();
        }
        else
        {
            costText.text = "Press \"E\"\nCOST: " + costCoin.ToString();
        }*/

        costCoin = 0;
        costText.text = "Get It";
        /*random_ItemCode1 = RandomSkillCode();
        random_ItemCode2 = RandomSkillCode();
        random_ItemCode3 = RandomSkillCode();*/
    }

    int RandomSkillCode()
    {
        int random_ItemCode = 0;

        //추후 마스터된 스킬목록 제외한 랜덤 (int)값 추출
        //단, 전부 마스터시, (회복 or 달러) 중 하나 지급.
        // RandomShootCtrl ,BulletSupportManager ,RandomGrenadeCtrl ,RandomThrrowBombCtrl ,AirBombCtrl
       List<int> other = new List<int>(); //스킬 마스터시 마스터된 스킬은 상자에 안뜨게하는 함수.

        if (RandomShootCtrl.isMaster)// A_칼 던지기
        {
            Debug.Log("칼 마스터");
            other.Add(0);
        }
        if(BulletSupportManager.isMaster)// A_지원 사격
        {
            Debug.Log("지원사격 마스터");
            other.Add(1);
        }
        if (RandomGrenadeCtrl.isMaster)// A_수류탄 투척
        {
            Debug.Log("수류탄 마스터");
            other.Add(2);
        }
        if (RandomThrrowBombCtrl.isMaster)// A_포격 요청
        {
            Debug.Log("포격 마스터");
            other.Add(3);
        }
        if (AirBombCtrl.isMaster)// A_에어스트라이커
        {
            Debug.Log("공중폭격 마스터");
            other.Add(4);
        }

        if (other.Count < 5) //모든 스킬 마스터시.
        {
            int newItemCode = GetRandomItemCodeExcluding(other);
           


           /* var range = Enumerable.Range(0, 5).Where(i => !other.Contains(i));
            var rand = new System.Random();
            int index = rand.Next(0, 5 - other.Count);
            random_ItemCode = range.ElementAt(index);*/

            random_ItemCode = newItemCode;
            //Debug.Log("New Item Code: " + random_ItemCode);
            if (random_ItemCode.Equals(random_ItemCode1) || random_ItemCode.Equals(random_ItemCode2))
            {
                if (random_ItemCode.Equals(random_ItemCode1) && !other.Contains(random_ItemCode1))
                {
                    other.Add(random_ItemCode1);
                }
                    
                if (random_ItemCode.Equals(random_ItemCode2) && !other.Contains(random_ItemCode2))
                {
                    other.Add(random_ItemCode2);
                }
                   

                random_ItemCode = GetRandomItemCodeExcluding(other);
                Debug.Log("New Item Code: " + random_ItemCode);
                //Debug.LogError("값: " + random_ItemCode);
            }
        }
        else
        {
            Debug.Log("ALL 마스터");
            random_ItemCode = 5; //쉘터 머니 지급
            //쉘터 머니 습득시 함수짜줘 ㅋ
            //그리고 박스 생성될때 마스터된 스킬들 확인 부탁.
        }
        
    
        return random_ItemCode;
    }

    public void OpenBox()
    {
        if (!isEpicBox)
        {
            OpenSkillBox();
        }
        else
        {
            OpenEpicBox();
        }
    }

    public void OpenSkillBox()
    {
        Debug.Log("스킬 상자 오픈");
        random_ItemCode1 = RandomSkillCode();
        random_ItemCode2 = RandomSkillCode();
        random_ItemCode3 = RandomSkillCode();

        //skill_LevelState.UpgradeSkill(random_ItemCode);
        List<int> skillCodeList = new List<int>();
        skillCodeList.Add(random_ItemCode1);
        skillCodeList.Add(random_ItemCode2);
        skillCodeList.Add(random_ItemCode3);
        stageManager.OpenRandomBox(skillCodeList);
        //Debug.LogError("스킬 코드: "+ random_ItemCode+" GET!");
    }

    public void OpenEpicBox()
    {
        //stageManager.Get_StageItemList("Item-Name");//아이템 List 저장 -현재 사용 안함
    }
    int GetRandomItemCodeExcluding(List<int> excludedCodes)
    {
        // LINQ를 사용하여 제외할 코드를 포함하지 않는 리스트를 만듭니다.
        List<int> filteredItemList = itemList.Where(item => !excludedCodes.Contains(item)).ToList();

        // 필터링된 리스트에서 랜덤으로 선택합니다.
        if(filteredItemList.Count == 0)
        {
            return 5; // <<구급상자
        }
        else
        {
            int randomIndex = Random.Range(0, filteredItemList.Count);
            return filteredItemList[randomIndex];
        }
        
       
    }
}
