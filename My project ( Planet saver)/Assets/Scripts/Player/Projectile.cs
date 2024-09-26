using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private float direction;
    private bool hit;
    private float lifetime;

    private Animator anim;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        // If hit, stop the movement
        if (hit) return;

        // Move the fireball horizontally based on the direction
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        // Increment lifetime and deactivate fireball if it exceeds the limit
        lifetime += Time.deltaTime;
        if (lifetime > 5) gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ignore further collisions after the fireball hits something
        hit = true;
        boxCollider.enabled = false;

        // Trigger explosion animation
        anim.SetTrigger("explode");

        // Handle specific collision logic here, for example:
        // if (collision.gameObject.CompareTag("Enemy")) { DealDamage(collision); }
    }

    public void SetDirection(float _direction)
    {
        // Reset lifetime and hit status
        lifetime = 0;
        hit = false;
        direction = _direction;

        // Reactivate the fireball and enable collision
        gameObject.SetActive(true);
        boxCollider.enabled = true;

        // Make sure the fireball faces the right direction
        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
            localScaleX = -localScaleX;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);

        // OPTIONAL: Ignore collision with the player to prevent self-explosion
        Collider2D playerCollider = FindObjectOfType<PlayerMovement>().GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(boxCollider, playerCollider);
    }

    // This method should be called via an Animation Event at the end of the explosion animation
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}