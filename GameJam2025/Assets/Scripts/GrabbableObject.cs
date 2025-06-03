using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GrabbableObject : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake() => rb = GetComponent<Rigidbody2D>();

    public void PickUp()
    {
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        rb.gravityScale = 0;
    }

    public void Throw(Vector2 direction, float force)
    {
        rb.isKinematic = false;
        rb.gravityScale = 2;
        rb.velocity = Vector2.zero;
        rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5f) 
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

}
