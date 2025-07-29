using UnityEngine;
using System.Collections.Generic;

public class MoveAfterContact : MonoBehaviour
{
    [Header("Pengaturan Beban (ambil dari Rigidbody.mass)")]
    public float requiredTotalMass = 10f;

    [Header("Waktu Interaksi")]
    public float contactDurationThreshold = 2f;

    [Header("Pengaturan Gerakan")]
    public float moveDuration = 3f;
    public Vector3 moveDirection = Vector3.forward;
    public float moveSpeed = 2f;

    private float contactTimer = 0f;
    private float moveTimer = 0f;
    private bool isMoving = false;

    private List<Transform> objectsOnTop = new List<Transform>();
    private Dictionary<Transform, float> objectMasses = new Dictionary<Transform, float>();

    void Update()
    {

    if (isMoving)
    {
        if (moveTimer < moveDuration)
        {
            Vector3 movement = moveDirection.normalized * moveSpeed * Time.deltaTime;
            transform.Translate(movement, Space.World);

            foreach (Transform obj in objectsOnTop)
            {
                if (obj != null)
                    obj.Translate(movement, Space.World);
            }

            moveTimer += Time.deltaTime;
        }
        else
        {
            isMoving = false;
            objectsOnTop.Clear();
            objectMasses.Clear();
        }
    }

    }
    

    void OnTriggerStay(Collider other)
    {
        if (other.transform.position.y > transform.position.y + 0.1f)
        {
            if (!objectsOnTop.Contains(other.transform))
            {
                objectsOnTop.Add(other.transform);
                other.transform.SetParent(transform);

                Rigidbody rb = other.GetComponent<Rigidbody>();
                float mass = rb != null ? rb.mass : 0f;
                objectMasses[other.transform] = mass;
            }

            contactTimer += Time.deltaTime;

            if (!isMoving && contactTimer >= contactDurationThreshold && GetTotalMass() >= requiredTotalMass)
            {
                isMoving = true;
                moveTimer = 0f;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        contactTimer = 0f;

        if (objectsOnTop.Contains(other.transform))
        {
            other.transform.SetParent(null);
            objectsOnTop.Remove(other.transform);
            objectMasses.Remove(other.transform);
        }
    }

    float GetTotalMass()
    {
        float total = 0f;
        foreach (float mass in objectMasses.Values)
            total += mass;
        return total;
    }
}
