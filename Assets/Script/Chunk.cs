using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
	public int cubeSize = 12;
	public bool update;

	private List<Vector3> newVertices = new List<Vector3>();
	private List<Vector2> newUV = new List<Vector2>();
	private List<int> newTriangles = new List<int>();
	private Vector2 defaultMat = new Vector2(0, 0);

	private GameObject cubeObject;
	private Mesh mesh;
	private MeshCollider col;
	public MainCube cube { get; set; }

	private int faceCount;

	private void Start()
	{
		cube = MainCube.instances;
		cubeObject = cube.gameObject;
		mesh = GetComponent<MeshFilter>().mesh;
		col = GetComponent<MeshCollider>();

		GenerateMesh();
	}

	private void LateUpdate()
	{
		if (update)
		{
			GenerateMesh();
			update = false;
		}
	}

	private void CubeTop(int x, int y, int z, byte block)
	{
		newVertices.Add(new Vector3( x , y ,z+1) * 0.01f);
		newVertices.Add(new Vector3(x+1, y ,z+1) * 0.01f);
		newVertices.Add(new Vector3(x+1, y , z ) * 0.01f);
		newVertices.Add(new Vector3( x , y , z ) * 0.01f);

		Cube();
	}

	private void CubeNorth(int x, int y, int z, byte block)
	{
		newVertices.Add(new Vector3 (x + 1, y-1, z + 1) * 0.01f);
		newVertices.Add(new Vector3 (x + 1, y, z + 1) * 0.01f);
		newVertices.Add(new Vector3 (x, y, z + 1) * 0.01f);
		newVertices.Add(new Vector3 (x, y-1, z + 1) * 0.01f);
		Cube();
	}

	private void CubeEast(int x, int y, int z, byte block)
	{
		newVertices.Add(new Vector3 (x + 1, y - 1, z) * 0.01f);
		newVertices.Add(new Vector3 (x + 1, y, z) * 0.01f);
		newVertices.Add(new Vector3 (x + 1, y, z + 1) * 0.01f);
		newVertices.Add(new Vector3 (x + 1, y - 1, z + 1) * 0.01f);
		Cube();
	}

	private void CubeSouth(int x, int y, int z, byte block)
	{
		newVertices.Add(new Vector3 (x, y - 1, z) * 0.01f);
		newVertices.Add(new Vector3 (x, y, z) * 0.01f);
		newVertices.Add(new Vector3 (x + 1, y, z) * 0.01f);
		newVertices.Add(new Vector3 (x + 1, y - 1, z) * 0.01f);
		Cube();
	}

	private void CubeWest(int x, int y, int z, byte block)
	{
		newVertices.Add(new Vector3 (x, y- 1, z + 1) * 0.01f);
		newVertices.Add(new Vector3 (x, y, z + 1) * 0.01f);
		newVertices.Add(new Vector3 (x, y, z	) * 0.01f);
		newVertices.Add(new Vector3 (x, y - 1, z) * 0.01f);
		Cube();
	}
	
	private void CubeBot(int x, int y, int z, byte block)
	{
		newVertices.Add(new Vector3 (x,  y-1,  z ) * 0.01f);
		newVertices.Add(new Vector3 (x + 1, y-1,  z ) * 0.01f);
		newVertices.Add(new Vector3 (x + 1, y-1,  z + 1) * 0.01f);
		newVertices.Add(new Vector3 (x,  y-1,  z + 1) * 0.01f);
		Cube();
	}

	private void Cube()
	{
		newTriangles.Add(faceCount*4);
		newTriangles.Add(faceCount*4 + 1);
		newTriangles.Add(faceCount*4 + 2);
		newTriangles.Add(faceCount*4);
		newTriangles.Add(faceCount*4 + 2);
		newTriangles.Add(faceCount*4 + 3);

		newUV.Add(defaultMat);
		newUV.Add(defaultMat);
		newUV.Add(defaultMat);
		newUV.Add(defaultMat);

		faceCount++;
	}

	public void GenerateMesh()
	{
		for (int x = 0; x < cubeSize; x++)
		{
			for (int y = 0; y < cubeSize; y++)
			{
				for (int z = 0; z < cubeSize; z++)
				{
					if (cube.Block(x,y,z) != 0)
					{
						if (cube.Block(x,y+1,z) == 0)
							CubeTop(x,y,z,cube.Block(x,y,z));

						if (cube.Block(x,y-1,z) == 0)
							CubeBot(x,y,z,cube.Block(x,y,z));

						if (cube.Block(x+1,y,z) == 0)
							CubeEast(x,y,z,cube.Block(x,y,z));

						if (cube.Block(x-1,y,z) == 0)
							CubeWest(x,y,z,cube.Block(x,y,z));

						if (cube.Block(x,y,z+1) == 0)
							CubeNorth(x,y,z,cube.Block(x,y,z));

						if (cube.Block(x,y,z-1) == 0)
							CubeSouth(x,y,z,cube.Block(x,y,z));
					}
				}
			}
		}

		UpdateMesh();
	}

	private void UpdateMesh()
	{
		mesh.Clear();
		mesh.vertices = newVertices.ToArray();
		mesh.uv = newUV.ToArray();
		mesh.triangles = newTriangles.ToArray();
		mesh.Optimize();	
		mesh.RecalculateNormals();

		col.sharedMesh = null;
		col.sharedMesh = mesh;

		newVertices.Clear();
		newUV.Clear();
		newTriangles.Clear();

		faceCount = 0;
	}

	public void RemoveCube(Vector3 pos)
	{
		int xx = Mathf.FloorToInt((pos.x - transform.position.x) / 0.01f);
		int yy = Mathf.RoundToInt((Mathf.Abs(transform.position.y - 0.01f) - Mathf.Abs(pos.y)) / 0.01f);
		int zz = Mathf.FloorToInt((pos.z - transform.position.z) / 0.01f);

		xx = Mathf.Clamp(xx, 0, 11);
		yy = Mathf.Clamp(yy, 0, 11);
		zz = Mathf.Clamp(zz, 0, 11);

//		Debug.Log(new Vector3(xx, yy, zz));

		for (int i = yy; i < 12; i++)
		{
			cube.data[xx, i, zz] = (byte)0;
			update = true;
		}
	}
}

