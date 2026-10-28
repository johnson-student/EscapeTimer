using UnityEngine;

public class EscapeTimerStarterExit : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        var manager = FindObjectOfType<EscapeTimerStarterManager>();
        if (manager != null) manager.TryEscape();
    }
}
