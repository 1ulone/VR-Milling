using UnityEngine;

public class MoveAlongX : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f; // Speed control (can be changed in Inspector)

    void Update()
    {
        // Move the object along the X axis
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
}
