using UnityEngine;
using UnityEngine.Events;

public class ButtonVR : MonoBehaviour
{
    public GameObject button;
    public UnityEvent onPress;
    public UnityEvent onRelease;
    GameObject presser;
    AudioSource sound;
    bool isPressed;
    Vector3 initialPosition; // <<< Tambahkan ini

    void Start()
    {
        sound = GetComponent<AudioSource>();
        isPressed = false;
        initialPosition = button.transform.localPosition; // <<< Simpan posisi awal
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed)
        {
            button.transform.localPosition = initialPosition + new Vector3(0, -0.02f, 0); // <<< Geser dari posisi awal, bukan set absolut
            presser = other.gameObject;
            onPress.Invoke();
            if (sound != null) sound.Play();
            isPressed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == presser)
        {
            button.transform.localPosition = initialPosition; // <<< Balik ke posisi awal
            onRelease.Invoke();
            isPressed = false;
        }
    }

    public void SpawnSphere()
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        sphere.transform.position = transform.position + transform.forward * 2.0f + Vector3.up * 1.0f;
        Rigidbody rb = sphere.AddComponent<Rigidbody>();
        rb.AddForce(transform.forward * 5f, ForceMode.Impulse);
    }
}
