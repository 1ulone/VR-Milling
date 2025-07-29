using UnityEngine;

public class MainCube : MonoBehaviour
{
	public static MainCube instances;

	public byte[,,] data;
	public int sizeX = 12;
	public int sizeY = 12;
	public int sizeZ = 12;

	private void Awake()
	{
		instances = this;
		data = new byte[sizeX,sizeY,sizeZ];
		Initialize();
	}

	public void Initialize()
	{
		data = new byte[sizeX,sizeY,sizeZ];
		for (int x=0; x < sizeX; x++)
		{
			for (int y=0; y < sizeX; y++)
			{
				for (int z=0; z < sizeX; z++)
				{
					data[x,y,z] = 1;
				}
			}
		}
	}

	public void InitializeWith(Vector3Int[] i)
	{
		for (int x=0; x < sizeX; x++)
		{
			for (int y=0; y < sizeX; y++)
			{
				for (int z=0; z < sizeX; z++)
				{
					data[x,y,z] = 1;
				}
			}
		}

		foreach(Vector3Int v in i)
		{
			data[(int)v.x,(int)v.y,(int)v.z] = 0;
		}
	}

	public byte Block(int x, int y, int z)
	{
		if (x >= sizeX || x < 0 || y >= sizeY || y < 0 || z >= sizeZ || z < 0) 
			return (byte)0;

		return data[x,y,z];
	}
}
