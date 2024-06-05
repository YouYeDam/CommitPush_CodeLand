using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    public List<QuestData> QuestDatas; // 모든 퀘스트 데이터 리스트
    private List<Quest> allQuests = new List<Quest>(); // 모든 퀘스트 리스트
    private List<Quest> activeQuests = new List<Quest>(); // 활성화된 퀘스트 리스트
    private List<Quest> completedQuests = new List<Quest>(); // 완료된 퀘스트 리스트
    private GameObject QuestContent;  // 퀘스트 슬롯의 부모인 Content
    private QuestSlot[] QuestSlots;
    public PlayerStatus PlayerStatus;
    PlayerGetItem PlayerGetItem;
    PlayerMoney PlayerMoney;
    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
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

    // 퀘스트 초기화
    private void InitializeQuests()
    {
        foreach (var questData in QuestDatas)
        {
            Quest quest = new Quest(
                questData.Title,
                questData.Description,
                questData.ExperienceReward,
                questData.BitReward,
                questData.ItemRewards,
                questData.Objectives,
                questData.RequiredLevel // 추가된 요구 레벨
            );
            allQuests.Add(quest);
        }
    }

    // 모든 퀘스트 목표의 CurrentAmount를 초기화하는 메서드
    private void ResetAllQuestObjectives()
    {
        foreach (var quest in allQuests)
        {
            foreach (var objective in quest.Objectives)
            {
                objective.ResetCurrentAmount();
            }
        }
    }

    // 퀘스트 시작
    public void StartQuest(string questTitle)
    {
        Quest quest = allQuests.Find(q => q.Title == questTitle);
        if (quest != null && !activeQuests.Contains(quest) && !quest.IsCompleted)
        {
            // 요구 레벨을 확인
            if (PlayerStatus.PlayerLevel >= quest.RequiredLevel)
            {
                activeQuests.Add(quest);
                Debug.Log("퀘스트 시작: " + quest.Title);
                AddQuestSlot(quest); // UI에 퀘스트 추가

                // 현재 인벤토리를 체크하여 수집 목표 업데이트
                CheckInventoryForQuestItems(quest);

                // 퀘스트 목표 즉시 완료 여부 체크
                if (quest.Objectives.TrueForAll(obj => obj.Type == QuestObjective.ObjectiveType.None || obj.IsComplete()))
                {
                    SetQuestReadyToComplete(quest);
                }
            }
        }
    }
    private void AddQuestSlot(Quest quest)
    {
        bool slotFound = false;
        
        for (int i = 0; i < QuestSlots.Length; i++)
        {
            if (QuestSlots[i].QuestNameText.text == "")
            {
                QuestSlots[i].AddQuest(quest);
                slotFound = true;
                break;
            }
        }

        // 남는 퀘스트 슬롯이 없는 경우
        if (!slotFound)
        {
            activeQuests.Remove(quest); // 활성화된 퀘스트 리스트에서 제거
        }
    }

    // 퀘스트 완료 준비 상태로 설정
    private void SetQuestReadyToComplete(Quest quest)
    {
        quest.IsReadyToComplete = true;
        UpdateQuestSlot(quest); // 퀘스트 슬롯 상태 업데이트
    }

    // 퀘스트 완료
    public void CompleteQuest(string questTitle)
    {
        Quest quest = activeQuests.Find(q => q.Title == questTitle);
        if (quest != null && quest.IsReadyToComplete)
        {
            // 목표의 CurrentAmount 초기화 및 아이템 차감
            foreach (var objective in quest.Objectives)
            {
                if (objective.Type == QuestObjective.ObjectiveType.Collect)
                {
                    PlayerGetItem.InventoryScript.RemoveItem(objective.TargetName, objective.RequiredAmount);
                }
                objective.ResetCurrentAmount();
            }

            quest.IsCompleted = true;
            activeQuests.Remove(quest);
            completedQuests.Add(quest);

            // 보상 지급
            RewardPlayer(quest);
            UpdateQuestSlot(quest); // 퀘스트 슬롯 상태 업데이트
            IncrementNPCQuestIndex(questTitle);
        }
    }
    private void IncrementNPCQuestIndex(string questTitle) // 퀘스트 인덱스 증가
    {
        NPC[] npcs = FindObjectsOfType<NPC>();
        foreach (NPC npc in npcs)
        {
            for (int i = 0; i < npc.QuestsToGive.Count; i++)
            {
                if (npc.QuestsToGive[i].Title == questTitle)
                {
                    if (npc.currentQuestIndex < npc.QuestsToGive.Count - 1)
                    {
                        npc.currentQuestIndex++;
                    }
                    break;
                }
            }
        }
    }
    //퀘스트 슬롯 갱신
    private void UpdateQuestSlot(Quest quest)
    {
        foreach (QuestSlot questSlot in QuestSlots)
        {
            if (questSlot.QuestName == quest.Title)
            {
                questSlot.UpdateQuestStatus();
                break;
            }
        }
    }

    // 플레이어에게 보상 지급
    private void RewardPlayer(Quest quest)
    {
        PlayerStatus.GainEXP(quest.ExperienceReward);
        PlayerMoney.Bit += quest.BitReward;
        foreach (Item item in quest.ItemRewards)
        {
            PlayerGetItem.InventoryScript.AcquireItem(item);
            UpdateObjective(item.ItemName, 1, true);
        }
    }

    // 퀘스트 목표 업데이트
    public void UpdateObjective(string targetName, int amount, bool isItem = false)
    {
        foreach (var quest in activeQuests)
        {
            foreach (var objective in quest.Objectives)
            {
                // 몬스터 처치 목표 업데이트
                if (!isItem && objective.Type == QuestObjective.ObjectiveType.Kill && objective.TargetName == targetName)
                {
                    objective.CurrentAmount += amount;

                    // 퀘스트가 모두 완료되었는지 체크
                    if (quest.Objectives.TrueForAll(obj => obj.IsComplete()))
                    {
                        SetQuestReadyToComplete(quest);
                    }

                    break;
                }

                // 아이템 수집 목표 업데이트
                if (isItem && objective.Type == QuestObjective.ObjectiveType.Collect && objective.TargetName == targetName)
                {
                    objective.CurrentAmount += amount;

                    // 퀘스트가 모두 완료되었는지 체크
                    if (quest.Objectives.TrueForAll(obj => obj.IsComplete()))
                    {
                        SetQuestReadyToComplete(quest);
                    }

                    break;
                }
            }
        }
    }

    // 아이템 제거 목표 업데이트
    public void UpdateRemoveObjective(string itemName, int amount)
    {
        foreach (var quest in activeQuests)
        {
            foreach (var objective in quest.Objectives)
            {
                if (objective.Type == QuestObjective.ObjectiveType.Collect && objective.TargetName == itemName)
                {
                    objective.CurrentAmount -= amount;
                    if (objective.CurrentAmount < 0) objective.CurrentAmount = 0;

                    // 퀘스트가 완료된 상태에서 다시 제거될 때의 처리
                    if (quest.Objectives.TrueForAll(obj => obj.IsComplete()))
                    {
                        SetQuestReadyToComplete(quest);
                    }
                    else
                    {
                        quest.IsReadyToComplete = false;
                        UpdateQuestSlot(quest); // 퀘스트 슬롯 상태 업데이트
                    }

                    break;
                }
            }
        }
    }

    private void CheckInventoryForQuestItems(Quest quest)
    {
        foreach (var objective in quest.Objectives)
        {
            if (objective.Type == QuestObjective.ObjectiveType.Collect)
            {
                int currentAmount = PlayerGetItem.InventoryScript.GetItemAmount(objective.TargetName);
                if (currentAmount > 0)
                {
                    objective.CurrentAmount += currentAmount;

                    // 퀘스트가 모두 완료되었는지 체크
                    if (quest.Objectives.TrueForAll(obj => obj.IsComplete()))
                    {
                        SetQuestReadyToComplete(quest);
                    }
                }
            }
        }
    }
    // 특정 퀘스트를 제목으로 검색
    public Quest GetQuestByTitle(string questTitle)
    {
        return allQuests.Find(q => q.Title == questTitle);
    }

    // 특정 퀘스트가 활성화 상태인지 확인
    public bool IsQuestActive(string questTitle)
    {
        return activeQuests.Exists(q => q.Title == questTitle);
    }

    // 현재 활성화된 퀘스트 리스트 반환
    public List<Quest> GetActiveQuests()
    {
        return activeQuests;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int UINum = FindObjectsOfType<QuestManager>().Length;
        string currentSceneName = scene.name;

        if (UINum > 1 || currentSceneName == "Main Menu Scene")
        {
            Destroy(gameObject);
        }
    }
}
