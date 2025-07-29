using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    [Header("Conveyor Settings")]
    public Vector3 moveDirection = Vector3.forward;
    public float moveSpeed = 2f;

    private bool isRunning = true;

    public void ToggleConveyor()
    {
        isRunning = !isRunning;
        Debug.Log("Conveyor is now " + (isRunning ? "Running" : "Stopped"));
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!isRunning) return;

        Rigidbody rb = collision.rigidbody;

        if (rb != null && !rb.isKinematic)
        {
            rb.linearVelocity = moveDirection.normalized * moveSpeed;
        }
    }
}
