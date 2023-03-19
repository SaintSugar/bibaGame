using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkRenderer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private ChunkCreator chunkData;
    [SerializeField]
    void Start()
    {   

        mesh = GetComponent<MeshFilter> ().mesh;
        col = GetComponent<MeshCollider> ();
        
        buildMeshes();
        
        
        Render();
    }

    void buildMeshes() {
        transform.position = new Vector3(chunkData.chunkPos.x, 0, chunkData.chunkPos.y) * scale * chunkData.chunkWidth/2;

        Block block;
        for (int x = 0; x < chunkData.chunkWidth; x++) {
            for (int y = 0; y < chunkData.chunkHeigth; y++) {
                for (int z = 0; z < chunkData.chunkWidth; z++) {
                    block = chunkData.GetBlock(x, y, z);
                    if (block.blockID == 0) continue;
                    if (chunkData.GetBlock(x, y + 1, z).blockID == 0) AddTop(x, y, z, block);
                    if (chunkData.GetBlock(x, y - 1, z).blockID == 0) AddBottom(x, y, z, block);
                    if (chunkData.GetBlock(x + 1, y, z).blockID == 0) AddRight(x, y, z, block);
                    if (chunkData.GetBlock(x - 1, y, z).blockID == 0) AddLeft(x, y, z, block);
                    if (chunkData.GetBlock(x, y, z + 1).blockID == 0) AddFront(x, y, z, block);
                    if (chunkData.GetBlock(x, y, z - 1).blockID == 0) AddBack(x, y, z, block);
                }                
            }
        }
    }

    private List<Vector3> newVertices = new List<Vector3>();
    private List<int> newTriangles = new List<int>();
    private List<Vector2> newUV = new List<Vector2>();
    private Mesh mesh;


    public List<Vector3> colVertices = new List<Vector3>();
    public List<int> colTriangles = new List<int>();
    private int colCount;

    private MeshCollider col;
    [SerializeField]
    private bool reRender = false;
    void Update() 
    {
        if (reRender) {
            reRender = false;
            buildMeshes();
            Render();
        }

    }
    public void Render(){
        mesh.Clear ();
        mesh.vertices = newVertices.ToArray();
        mesh.triangles = newTriangles.ToArray();
        mesh.uv = newUV.ToArray();
        mesh.Optimize ();
        mesh.RecalculateNormals ();


        Mesh newMesh = new Mesh();
        newMesh.vertices = colVertices.ToArray();
        newMesh.triangles = colTriangles.ToArray();
        col.sharedMesh= newMesh;

        colVertices.Clear();
        colTriangles.Clear();
        colCount=0;


        sidesAmount = 0;
        newVertices.Clear();
        newTriangles.Clear();
        newUV.Clear();
    }


    [SerializeField]
    public float scale;
    private int sidesAmount = 0;
    private float tUnit = 0.2f;

    private void AddSide(float x, float y, float z, float [] delta, int [] order, Block block) {
        
        x = transform.position.x + x * scale;
        y = transform.position.y + y * scale;
        z = transform.position.z + z * scale;

        if (!block.invisible) {

            newVertices.Add( new Vector3 (x + delta[0], y + delta[1], z + delta[2]));
            newVertices.Add( new Vector3 (x + delta[3], y + delta[4], z + delta[5]));
            newVertices.Add( new Vector3 (x + delta[6], y + delta[7], z + delta[8]));
            newVertices.Add( new Vector3 (x + delta[9], y + delta[10], z + delta[11]));

            newTriangles.Add(order[0] + sidesAmount * 4);
            newTriangles.Add(order[1] + sidesAmount * 4);
            newTriangles.Add(order[2] + sidesAmount * 4);
            newTriangles.Add(order[3] + sidesAmount * 4);
            newTriangles.Add(order[4] + sidesAmount * 4);
            newTriangles.Add(order[5] + sidesAmount * 4);

            newUV.Add(new Vector2 (tUnit * block.texture.x, tUnit * block.texture.y + tUnit));
            newUV.Add(new Vector2 (tUnit * block.texture.x + tUnit, tUnit * block.texture.y + tUnit));
            newUV.Add(new Vector2 (tUnit * block.texture.x + tUnit, tUnit * block.texture.y));
            newUV.Add(new Vector2 (tUnit * block.texture.x, tUnit * block.texture.y));

            sidesAmount++;
        }
        if (block.solid) {
            colVertices.Add( new Vector3 (x + delta[0], y + delta[1], z + delta[2]));
            colVertices.Add( new Vector3 (x + delta[3], y + delta[4], z + delta[5]));
            colVertices.Add( new Vector3 (x + delta[6], y + delta[7], z + delta[8]));
            colVertices.Add( new Vector3 (x + delta[9], y + delta[10], z + delta[11]));

            colTriangles.Add(order[0] + colCount * 4);
            colTriangles.Add(order[1] + colCount * 4);
            colTriangles.Add(order[2] + colCount * 4);
            colTriangles.Add(order[3] + colCount * 4);
            colTriangles.Add(order[4] + colCount * 4);
            colTriangles.Add(order[5] + colCount * 4);

            colCount++;
        }
        
    }
    private void AddTop(float x, float y, float z, Block block) {
        float [] delta = {  
                            scale, scale, scale,
                            0    , scale, scale,
                            0    , scale, 0    ,
                            scale, scale, 0    ,
        };
        int [] order = {1, 0, 3, 2, 1, 3};
        AddSide(x, y, z, delta, order, block);

        //colVertices.Add( new Vector3 (x  , y + scale, z));
        //colVertices.Add( new Vector3 (x + scale , y + scale, z));
        //colVertices.Add( new Vector3 (x + scale , y + scale, z + scale));
        //colVertices.Add( new Vector3 (x  , y + scale, z + scale));

        //colTriangles.Add(colCount*4);
        //colTriangles.Add((colCount*4)+3);
        //colTriangles.Add((colCount*4)+1);
        //colTriangles.Add((colCount*4)+2);
        //colTriangles.Add((colCount*4)+1);
        //colTriangles.Add((colCount*4)+3);

        //colCount++;
    }
    private void AddBack(float x, float y, float z, Block block) {
        float [] delta = {  
                            scale, scale, 0    ,
                            0    , scale, 0    ,
                            0    , 0    , 0    ,
                            scale, 0    , 0    
        };
        int [] order = {0, 3, 1, 2, 1, 3};
        AddSide(x, y, z, delta, order, block);
    }
    private void AddRight(float x, float y, float z, Block block) {
        float [] delta = {  
                            scale, scale, 0,
                            scale, scale, scale,
                            scale, 0    , scale,
                            scale, 0    , 0    
        };
        int [] order = {3, 0, 1, 1, 2, 3};
        AddSide(x, y, z, delta, order, block);
    }
    private void AddLeft(float x, float y, float z, Block block) {
        float [] delta = {  
                            0    , scale, 0    ,
                            0    , scale, scale,
                            0    , 0    , scale,
                            0    , 0    , 0
        };
        int [] order = {0, 3, 1, 2, 1, 3};
        AddSide(x, y, z, delta, order, block);
    }
    private void AddFront(float x, float y, float z, Block block) {
        float [] delta = {  
                            scale, scale, scale,
                            0    , scale, scale,
                            0    , 0    , scale,
                            scale, 0    , scale
        };
        int [] order = {3, 0, 1, 1, 2, 3};
        AddSide(x, y, z, delta, order, block);
    }
    private void AddBottom(float x, float y, float z, Block block) {
        float [] delta = {  
                            scale, 0    , scale,
                            0    , 0    , scale,
                            0    , 0    , 0    ,
                            scale, 0    , 0    
        };
        int [] order = {0, 1, 3, 1, 2, 3};
        AddSide(x, y, z, delta, order, block);
    }
    private void AddBlock(float x, float y, float z, Block block) {
        AddFront(x, y, z, block);
        AddBack(x, y, z, block);
        AddRight(x, y, z, block);
        AddLeft(x, y, z, block);
        AddTop(x, y, z, block);
        AddBottom(x, y, z, block);
    }
}