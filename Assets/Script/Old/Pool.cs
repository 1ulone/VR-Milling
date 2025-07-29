using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
	[SerializeField] private List<PoolObject> poolLists;
	[SerializeField] private Transform parent;
	private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

	private void Awake()
	{
		foreach(PoolObject p in poolLists)
		{
			Queue<GameObject> q = new Queue<GameObject>();
			string ntag = p.prefab.name.ToLower();
			for (int i = 0; i < p.count; i++)
			{
				GameObject g = Instantiate(p.prefab);
				g.name = ntag;
				g.transform.SetParent(parent);
				g.SetActive(false);
				q.Enqueue(g);
			}
			poolDictionary.Add(ntag, q);
		}
	}

	public GameObject Create(string tag, Vector3 pos, Quaternion rot)
	{
		if (!poolDictionary.ContainsKey(tag.ToLower()))
			return null;

		GameObject g = poolDictionary[tag.ToLower()].Dequeue();
		g.transform.position = pos;
		g.transform.rotation = rot;

		return g;
	}

	public void Destroy(GameObject prefab)
	{
		if (!poolDictionary.ContainsKey(prefab.name))
			return;

		prefab.SetActive(false);
		poolDictionary[prefab.name].Enqueue(prefab);
	}
}

[System.Serializable]
public class PoolObject
{
	public GameObject prefab;
	public int count;
}
