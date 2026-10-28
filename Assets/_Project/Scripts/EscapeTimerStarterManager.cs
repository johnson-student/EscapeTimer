using UnityEngine;

public class EscapeTimerStarterManager : MonoBehaviour
{
    public Transform player;
    public GameObject pressurePlate;
    public GameObject exitDoor;
    public GameObject exitZone;

    public float timeLimit = 60f;
    float timeLeft;
    bool plateActivated;
    bool won;
    bool lost;

    void Start()
    {
        timeLeft = timeLimit;
        if (player == null) player = GameObject.FindWithTag("Player")?.transform;
        if (exitDoor == null) exitDoor = GameObject.FindWithTag("Exit");
    }

    void Update()
    {
        if (won || lost) return;
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            lost = true;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            var p = GameObject.FindWithTag("Player");
            if (p != null) p.transform.position = new Vector3(-4.5f, 1f, -4.5f);
            timeLeft = timeLimit;
            plateActivated = false;
            won = false;
            lost = false;
            if (exitDoor != null) exitDoor.SetActive(true);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // This manager's trigger is not used; pressure plate and exit-zone events are handled by helper components below.
    }

    public void ActivatePlate()
    {
        plateActivated = true;
        if (exitDoor != null) exitDoor.SetActive(false);
    }

    public void TryEscape()
    {
        if (plateActivated) won = true;
        else Debug.Log("The exit is locked! Find the pressure plate first.");
    }

    void OnGUI()
    {
        GUI.color = Color.white;
        GUI.Box(new Rect(15, 15, 260, 75), "");
        GUI.Label(new Rect(30, 25, 230, 25), "TIME LEFT: " + Mathf.CeilToInt(timeLeft));
        GUI.Label(new Rect(30, 52, 230, 25), plateActivated ? "Pressure plate: ACTIVE" : "Pressure plate: INACTIVE");

        if (won) GUI.Box(new Rect(Screen.width / 2 - 110, Screen.height / 2 - 35, 220, 70), "ESCAPED! YOU WIN!");
        if (lost) GUI.Box(new Rect(Screen.width / 2 - 110, Screen.height / 2 - 35, 220, 70), "TIME UP! YOU LOSE!");
        GUI.Label(new Rect(15, Screen.height - 35, 300, 25), "WASD: Move    R: Restart");
    }

    void OnDisable()
    {
        // Teardown hook for the lab; no event subscriptions are currently used.
    }
}
