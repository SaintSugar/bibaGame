using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkLoad : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Vector2 chuncsXY = new Vector2(0, 0);
    [SerializeField]
    private int viewDistance = 0;

    [SerializeField]
    private GameObject chunk;
    private List<GameObject> chunks;
    private int chunksAmount = 0;

    private float chunkSize;

    void Start()
    {
        chunks = new List<GameObject>();
        chunkSize = (chunk.GetComponent<ChunkCreator>().chunkWidth * chunk.GetComponent<ChunkRenderer>().scale);
    }

    // Update is called once per frame
    void Update()
    {
        drawDistance();
    }
    bool AddChunk(Vector2 pos) {
        for (int i = 0; i < chunks.Count; i++) {
            if (chunks[i].GetComponent<ChunkCreator>().chunkPos == pos) return false;
        }
        GameObject newChunk = Instantiate(chunk);
        newChunk.GetComponent<ChunkCreator>().chunkPos = pos;
        chunks.Add(newChunk);
        chunksAmount++;
        return true;
    }
    void drawZone() {
        for (int x = 0; x < chuncsXY.x; x++) {
            for (int y = 0; y < chuncsXY.y; y++) {

                GameObject newChunk = Instantiate(chunk);
                newChunk.GetComponent<ChunkCreator>().chunkPos = new Vector2(x, y);
                chunks.Add(newChunk);
                chunksAmount++;
            }
        }
    }
    void drawDistance() {
        Vector2 ownChunk = new Vector2(0, 0);
        ownChunk.x = Mathf.Ceil(transform.position.x / chunkSize) - 1;
        ownChunk.y = Mathf.Ceil(transform.position.z / chunkSize) - 1;
        List<Vector2> positions = new List<Vector2>();
        for (int x = -viewDistance; x <= viewDistance; x++) {
            for (int y = -viewDistance; y <= viewDistance; y++) {
                Vector2 pos = new Vector2(x, y) + ownChunk;
                AddChunk(pos);
                positions.Add(pos);
            }
        }
        for (int i = 0; i < chunks.Count; i++) {
            if (!positions.Contains(chunks[i].GetComponent<ChunkCreator>().chunkPos)) {
                Destroy(chunks[i]);
                chunks.Remove(chunks[i]);
            }
        }
    }
}
