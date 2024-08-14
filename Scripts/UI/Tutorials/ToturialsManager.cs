using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToturialsManager : Singleton<ToturialsManager>
{
    [Title("TutorialsCheck")]
    public GameObject toturialsCheck;

    [Title("Tutorials Object")]
    public GameObject objectPosition;
    public GameObject ToturialsUI;
    public GameObject frame;
    public TextMeshProUGUI tutorialsDescriptionTxt;
    public GameObject arrow;
    public string[] desTxt = new string[37];
    public GameObject[] setPosition;
    public List<Button> allButtons;

    [Title("Tutorials Bool")]
    public bool[] isClear;
    public bool isTutorials = false;


    [Title("Tutorials Phase")]
    public int phase = 0;
    private float lastClickTime;
    private const float clickDelay = 0.7f;
    private readonly Dictionary<int, int> phasePositions = new Dictionary<int, int>
    {
        {3, 0}, {5, 1}, {6, 2}, {7, 3}, {9, 4}, {10, 5},
        {13, 6}, {14, 7}, {15, 8}, {17, 9}, {18, 10}, {19, 11},
        {20, 12}, {21, 13}, {23, 14}, {24, 15}, {25, 16}, {26, 17},
        {27, 18}, {28, 19}, {30, 20}, {31, 21}, {33, 22}, {34, 22}
    };

    protected override void Awake()
    {
        base.Awake();
        InitializeDescriptions();
        InitializeisClear();
        arrow.SetActive(false);
        if (DataManager.Instance?.userInfo?.isUserTutorials ?? true)
        {
            Destroy(gameObject);
        }
        
        if (isTutorials)
        {
            Destroy(toturialsCheck);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= lastClickTime + clickDelay)
        {
            OnNextPhase();
            lastClickTime = Time.time;
        }
    }

    private void UpdateDescription()
    {
        if (phase < desTxt.Length)
        {
            tutorialsDescriptionTxt.text = desTxt[phase];
            if (phasePositions.TryGetValue(phase, out int posIndex))
            {
                arrow.SetActive(true);
                SetPosition(posIndex);
            }
            else
            {
                arrow.SetActive(false);
            }
        }
    }

    private void OnNextPhase()
    {
        if (phase >= desTxt.Length)
        {
            DataManager.Instance.userInfo.isUserTutorials = true;
            Destroy(gameObject);
            return;
        }

        if (!IsPhaseConditionMet(phase))
        {
            return;
        }
        if (phase == 18)
        {
            this.gameObject.SetActive(false); // 초기에는 숨김
        }

        phase++;
        UpdateDescription();
    }

    private bool IsPhaseConditionMet(int currentPhase)
    {

        switch (currentPhase)
        {
            case 3:
                return CheckConditionForPhase3();
            case 5:
                return CheckConditionForPhase5();
            case 6: 
                return CheckConditionForPhase6();
            case 7:
                return CheckConditionForPhase7();
            case 10:
                return CheckConditionForPhase10();
            case 13:
                return CheckConditionForPhase13();
            case 21:
                return CheckConditionForPhase21();
            case 23:
                return CheckConditionForPhase23();
            case 24:
                return CheckConditionForPhase24();
            case 25:
                return CheckConditionForPhase25();
            case 27:
                return CheckConditionForPhase27();
            case 30:
                return CheckConditionForPhase30();
            case 31:
                return CheckConditionForPhase31();
            case 35:
                return CheckConditionForPhase35();
            default:
                return true;
        }
    }

    private void SetPosition(int i)
    {
        arrow.transform.position = setPosition[i].transform.position;
    }


    private bool CheckConditionForPhase3()
    {
        if (isClear[0])
        {
            return true;
        }
        return false;
    }

    private bool CheckConditionForPhase5()
    {
        if (DataManager.Instance.characterList.Count > 0)
        {
            return true;
        }
        return false;
    }

    private bool CheckConditionForPhase6()
    {
        if (isClear[1])
        {
            return true;
        }
        return false;
    }
    private bool CheckConditionForPhase7()
    {
        if (isClear[2])
        {
            return true;
        }
        return false;
    }

    private bool CheckConditionForPhase10()
    {
        if (isClear[3])
        {
            return true;
        }
        return false;
    }
    private bool CheckConditionForPhase13()
    {
        if (isClear[5])
        {
            return true;
        }
        return false;
    }

    private bool CheckConditionForPhase21()
    {
        if (isClear[6])
        {
            return true;
        }
        return false;
    }

    private bool CheckConditionForPhase23()
    {
        if (isClear[7])
        {
            return true;
        }
        return false;
    }

    private bool CheckConditionForPhase24()
    {
        if (isClear[8])
        {
            return true;
        }
        return false;
    }

    private bool CheckConditionForPhase25()
    {
        if (isClear[9])
        {
            return true;
        }
        return false;
    }

    private bool CheckConditionForPhase27()
    {
        if (isClear[11])
        {
            return true;
        }
        return false;
    }

    private bool CheckConditionForPhase30()
    {
        if (isClear[13])
        {
            return true;
        }
        return false;
    }

    private bool CheckConditionForPhase31()
    {
        if (isClear[14])
        {
            return true;
        }
        return false;
    }

    private bool CheckConditionForPhase35()
    {
        if (isClear[15])
        {
            return true;
        }
        return false;
    }

    public void ToturialsStart()
    {
        if (DataManager.Instance?.userInfo?.isUserTutorials ?? true)
        {
            Destroy(gameObject);
        }
        else
        {
            objectPosition.SetActive(true);
            ToturialsUI.SetActive(true);
            isTutorials = true;
        }
    }

    private void InitializeDescriptions()
    {
        desTxt[0] = "안녕하세요. 이곳은 던전식당입니다. \n당신은 던전식당의 주방장이 되어 \n던전을 탐험하는 모험가들에게 식사를 제공하는 일을 하고 있습니다.";
        desTxt[1] = "식당을 방문한 모험가들을 고용해 파티를 만들고\n 그들을 통해 던전을 모두 클리어하는 것이 당신의 목표입니다.";
        desTxt[2] = "그럼 모험가를 고용해 보겠습니다.";
        desTxt[3] = "모험가 고용 버튼을 눌러 모험가 고용 메뉴에 들어갈 수 있습니다."; //
        desTxt[4] = "이곳에서 모험가들을 고용할 수 있습니다. 모험가 이미지를 누르면 모험가의 상세 정보를 볼 수도 있습니다.";
        desTxt[5] = "모험가를 한 명 고용해 보세요.";//
        desTxt[6] = "모험가 고용을 완료했습니다. 뒤로가기 버튼을 눌러 식당으로 돌어갈 수 있습니다.";//
        desTxt[7] = "고용한 모험가를 이용해 던전을 돌파해 봅시다. 이 버튼을 눌러 던전으로 들어갈 수 있습니다.";//
        desTxt[8] = "던전에 입장하기 전 던전 정보를 확인할 수 있는 화면입니다.";
        desTxt[9] = "이곳에 마우스를 가져다 대 등장하는 몬스터 정보를 확인할 수 있습니다.";//
        desTxt[10] = "입장 버튼을 눌러 계속 진행합니다.";//
        desTxt[11] = "파티 구성 화면입니다. 이 화면에서 고용한 모험가들로 던전에 진입할 파티를 구성할 수 있습니다.";
        desTxt[12] = "우측의 목록에서 모험가를 드래그하거나 클릭한 뒤 좌측의 파티 편성 구역에 모험가를 배치해보세요.";
        desTxt[13] = "배치가 완료되었습니다. 파티를 구성하였으니 진짜 던전에 입장할 차례입니다.";//
        desTxt[14] = "던전 안의 몬스터를 잡으면 요리 게이지를 쌓을 수 있습니다.";//
        desTxt[15] = "요리 게이지를 전부 쌓으면 특별한 효과를 가진 요리 카드를 얻을 수 있습니다.";//
        desTxt[16] = "요리 카드는 사용하면 특별한 효과를 발동하고 만복도 게이지를 회복합니다.";//
        desTxt[17] = "만복도 게이지의 상태에 따라 모든 아군이 다양한 효과를 얻습니다. 만복도 게이지가 공복 상태가 되지 않도록 적절한 타이밍에 카드를 사용해야 합니다.";
        desTxt[18] = "모든 웨이브를 돌파하면 해당 스테이지를 클리어하게 됩니다.";//
        desTxt[19] = "스테이지를 클리어하면 골드와 재료, 메달을 획득할 수 있습니다. 스테이지에서 패배하더라도 골드와 재료를 획득할 수 있지만, 메달은 승리 시에만 획득할 수 있습니다.";//
        desTxt[20] = "버튼을 눌러 다음 스테이지로 이동하거나 식당으로 복귀할 수 있습니다. ";//
        desTxt[21] = "일단 식당으로 복귀해 봅시다.";//
        desTxt[22] = "식당에 방문한 모험가나 손님들의 머리 위 말풍선을 누르면 주문을 받을 수 있습니다.";
        desTxt[23] = "던전에서 획득한 요리 재료를 사용해 레시피를 해금하고 강화할 수 있습니다.";//
        desTxt[24] = "레시피 목록에서 현재까지 해금한 요리 목록을 볼 수 있습니다. 또한 레시피를 눌러 조건을 만족한 레시피를 강화할 수 있습니다. 레시피를 강화하면 전투 시 등장하는 요리 카드의 성능과, 손님들의 주문으로 획득하는 골드가 증가합니다.";//
        desTxt[25] = "식당 강화 버튼을 눌러 식당 강화 메뉴를 열 수 있습니다.";//
        desTxt[26] = "골드 강화 페이지에서는 골드를 사용해 모험가들의 능력치를 강화할 수 있습니다.";//
        desTxt[27] = "메달 강화 페이지에서는 메달을 사용해 게임 진행에 도움을 주는 특별한 효과를 해금하거나 강화할 수 있습니다.";//
        desTxt[28] = "파티 편성 최대 인원 수를 늘려봅시다.\r\n";//
        desTxt[29] = "메달을 어느정도 사용하면 식당 레벨이 함께 증가합니다. 메달을 더 많이 획득하고 소비해 식당을 업그레이드해보세요.";
        desTxt[30] = "모험가 버튼을 눌러 고용한 모험가 목록을 볼 수 있습니다.";//
        desTxt[31] = "모험가 관리 메뉴입니다. 모험가를 선택하면 해당 모험가의 상세 정보를 볼 수 있습니다.";//
        desTxt[32] = "이곳에서 모험가의 능력치와 스킬 정보를 확인할 수 있습니다. ";
        desTxt[33] = "단 우측의 해고 버튼을 눌러 모험가를 목록에서 지울 수 있습니다. 모험가를 해고하면 해당 모험가를 더 이상 사용할 수 없습니다.";//
        desTxt[34] = "레벨 업 버튼을 눌러 모험가의 레벨을 올릴 수 있습니다. 레벨 업으로 올릴 수 있는 최대 레벨은 식당 레벨과 동일합니다";//
        desTxt[35] = "현재 식당 레벨은 2로, 레벨 업으로 올릴 수 있는 모험가의 최대 레벨도 2입니다. 레벨 업 버튼을 눌러 모험가 레벨을 올려보세요.";
        desTxt[36] = "이상으로 튜토리얼을 종료하겠습니다 감사합니다";
    }

    private void InitializeisClear()
    {
        isClear = new bool[16];

        isClear[0] = false; //EmploymentOpen
        isClear[1] = false; //EmploymentBack
        isClear[2] = false; //DungeonIn
        isClear[3] = false; //CombatIn
        isClear[4] = false; //EntryCharacter
        isClear[5] = false; //EnterCombat
        isClear[6] = false; //ReturnRes
        isClear[7] = false; //EnterRecipe
        isClear[8] = false; //ClickRecipe
        isClear[9] = false; //EnterRes
        isClear[10] = false; //ClickGold
        isClear[11] = false; //ClickMedal
        isClear[12] = false; //PartyCount
        isClear[13] = false; //CharacterManagement
        isClear[14] = false; //EnterCharacterInfo
        isClear[15] = false; //CharacterLevelUp
    }
}
