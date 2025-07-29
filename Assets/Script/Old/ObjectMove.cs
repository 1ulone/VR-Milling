using UnityEngine;

public class ObjectMove : MonoBehaviour
{
	private Rigidbody rb;
	private Transform[] target;

	private int index;
	private const float speed = 100f;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		if (target == null)
			return;

		if (index >= target.Length)
			return;

		if (Vector3.Distance(rb.transform.position, target[index].position) <= 0.5f)
			index++;
	}

	private void FixedUpdate()
	{
		if (target == null)
			return;

		if (index >= target.Length)
			return;

		Vector3 dir = (target[index].position - rb.transform.position).normalized;
		rb.linearVelocity = new Vector3(dir.x * speed * Time.deltaTime, 0, dir.z * speed * Time.deltaTime);
	}

	public void SetTarget(Transform[] target)
	{
		rb.constraints = RigidbodyConstraints.FreezeRotation;
		this.target = target;
	}
}
