// Fixed MapCreator.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    public enum TYPE
    {
        NONE = -1,
        FLOOR = 0,
        HOLE,
        NUM,
    };
};

public class MapCreator : MonoBehaviour
{
    public static float BLOCK_WIDTH = 1.0f;
    public static float BLOCK_HEIGHT = 0.2f;
    public static int BLOCK_NUM_IN_SCREEN = 35;

    private struct FloorBlock
    {
        public bool is_created;
        public Vector3 position;
    };

    private FloorBlock last_block;
    private PlayerControl player = null;
    private LevelControl level_control = null;
    public TextAsset level_data_text = null;
    private BlockCreator block_creator;
    private EarthStageManager game_root = null;

    private void Create_floor_block()
    {
        Vector3 block_position;

        if (!this.last_block.is_created)
        {
            block_position = this.player.transform.position;
            block_position.x -= BLOCK_WIDTH * ((float)BLOCK_NUM_IN_SCREEN / 2.0f);
            block_position.y = 0.0f;
        }
        else
        {
            block_position = this.last_block.position;
        }

        block_position.x += BLOCK_WIDTH;

        this.level_control.update(this.game_root.getPlayTime());

        block_position.y = level_control.current_block.height * BLOCK_HEIGHT;

        LevelControl.CreationInfo current = this.level_control.current_block;

        if (current.block_type == Block.TYPE.FLOOR)
        {
            this.block_creator.createBlock(block_position);
        }

        this.last_block.position = block_position;
        this.last_block.is_created = true;
    }

    public bool IsDelete(GameObject block_object)
    {
        float left_limit = this.player.transform.position.x
                          - BLOCK_WIDTH * ((float)BLOCK_NUM_IN_SCREEN / 2.0f);
        return (block_object.transform.position.x < left_limit);
    }

    void Update()
    {
        float block_generate_x = this.player.transform.position.x;
        block_generate_x += BLOCK_WIDTH * ((float)BLOCK_NUM_IN_SCREEN + 1) / 2.0f;

        while (this.last_block.position.x < block_generate_x)
        {
            this.Create_floor_block();
        }
    }

    void Start()
    {
        this.level_control = FindFirstObjectByType<LevelControl>();
        if (this.level_control == null)
        {
            Debug.LogError("LevelControl을 찾을 수 없습니다.");
            return;
        }

        this.level_control.initialize();

        this.player = FindFirstObjectByType<PlayerControl>();
        this.player.level_control = this.level_control;

        this.last_block.is_created = false;
        this.block_creator = this.gameObject.GetComponent<BlockCreator>();
        this.level_control.loadLevelData(this.level_data_text);
        this.game_root = FindFirstObjectByType<EarthStageManager>();
    }
}