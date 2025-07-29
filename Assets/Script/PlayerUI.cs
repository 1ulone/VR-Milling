using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
	public TextMeshProUGUI points; 

	private void Update()
	{
		points.text = TaskController.instances.points.ToString(); 
	}
}
