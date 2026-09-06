using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Mission
{
    public enum MissionType { RunTime, CollectCoins, JumpCount, KillBoss }

    public MissionType type;
    public int goalValue;
    public int currentValue;
    public bool isCompleted => currentValue >= goalValue;
}

public class MissionManager : MonoBehaviour
{
    private StageController stageController;

    public List<Mission> missions = new List<Mission>();

    public Text[] missionTexts;     // 미션 설명 텍스트 UI
    public GameObject[] checkmarks; // 미션 체크 표시 UI

    public int stageIndex = 1;

    void Start()
    {
        stageController = FindObjectOfType<StageController>();
        InitMissions(stageIndex);
    }

    void Update()
    {
        CheckMissionCompletion();
        UpdateMissionTexts();
    }

    public void InitMissions(int stage)
    {
        missions.Clear();

        if (stage == 1)
        {
            missions.Add(new Mission { type = Mission.MissionType.RunTime, goalValue = Random.Range(10, 20), currentValue = 0 });
            missions.Add(new Mission { type = Mission.MissionType.CollectCoins, goalValue = Random.Range(500, 1500), currentValue = 0 });
            missions.Add(new Mission { type = Mission.MissionType.JumpCount, goalValue = Random.Range(10, 20), currentValue = 0 });
        }

        else if (stage == 2)
        {
            missions.Add(new Mission { type = Mission.MissionType.RunTime, goalValue = Random.Range(15, 30), currentValue = 0 });
            missions.Add(new Mission { type = Mission.MissionType.CollectCoins, goalValue = Random.Range(1500, 2000), currentValue = 0 });
            missions.Add(new Mission { type = Mission.MissionType.JumpCount, goalValue = Random.Range(10, 20), currentValue = 0 });
        }

        else if (stage == 3)
        {
            missions.Add(new Mission { type = Mission.MissionType.KillBoss, goalValue = 1, currentValue = 0 });
        }

        UpdateMissionTexts();  // 텍스트 초기화
        UpdateCheckmarks();    // 체크 UI 초기화
    }

    public void AddProgress(Mission.MissionType type, int amount)
    {
        foreach (var mission in missions)
        {
            if (mission.type == type && !mission.isCompleted)
            {
                mission.currentValue += amount;
                break;
            }
        }
    }

    void CheckMissionCompletion()
    {
        for (int i = 0; i < missions.Count; i++)
        {
            if (missions[i].isCompleted)
            {
                if (checkmarks != null && i < checkmarks.Length)
                    checkmarks[i].SetActive(true);

                if (missionTexts != null && i < missionTexts.Length)
                    missionTexts[i].color = Color.green;
            }
        }

        if (AllMissionsCompleted())
        {
            //Debug.Log("모든 미션 완료! 스테이지 클리어 가능!");

            // 다음 스테이지로 이동 (최대 3까지만)
            if (stageController != null && stageIndex < 3)
            {
                int nextStage = stageIndex + 1;
                string stageName = $"Stage {nextStage}";
                stageController.ApplyLevelChange(nextStage - 1, stageName); // index는 0부터니까 -1
                stageIndex = nextStage;

                InitMissions(stageIndex); // 다음 스테이지 미션 초기화
            }
        }
    }

    bool AllMissionsCompleted()
    {
        foreach (var mission in missions)
        {
            if (!mission.isCompleted) return false;
        }
        return true;
    }

    void UpdateMissionTexts()
    {
        for (int i = 0; i < missions.Count; i++)
        {
            if (i < missionTexts.Length)
            {
                missionTexts[i].text = GetMissionProgressText(missions[i]);
                missionTexts[i].color = Color.white;
            }
        }
    }


    void UpdateCheckmarks()
    {
        for (int i = 0; i < checkmarks.Length; i++)
        {
            checkmarks[i].SetActive(false);
        }
    }

    string GetMissionProgressText(Mission mission)
    {
        string unit = "";
        string label = "";

        switch (mission.type)
        {
            case Mission.MissionType.RunTime:
                unit = "초";
                label = "달리기";
                break;

            case Mission.MissionType.CollectCoins:
                unit = "코인";
                label = "획득";
                break;

            case Mission.MissionType.JumpCount:
                unit = "번";
                label = "점프";
                break;

            case Mission.MissionType.KillBoss:
                return $"우주 괴물 처치 : {mission.currentValue}/{mission.goalValue}";
        }

        return $"{mission.goalValue}{unit} {label} : {mission.currentValue}{unit}";
    }
}