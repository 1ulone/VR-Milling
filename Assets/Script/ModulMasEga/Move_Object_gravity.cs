using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MoveAboveTargetOnly : MonoBehaviour
{
    public float speed = 5f;
    public float checkDistance = 1f;
    public GameObject allowedGround; // GameObject tertentu sebagai "tanah"
    
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        RaycastHit hit;

        // Raycast ke bawah
        bool hitSomething = Physics.Raycast(transform.position, Vector3.down, out hit, checkDistance);

        if (hitSomething && hit.collider.gameObject == allowedGround)
        {
            // Di atas GameObject yang diizinkan → bergerak
            rb.useGravity = false;
            rb.linearVelocity = new Vector3(speed, rb.linearVelocity.y, 0); // Gerak di sumbu X
        }
        else
        {
            // Bukan di atas GameObject yang diizinkan → jatuh
            rb.useGravity = true;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * checkDistance);
    }
}
