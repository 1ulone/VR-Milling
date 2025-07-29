using UnityEngine;

public class OrbitMovement : MonoBehaviour
{
    public Transform target; // Objek yang akan dikelilingi (misalnya rumah)
    public float radius = 5f; // Jarak dari target (radius)
    public float speed = 30f; // Derajat per detik (rotasi)
    public bool clockwise = true; // Arah rotasi
    public float numberOfRounds = 1f; // Berapa kali mengelilingi

    private float angle = 0f;
    private Vector3 center;
    private float totalAngleToTravel;
    private float currentAngleTravelled = 0f;

    void Start()
    {
        if (target != null)
        {
            center = target.position;
            totalAngleToTravel = numberOfRounds * 360f;
            angle = 0f;
        }
    }

    void Update()
    {
        if (target == null || currentAngleTravelled >= totalAngleToTravel)
            return;

        float deltaAngle = speed * Time.deltaTime;
        if (!clockwise)
            deltaAngle *= -1;

        angle += deltaAngle;
        currentAngleTravelled += Mathf.Abs(deltaAngle);

        float rad = angle * Mathf.Deg2Rad;
        float x = Mathf.Cos(rad) * radius;
        float z = Mathf.Sin(rad) * radius;

        transform.position = new Vector3(center.x + x, transform.position.y, center.z + z);
        transform.LookAt(center); // Opsional: mobil selalu menghadap ke pusat (rumah)
    }
}
