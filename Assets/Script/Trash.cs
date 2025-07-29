using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Trash : MonoBehaviour
{
	[SerializeField] private GameObject chunkPrefab;
	[SerializeField] private Transform prefabOrigin;

	public void TrashEntered()
	{
		IXRSelectInteractable obj = GetComponent<XRSocketInteractor>().GetOldestInteractableSelected();
		Destroy(obj.transform.gameObject);

		MainCube.instances.Initialize();
		Instantiate(chunkPrefab, prefabOrigin.position, Quaternion.identity);
	}
}
