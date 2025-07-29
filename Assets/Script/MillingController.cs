using UnityEngine;

public class MillingController : MonoBehaviour
{
	[Header("Default")]
	[SerializeField] private float stepMultiplier = 1;


	[Header("Milling Handle")]
	[SerializeField] private GameObject x_axisHandle;
	[SerializeField] private GameObject y_axisHandle;
	[SerializeField] private GameObject z_axisHandle;


	[Header("Output of Handle")]
	[SerializeField] private GameObject drill;
	[SerializeField] private GameObject board;

	private const float maximumValue = 170.0f;
	private Vector3 drillOriginalPosition;
	private Vector3 boardOriginalPosition;

	private float multiplierFormula(GameObject handle)
	{ return (float)(Mathf.CeilToInt((Mathf.Atan2(handle.transform.rotation.y, handle.transform.rotation.x) * Mathf.Rad2Deg)) - 5); }

	private float xMul { get { return (multiplierFormula(x_axisHandle)/maximumValue ) * stepMultiplier; } }
	private float yMul { get { return (multiplierFormula(y_axisHandle)/maximumValue ) * stepMultiplier; } }
	private float zMul { get { return (multiplierFormula(z_axisHandle)/maximumValue ) * stepMultiplier; } }

	private void Start()
	{
		drillOriginalPosition = drill.transform.position;
		boardOriginalPosition = board.transform.position;
	}

	private void Update()
	{
		drill.transform.position = drillOriginalPosition + (Vector3.up * Mathf.Clamp(yMul, -0.37f, 0.47f));
		board.transform.position = boardOriginalPosition + new Vector3(Mathf.Clamp(xMul, -0.45f, 0.3f), 0, Mathf.Clamp(zMul, -0.35f, 0.27f));

//		Debug.Log(xMul + " , " + zMul);
	}
}
