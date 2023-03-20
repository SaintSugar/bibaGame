using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ChunkCreator : MonoBehaviour
{
    public int chunkWidth = 16;
    public int chunkHeigth = 16;
    public Vector2 chunkPos = new Vector2(0, 0);
    public List<Block> chunkData = new List<Block>();
    // Start is called before the first frame update
    void Start()
    {
        chunkData.Clear();
        //PerlinNoise();
        //Solid();
        //Platformer();
        TavernTest();
    }
    [SerializeField]
    private bool regenerate = false;
    void Update() {
        if (regenerate) {
            regenerate = false;
            chunkData.Clear();
            //PerlinNoise();
            Solid();
        }
    }
    public void FullOfAir() {
        Block block;
        for (int z = 0; z < chunkWidth; z++) {
            for (int y = 0; y < chunkHeigth; y++) {
                for (int x = 0; x < chunkWidth; x++) {
                    block = new Block();
                    block.SetBlock(0);
                    chunkData.Add(block);
                }
            }
        }
    }
    [SerializeField]
    public float PerlinScale = 1;
    void PerlinNoise() {
        Block block;
        for (int z = 0; z < chunkWidth; z++) {
            for (int y = 0; y < chunkHeigth; y++) {
                for (int x = 0; x < chunkWidth; x++) {
                    block = new Block();

                    float noise = Mathf.PerlinNoise ((x + chunkWidth * chunkPos.x) / PerlinScale, (z + chunkWidth * chunkPos.y) / PerlinScale);
                    int H =  Mathf.RoundToInt(Mathf.Pow(noise, 4) * chunkHeigth);
                    if (y > H) block.SetBlock(0);
                    else block.SetBlock(1);
                    chunkData.Add(block);
                    //Debug.Log(block.blockID);
                }
            }
        }
    }
    public void MaxPolyChunk() {
        Block block;
        for (int z = 0; z < chunkWidth; z++) {
            for (int y = 0; y < chunkHeigth; y++) {
                for (int x = 0; x < chunkWidth; x++) {
                    block = new Block();
                    block.SetBlock((int)((x+y) % 2));
                    chunkData.Add(block);
                }
            }
        }
    }

    public void Solid() {
        Block block;
        for (int z = 0; z < chunkWidth; z++) {
            for (int y = 0; y < chunkHeigth; y++) {
                for (int x = 0; x < chunkWidth; x++) {
                    block = new Block();
                    block.SetBlock(1);
                    chunkData.Add(block);
                }
            }
        }
    }
    [SerializeField]
    private float platformerPerlinExistance = 0;
    [SerializeField]
    private float platformerPerlinHeigth = 0;
    public void Platformer() {
        Block block;
        float noise;
        int H;
        int E;
        for (int z = 0; z < chunkWidth; z++) {
            for (int y = 0; y < chunkHeigth; y++) {
                for (int x = 0; x < chunkWidth; x++) {
                    switch (y) {
                        case 0:
                            block = new Block();
                            block.SetBlock(1);
                            chunkData.Add(block);
                            break;
                        default:
                            noise = Mathf.PerlinNoise ((x + chunkWidth * chunkPos.x) / platformerPerlinExistance, (z + chunkWidth * chunkPos.y) / platformerPerlinExistance);
                            E =  Mathf.RoundToInt(Mathf.Pow(noise, 1) * 100);
                            noise = Mathf.PerlinNoise ((x + chunkWidth * chunkPos.x + 1000) / platformerPerlinHeigth, (z + chunkWidth * chunkPos.y + 1000) / platformerPerlinHeigth);
                            H =  Mathf.RoundToInt(Mathf.Pow(noise, 1) * chunkHeigth);
                            if (E > 50 && y == H) {
                                block = new Block();
                                block.SetBlock(1);
                                chunkData.Add(block);
                            }
                            else {
                                block = new Block();
                                block.SetBlock(0);
                                chunkData.Add(block);
                            }
                            
                            break;
                    }
                }
            }
        }
    }
    public void TavernTest() {
        FullOfAir();
        FillByBlock(new Vector3(0, 0, 0), new Vector3(15, 0, 15), 1);
        if (chunkPos == new Vector2(0, 0)) {
            FillByBlock(new Vector3(0, 0, 0), new Vector3(13, 8, 11), 2);
            FillByBlock(new Vector3(1, 1, 1), new Vector3(12, 7, 10), 0);
            FillByBlock(new Vector3(1, 4, 1), new Vector3(12, 4, 10), 2);
            SetBlock(0, 3, 1, 0);
        }
    }
    // Update is called once per frame
    private int getIndex(int x, int y, int z) {
        return x + y * chunkHeigth + z * chunkHeigth * chunkWidth;
    }
    public Block GetBlock(int x, int y, int z) {
        if (x < 0 || x >= chunkWidth || y >= chunkHeigth || y < 0 || z < 0 || z >= chunkWidth) {
            Block block = new Block();
            block.SetBlock(0);
            return block;
        }
        return chunkData[getIndex(x, y, z)];
    }
    void SetBlock(int blockID, int x, int y, int z) {
        Block block = new Block();
        block.SetBlock(blockID);
        chunkData[getIndex(x, y, z)] = block;
    }

    void FillByBlock(Vector3 start, Vector3 end, int blockID) {
        if ((int)start.magnitude > (int)end.magnitude) {
            Vector3 buff = start;
            start = end;
            end = buff;
        }


        for (int x = (int)start.x; x <= (int)end.x; x++) {
            for (int y = (int)start.y; y <= (int)end.y; y++) {
                for (int z = (int)start.z; z <= (int)end.z; z++) {
                    SetBlock(blockID, x, y, z);
                }
            }
        }
    }
}


public class Block {
    public bool invisible;
    public bool transparent;
    public bool solid;
    public int blockID = 0;
    public Vector2 texture;
    public void SetBlock(int blockID) {
        this.blockID = blockID;
        switch(blockID) {
            case 0:
                transparent = true;
                solid = false;
                invisible = true;
                break;
            case 1:
                transparent = false;
                solid = true;
                invisible = false;
                texture = new Vector2(0, 4);
                break;
            case 2:
                transparent = false;
                solid = true;
                invisible = false;
                texture = new Vector2(1, 3);
                break;
        }
    }
}
