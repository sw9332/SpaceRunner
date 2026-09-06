using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StageController : MonoBehaviour
{
    private LevelControl levelControl;
    [SerializeField] private BOSS boss;
    public Text stageText;
    public float stageTime = 1.5f;
    private int currentStage = 0;

    private bool isShowingStageText = false;
    private int lastAppliedStageIndex = -1;

    public void ApplyLevelChange(int levelIndex, string stageName)
    {
        if (lastAppliedStageIndex == levelIndex) return;

        lastAppliedStageIndex = levelIndex;

        if (levelControl == null) return;

        while (levelControl.level_datas.Count <= levelIndex)
        {
            levelControl.level_datas.Add(new LevelData());
        }

        LevelData levelData = levelControl.level_datas[levelIndex];

        switch (levelIndex)
        {
            case 0:
                levelData.end_time = 15f;
                levelData.player_speed = 5f;
                levelData.floor_count.min = 10;
                levelData.floor_count.max = 30;
                levelData.hole_count.min = 3;
                levelData.hole_count.max = 6;
                levelData.height_diff.min = -3;
                levelData.height_diff.max = 2;
                break;

            case 1:
                levelData.end_time = 20f;
                levelData.player_speed = 10f;
                levelData.floor_count.min = 10;
                levelData.floor_count.max = 15;
                levelData.hole_count.min = 5;
                levelData.hole_count.max = 8;
                levelData.height_diff.min = -4;
                levelData.height_diff.max = 2;
                break;

            case 2:
                levelData.end_time = 20f;
                levelData.player_speed = 15f;
                levelData.floor_count.min = 2;
                levelData.floor_count.max = 5;
                levelData.hole_count.min = 500;
                levelData.hole_count.max = 500;
                levelData.height_diff.min = -5;
                levelData.height_diff.max = 1;

                if (boss != null) boss.TriggerBossAppearance();
                break;
        }

        currentStage = levelIndex + 1;

        StartCoroutine(ShowStageText(stageName));

        levelControl.level_datas[levelIndex] = levelData;
        levelControl.level = levelIndex;

        levelControl.clear_next_block(ref levelControl.next_block);
        levelControl.update_level(ref levelControl.next_block, levelControl.current_block, 0);
        levelControl.block_count = 10;
    }

    private IEnumerator ShowStageText(string stageName)
    {
        isShowingStageText = true;

        if (stageName == "Stage 1") stageText.text = stageName+" (중력 적용)";
        if (stageName == "Stage 2") stageText.text = stageName + " (중력 적용)";
        if (stageName == "Stage 3") stageText.text = stageName + " (중력 해제)";
        stageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        stageText.gameObject.SetActive(false);

        isShowingStageText = false;
    }

    void Start()
    {
        levelControl = FindFirstObjectByType<LevelControl>();
        StartCoroutine(ShowStageText("Stage 1"));
    }
}