using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    public List<QuestData> QuestDatas; // 모든 퀘스트 데이터 리스트
    List<Quest> AllQuests = new List<Quest>(); // 모든 퀘스트 리스트
    List<Quest> ActiveQuests = new List<Quest>(); // 활성화된 퀘스트 리스트
    List<Quest> CompletedQuests = new List<Quest>(); // 완료된 퀘스트 리스트
    GameObject QuestContent;  // 퀘스트 슬롯의 부모인 Content
    QuestSlot[] QuestSlots;
    public PlayerStatus PlayerStatus;
    PlayerGetItem PlayerGetItem;
    PlayerMoney PlayerMoney;
    public NPCQuestState NpcQuestState;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        NpcQuestState = gameObject.AddComponent<NPCQuestState>(); // NPCQuestState 추가
    }

    void Start()
    {
        InitializeQuests();
        ResetAllQuestObjectives();
        QuestContent = GameObject.Find("UIManager").transform.Find("Quest/Scroll View/Viewport/QuestContent").gameObject;
        QuestSlots = QuestContent.GetComponentsInChildren<QuestSlot>();
        PlayerGetItem = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerGetItem>();
        PlayerMoney = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoney>();
        PlayerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
    }

    void InitializeQuests() { // 퀘스트 초기화
        foreach (var questData in QuestDatas)
        {
            Quest quest = new Quest(
                questData.Title,
                questData.Description,
                questData.ExperienceReward,
                questData.BitReward,
                questData.ItemRewards,
                questData.Objectives,
                questData.RequiredLevel,
                questData.Place,
                questData.NPCName
            );
            AllQuests.Add(quest); // 모든 퀘스트 리스트에 추가
        }
    }

    void ResetAllQuestObjectives() { // 모든 퀘스트 목표의 현재 수량을 초기화
        foreach (var quest in AllQuests)
        {
            foreach (var objective in quest.Objectives)
            {
                objective.ResetCurrentAmount();
            }
        }
    }
    public void StartQuest(string QuestTitle) { // 퀘스트 시작
        Quest quest = AllQuests.Find(q => q.Title == QuestTitle); // 모든 퀘스트 리스트에서 해당하는 제목을 가진 퀘스트를 할당

        if (quest != null && !ActiveQuests.Contains(quest) && !quest.IsCompleted) { // 퀘스트가 이미 활성화 상태거나 완료된 상태가 아닐 경우에만 실행
            if (PlayerStatus.PlayerLevel >= quest.RequiredLevel) { // 요구 레벨을 확인
                ActiveQuests.Add(quest); // 활성화된 퀘스트 리스트에 추가
                AddQuestSlot(quest); // UI에 퀘스트 추가
                
                CheckInventoryForQuestItems(quest); // 현재 인벤토리를 체크하여 수집 목표 업데이트

                if (quest.Objectives.TrueForAll(obj => obj.Type == QuestObjective.ObjectiveType.None || obj.IsComplete())) { // 퀘스트 목표 즉시 완료 여부 체크
                    SetQuestReadyToComplete(quest);
                }
            }
        }
    }
    
    void AddQuestSlot(Quest quest) { // 퀘스트 UI창에 퀘스트 추가
        bool RemainingSlot = false;
        
        for (int i = 0; i < QuestSlots.Length; i++) {
            if (QuestSlots[i].QuestNameText.text == "") { // 퀘스트 슬롯이 비어있는 경우
                QuestSlots[i].AddQuest(quest);
                RemainingSlot = true;
                break;
            }
        }

        if (!RemainingSlot) { // 남는 퀘스트 슬롯이 없는 경우
            ActiveQuests.Remove(quest); // 활성화된 퀘스트 리스트에서 제거 (퀘스트를 못받게 함)
        }
    }

    void SetQuestReadyToComplete(Quest quest) { // 퀘스트 완료 준비 상태로 설정
        quest.IsReadyToComplete = true;
        UpdateQuestSlot(quest); // 퀘스트 슬롯 상태 업데이트
    }

    public void CompleteQuest(string questTitle) { // 퀘스트 완료
        Quest quest = ActiveQuests.Find(q => q.Title == questTitle); // 활성화된 퀘스트 리스트에서 해당하는 제목을 가진 퀘스트를 할당
        if (quest != null && quest.IsReadyToComplete) {
            foreach (var objective in quest.Objectives) { // 목표의 CurrentAmount 초기화 및 아이템 차감
                if (objective.Type == QuestObjective.ObjectiveType.Collect) {
                    PlayerGetItem.InventoryScript.RemoveItem(objective.TargetName, objective.RequiredAmount);
                }
                objective.ResetCurrentAmount();
            }

            quest.IsCompleted = true;
            ActiveQuests.Remove(quest);
            CompletedQuests.Add(quest);

            RewardPlayer(quest); // 보상 지급
            UpdateQuestSlot(quest); // 퀘스트 슬롯 상태 업데이트
            IncrementNPCQuestIndex(quest.NPCName);
        }
    }

    void IncrementNPCQuestIndex(string NpcName) { // 퀘스트 인덱스 증가
        int CurrentQuestIndex = NpcQuestState.GetQuestIndex(NpcName);
        NpcQuestState.SetQuestIndex(NpcName, CurrentQuestIndex + 1);
    }
    
    void UpdateQuestSlot(Quest quest) { //퀘스트 슬롯 갱신
        foreach (QuestSlot questSlot in QuestSlots)
        {
            if (questSlot.QuestName == quest.Title)
            {
                questSlot.UpdateQuestStatus();
                break;
            }
        }
    }
    
    void RewardPlayer(Quest quest) { // 플레이어에게 보상 지급
        PlayerStatus.GainEXP(quest.ExperienceReward);
        PlayerMoney.Bit += quest.BitReward;
        foreach (Item item in quest.ItemRewards)
        {
            PlayerGetItem.InventoryScript.AcquireItem(item);
            UpdateObjective(item.ItemName, 1, true);
        }
    }

    public void UpdateObjective(string TargetName, int Amount, bool IsItem = false) { // 퀘스트 목표 업데이트
        foreach (var Quest in ActiveQuests)
        {
            foreach (var Objective in Quest.Objectives)
            {
                if (!IsItem && Objective.Type == QuestObjective.ObjectiveType.Kill && Objective.TargetName == TargetName) { // 몬스터 처치 목표 업데이트
                    if (Objective.CurrentAmount <= Objective.RequiredAmount) {
                        Objective.CurrentAmount += Amount;
                    }

                    if (Quest.Objectives.TrueForAll(obj => obj.IsComplete())) { // 퀘스트가 모두 완료되었는지 체크
                        SetQuestReadyToComplete(Quest);
                    }

                    break;
                }

                if (IsItem && Objective.Type == QuestObjective.ObjectiveType.Collect && Objective.TargetName == TargetName) { // 아이템 수집 목표 업데이트
                    Objective.CurrentAmount += Amount;

                    if (Quest.Objectives.TrueForAll(obj => obj.IsComplete())) { // 퀘스트가 모두 완료되었는지 체크
                        SetQuestReadyToComplete(Quest);
                    }

                    break;
                }
            }
        }
    }

    public void UpdateRemoveObjective(string ItemName, int Amount) { // 아이템 제거 목표 업데이트
        foreach (var Quest in ActiveQuests) {
            foreach (var Objective in Quest.Objectives) {
                if (Objective.Type == QuestObjective.ObjectiveType.Collect && Objective.TargetName == ItemName) {
                    Objective.CurrentAmount -= Amount;

                    if (Objective.CurrentAmount < 0) {
                        Objective.CurrentAmount = 0;
                    }

                    if (Quest.Objectives.TrueForAll(obj => obj.IsComplete())) { // 아이템 제거 후에도 완료가 가능한 상태인지 확인
                        SetQuestReadyToComplete(Quest);
                    }
                    else { // 아이템 제거 후 완료가 불가능한 상태라면 해당 상태 반영
                        Quest.IsReadyToComplete = false;
                        UpdateQuestSlot(Quest);
                    }

                    break;
                }
            }
        }
    }

    void CheckInventoryForQuestItems(Quest quest) {
        foreach (var objective in quest.Objectives) {
            if (objective.Type == QuestObjective.ObjectiveType.Collect) {
                int CurrentAmount = PlayerGetItem.InventoryScript.GetItemAmount(objective.TargetName); // 퀘스트 아이템을 플레이어가 이미 얼마나 가지고 있는지 확인

                if (CurrentAmount > 0) { // 가지고 있는 게 있다면 현재 수량에 그만큼 반영
                    objective.CurrentAmount += CurrentAmount;
                    
                    if (quest.Objectives.TrueForAll(obj => obj.IsComplete())) { // 퀘스트가 모두 완료되었는지 체크
                        SetQuestReadyToComplete(quest);
                    }
                }
            }
        }
    }

    public Quest GetQuestByTitle(string questTitle) { // 특정 퀘스트를 제목으로 검색
        return AllQuests.Find(q => q.Title == questTitle);
    }

    
    public bool IsQuestActive(string questTitle) { // 특정 퀘스트가 활성화 상태인지 확인
        return ActiveQuests.Exists(q => q.Title == questTitle);
    }

    public List<Quest> GetActiveQuests() { // 현재 활성화된 퀘스트 리스트 반환
        return ActiveQuests;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode Mode) {
        int UINum = FindObjectsOfType<QuestManager>().Length;
        string CurrentSceneName = scene.name;

        if (UINum > 1 || CurrentSceneName == "Main Menu Scene") { // 퀘스트 매니저가 1개만 존재하도록
            Destroy(gameObject);
        }
    }
}
