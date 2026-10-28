using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EscapeTimerStarterPlayer : MonoBehaviour
{
    public float moveSpeed = 4f;
    Rigidbody rb;
    Vector3 move;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    
    void Update()
    {
        float x = 0f;
        float z = 0f;

        if (Input.GetKey(KeyCode.A)) x -= 1f;
        if (Input.GetKey(KeyCode.D)) x += 1f;
        if (Input.GetKey(KeyCode.W)) z += 1f;
        if (Input.GetKey(KeyCode.S)) z -= 1f;

        // Move relative to the direction the player faces
        move = (transform.right * x + transform.forward * z).normalized;
    }


    void FixedUpdate()
    {
        rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Player collided with: " + collision.gameObject.name);
    }
}
