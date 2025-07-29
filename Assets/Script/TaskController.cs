using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class TaskController : MonoBehaviour
{
	public static TaskController instances;
		
	[SerializeField] private AudioClip gagalClip;
	[SerializeField] private AudioClip benarClip;
	[SerializeField] private AudioSource source;
	[SerializeField] private Material[] taskPrompt;
	[SerializeField] private MeshRenderer whiteboard;
	[SerializeField] private GameObject chunkPrefab;
	[SerializeField] private Transform prefabOrigin;
	[SerializeField] private TextMeshPro notification;
	public int points { get; private set; }
	private int taskIndex;
	private bool first;
	private bool pass; 

	private List<Task> taskList = new List<Task>()
	{
		new Task(new List<DesiredArea>()
			{
				//			   | minx | maxx | minz | maxz | depth
				new DesiredArea(  0   ,  11  ,  0   ,  0   ,  11  ),
				new DesiredArea(  0   ,  11  ,  11  ,  11  ,  11  ),
			}, 5),
		new Task(new List<DesiredArea>()
			{
				//			   | minx | maxx | minz | maxz | depth
				new DesiredArea(  0   ,  11  ,  0   ,  0   ,  11  ),
				new DesiredArea(  0   ,  11  ,  11  ,  11  ,  11  ),
				new DesiredArea(  0   ,  0   ,  0   ,  11  ,  11  ),
				new DesiredArea(  11  ,  11  ,  0   ,  11  ,  11  ),
			}, 10),
		new Task(new List<DesiredArea>()
			{
				//			   | minx | maxx | minz | maxz | depth
				new DesiredArea(  1   ,  10  ,  1   ,  10  ,  10  ),
			}, 10),
		new Task(new List<DesiredArea>()
			{
				//			   | minx | maxx | minz | maxz | depth
				new DesiredArea(  0   ,  11  ,  0   ,  0   ,  10  ),
				new DesiredArea(  0   ,  11  ,  11  ,  11  ,  10  ),
				new DesiredArea(  0   ,  0   ,  0   ,  11  ,  10  ),
				new DesiredArea(  11  ,  11  ,  0   ,  11  ,  10  ),

				new DesiredArea(  1   ,  10  ,  0   ,  0   ,  11  ),
				new DesiredArea(  1   ,  10  ,  11  ,  11  ,  11  ),
				new DesiredArea(  0   ,  0   ,  1   ,  10  ,  11  ),
				new DesiredArea(  11  ,  11  ,  1   ,  10  ,  11  ),
			}, 15),
		new Task(new List<DesiredArea>()
			{
				//			   | minx | maxx | minz | maxz | depth
				new DesiredArea(  0   ,  5   ,  0   ,  5   ,  11  ),
				new DesiredArea(  0   ,  5   ,  8   ,  11  ,  11  ),
				new DesiredArea(  8   ,  11  ,  0   ,  5   ,  11  ),
				new DesiredArea(  8   ,  11  ,  8   ,  11  ,  11  ),
			}, 10)
	};

	private void Awake()
	{
		instances = this;
		notification.gameObject.SetActive(false);
		first = true;
		taskIndex = 0;
		points = 0;
		whiteboard.material = taskPrompt[taskIndex];

//		MainCube.instances.InitializeWith(taskList[taskIndex].vectors().ToArray());
	}

	public void SocketEntered()
	{
		if (first) { taskIndex = 0; first = false; }

		IXRSelectInteractable obj = GetComponent<XRSocketInteractor>().GetOldestInteractableSelected();
		StartCoroutine(CheckTask(MainCube.instances.data, taskIndex));

		if (pass)
		{
			source.PlayOneShot(benarClip, 1);
			notification.gameObject.SetActive(true);
			notification.text = "Anda Berhasil! ";
			StartCoroutine(textAnimation(true));
		}
		else 
		{
			source.PlayOneShot(gagalClip, 1);
			notification.gameObject.SetActive(true);
			notification.text = "Anda Gagal! ";
			StartCoroutine(textAnimation(false));
		}

		MainCube.instances.Initialize();
		Instantiate(chunkPrefab, prefabOrigin.position, Quaternion.identity);
		whiteboard.material = taskPrompt[taskIndex];

		Destroy(obj.transform.gameObject);
		taskIndex = Random.Range(0, taskList.Count);
	}

	private IEnumerator textAnimation(bool isFailed)
	{
		int i = 5;
		while(i > 0)
		{
			notification.color = isFailed ? Color.green : Color.red;
			yield return new WaitForSeconds(0.1f);
			notification.color = Color.white;
			yield return new WaitForSeconds(0.1f);
			i--;
		}
		notification.color = Color.white;
		notification.gameObject.SetActive(false);
	}

	public IEnumerator CheckTask(byte[,,] d, int taskIndex)
	{
		int newPoints = 0;
		Queue<Vector3Int> qvec = new Queue<Vector3Int>();

		foreach(Vector3Int v in taskList[taskIndex].vectors())
		{
//			Debug.Log(v.x + "," + v.y + "," + v.z + " = " + d[v.x, v.y, v.z]);
			
			if (d[v.x, v.y, v.z] == 0)
			{
				qvec.Enqueue(new Vector3Int(v.x, v.y, v.z));
				newPoints++;  
			} else 
			if (d[v.x, v.y, v.z] == 1)
			{
				newPoints--;
			}
			yield return new WaitForSeconds(0.01f);
		}

		int size = 12;
		while (qvec.Count >= 0)
		{
			for (int x=0; x < size; x++)
			{
				for (int y=0; y < size; y++)
				{
					for (int z=0; z < size; z++)
					{
						Vector3Int v = qvec.Dequeue(); 
						if ((x != v.x && y != v.y && z != v.z) && d[x,y,z] == 0) 
						{
							points--;
						}
					}
				}
			}
		}

		int minimumPoints = taskList[taskIndex].minimumPoints;

		points += newPoints < 0 ? 0 : newPoints;
		pass = newPoints > minimumPoints;
	}
}

public class Task 
{
	public int minimumPoints;
	private List<DesiredArea> desiredAreas;

	public Task(List<DesiredArea> coor, int dec)
	{
		this.desiredAreas = coor;
		minimumPoints = dec;
	}

	public List<Vector3Int> vectors()
	{
		List<Vector3Int> vlist = new List<Vector3Int>();
		foreach(DesiredArea v in desiredAreas)
		{
			for (int x=v.min.x; x < v.max.x; x++)
			{
				for (int y=v.min.y; y < v.max.y; y++)
				{
					for (int z=v.min.z; z < v.max.z; z++)
					{
						vlist.Add(new Vector3Int(x,y,z));
					}
				}
			}
		}

		return vlist;
	}
}

public class DesiredArea
{
	public Vector3Int min;
	public Vector3Int max;
	public int totalCount;

	public DesiredArea(int minX, int maxX, int minZ, int maxZ, int Depth)
	{
		this.min = new Vector3Int(minX, Depth, minZ);
		this.max = new Vector3Int(maxX, 11, maxZ) + Vector3Int.one;

		int xlen = (this.max.x - this.min.x);
		int ylen = (this.max.y - this.min.y);
		int zlen = (this.max.z - this.min.z);
		totalCount = Mathf.RoundToInt(
			xlen == 0 ? 1 : xlen *
			ylen == 0 ? 1 : ylen *
			zlen == 0 ? 1 : zlen);
	}
}

/*
		*/

