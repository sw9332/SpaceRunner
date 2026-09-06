using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData
{
    public struct Range
    {
        public int min;
        public int max;
    }

    public float end_time;
    public float player_speed;
    public Range floor_count;
    public Range hole_count;
    public Range height_diff;

    public LevelData()
    {
        this.end_time = 15.0f;
        this.player_speed = 5.0f;
        this.floor_count.min = 5;
        this.floor_count.max = 10;
        this.hole_count.min = 5;
        this.hole_count.max = 10;
        this.height_diff.min = -5;
        this.height_diff.max = 5;
    }
}

public class LevelControl : MonoBehaviour
{
    public List<LevelData> level_datas = new List<LevelData>();
    public int HEIGHT_MAX = 20;
    public int HEIGHT_MIN = -4;

    public struct CreationInfo
    {
        public Block.TYPE block_type;
        public int max_count;
        public int height;
        public int current_count;
    }

    public CreationInfo previous_block;
    public CreationInfo current_block;
    public CreationInfo next_block;
    public int block_count = 0;
    public int level = 0;

    public void initialize()
    {
        this.block_count = 0;
        clear_next_block(ref previous_block);
        clear_next_block(ref current_block);
        clear_next_block(ref next_block);
    }

    public void clear_next_block(ref CreationInfo block)
    {
        block.block_type = Block.TYPE.FLOOR;
        block.max_count = 1;
        block.height = 0;
        block.current_count = 0;
    }

    public void loadLevelData(TextAsset level_data_text)
    {
        string[] lines = level_data_text.text.Split('\n');

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] words = line.Split();
            int n = 0;
            LevelData level_data = new LevelData();

            foreach (var word in words)
            {
                if (word.StartsWith("#")) break;
                if (string.IsNullOrWhiteSpace(word)) continue;

                switch (n)
                {
                    case 0: level_data.end_time = float.Parse(word); break;
                    case 1: level_data.player_speed = float.Parse(word); break;
                    case 2: level_data.floor_count.min = int.Parse(word); break;
                    case 3: level_data.floor_count.max = int.Parse(word); break;
                    case 4: level_data.hole_count.min = int.Parse(word); break;
                    case 5: level_data.hole_count.max = int.Parse(word); break;
                    case 6: level_data.height_diff.min = int.Parse(word); break;
                    case 7: level_data.height_diff.max = int.Parse(word); break;
                }

                n++;
            }

            if (n >= 8)
                level_datas.Add(level_data);
        }

        if (level_datas.Count == 0)
        {
            Debug.LogError("[LevelData] No valid data.");
            level_datas.Add(new LevelData());
        }
    }

    public void update_level(ref CreationInfo current, CreationInfo previous, float passage_time)
    {
        LevelData level_data = level_datas[level];

        if (previous.block_type == Block.TYPE.FLOOR)
        {
            current.block_type = Block.TYPE.HOLE;
            current.max_count = Random.Range(level_data.hole_count.min, level_data.hole_count.max + 1);
            current.height = previous.height;
        }
        else
        {
            current.block_type = Block.TYPE.FLOOR;
            current.max_count = Random.Range(level_data.floor_count.min, level_data.floor_count.max + 1);

            int min = Mathf.Clamp(previous.height + level_data.height_diff.min, HEIGHT_MIN, HEIGHT_MAX);
            int max = Mathf.Clamp(previous.height + level_data.height_diff.max, HEIGHT_MIN, HEIGHT_MAX);

            current.height = Random.Range(min, max + 1);
        }

        //Debug.Log($"[Block] Stage {level + 1} | Type: {current.block_type}, Count: {current.max_count}, Height: {current.height}");
    }

    public void update(float passage_time)
    {
        current_block.current_count++;

        if (current_block.current_count >= current_block.max_count)
        {
            previous_block = current_block;
            current_block = next_block;
            clear_next_block(ref next_block);
            update_level(ref next_block, current_block, passage_time);
        }

        block_count++;
    }

    public float getPlayerSpeed()
    {
        return level_datas[level].player_speed;
    }

    public float current_speed = 0.0f;
    public LevelControl level_control = null;
}