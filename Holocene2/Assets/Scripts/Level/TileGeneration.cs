using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGeneration : MonoBehaviour {

	[SerializeField] private NoiseGeneration noiseGeneration;
	[SerializeField] private TextureBuilding textureBuilding;
	[SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshCollider meshCollider;
	[SerializeField] private MeshRenderer tileRenderer;
	[SerializeField] private float levelScale;
	[SerializeField] private TerrainType[] heightTerrainTypes;
    [SerializeField] private float heightMultiplier;
    [SerializeField] private AnimationCurve heightCurve;
    [SerializeField] private Wave[] heightWaves;

	private void Start () {
		//GenerateTile ();	
	}
	
	public void GenerateTile() {
		// calculate tile depth and width based on the mesh vertices
		Vector3[] meshVertices = this.meshFilter.mesh.vertices;
		int tileDepth = (int)Mathf.Sqrt (meshVertices.Length);
		int tileWidth = tileDepth;

        float offsetX = -this.gameObject.transform.position.x;
        float offsetZ = -this.gameObject.transform.position.z;

        // generate the height noise map
        float[,] heightMap = noiseGeneration.GeneratePerlinNoiseMap (tileWidth, tileDepth, levelScale,offsetX, offsetZ, heightWaves);

		// generate a texture using the heightMap
		Texture2D heightTexture = textureBuilding.BuildTexture (heightMap, heightTerrainTypes);

		// assign the texture to the renderer
		this.tileRenderer.material.mainTexture = heightTexture;

        UpdateMeshVertices(heightMap);
	}

    private void UpdateMeshVertices(float[,] heightMap)
    {
        int tileWidth = heightMap.GetLength(0);
        int tileDepth = heightMap.GetLength(1);

        Vector3[] meshVertices = this.meshFilter.mesh.vertices;

        for(int xIndex = 0; xIndex<tileWidth;xIndex++)
        {
            for (int zIndex = 0; zIndex < tileDepth; zIndex++)
            {
                int vertexIndex = zIndex * tileWidth + xIndex;

                float height = heightMap[xIndex, zIndex];

                Vector3 vertex = meshVertices[vertexIndex];
                meshVertices[vertexIndex] = new Vector3(vertex.x, this.heightCurve.Evaluate(height) * heightMultiplier, vertex.z);
            }
        }
        this.meshFilter.mesh.vertices = meshVertices;
        this.meshFilter.mesh.RecalculateBounds();
        this.meshFilter.mesh.RecalculateNormals();
        this.meshCollider.sharedMesh = this.meshFilter.mesh;

    }
}
