using UnityEngine;

public class EscapeTimerStarterPlate : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        var manager = FindObjectOfType<EscapeTimerStarterManager>();
        if (manager != null) manager.ActivatePlate();
        Debug.Log("Pressure plate activated!");
    }
}
