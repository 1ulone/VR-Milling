using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class PushButton : XRBaseInteractable
{
    [Header("Button Settings")]
    public float pressDepth = 0.02f;
    public float pressSpeed = 10f;

    [Header("Target Conveyor")]
    public ConveyorBelt conveyorBelt;

    [Header("Events")]
    public UnityEvent onPressed;

    private Vector3 originalPosition;
    private bool isPressed = false;
    private bool hasInvoked = false;

    protected override void Awake()
    {
        base.Awake();
        originalPosition = transform.localPosition;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        isPressed = true;

        if (!hasInvoked)
        {
            hasInvoked = true;

            // Panggil fungsi di ConveyorBelt jika diset
            if (conveyorBelt != null)
            {
                conveyorBelt.ToggleConveyor();
            }

            // Jalankan UnityEvent tambahan jika ada
            onPressed?.Invoke();
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        isPressed = false;
        hasInvoked = false;
    }

    void Update()
    {
        Vector3 targetPosition = isPressed
            ? originalPosition - new Vector3(0, pressDepth, 0)
            : originalPosition;

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * pressSpeed);
    }
}
