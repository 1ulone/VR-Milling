using UnityEngine;

public class SpindleController : MonoBehaviour
{
	[SerializeField] private LayerMask cuttableLayer;
	[SerializeField] private Transform highlight;
	[SerializeField] private ParticleSystem vfx;
	private GameObject objectOnTable;

	private void Start()
		=> vfx.gameObject.SetActive(false);

	private void OnTriggerStay(Collider other)
	{
		if ((cuttableLayer & (1 << other.gameObject.layer)) > 0)
		{
			if (other.TryGetComponent<Chunk>(out Chunk chunk))
			{
				chunk.RemoveCube(transform.position);
				vfx.Play();
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		vfx.Stop();
	}
}
