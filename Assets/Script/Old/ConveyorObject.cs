using UnityEngine;
using System.Collections.Generic;

public class ConveyorObject : MonoBehaviour
{
	[SerializeField] private Transform[] target;

	private Vector3 lastDir;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void FixedUpdate()
	{
		/*
		if (index >= target.Length)
		{
			objectToMove.AddForce(lastDir* 10f * speed * Time.deltaTime, ForceMode.Impulse);
			lastDir = Vector3.zero;
			objectToMove.constraints = RigidbodyConstraints.None;
			return;
		}
		*/
	}

	private	void OnCollisionEnter(Collision other)
	{
		Debug.Log("aaa");
		if (other.gameObject.TryGetComponent<ObjectMove>(out ObjectMove o))
			o.SetTarget(target);
	}
}
