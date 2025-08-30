using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 10f;

    private Rigidbody2D physics;
    private Vector2 movement;
    private Vector2 currentVelocity;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        physics = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        HandleSpriteFlip();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 targetVelocity = movement * maxSpeed;

        float lerpRate = (movement.magnitude > 0) ? acceleration : deceleration;

        currentVelocity = Vector2.Lerp(currentVelocity, targetVelocity, lerpRate * Time.fixedDeltaTime);

        physics.MovePosition(physics.position + currentVelocity * Time.fixedDeltaTime);
    }

    private void HandleSpriteFlip()
    {
        if (movement.x > 0)
            spriteRenderer.flipX = false;
        else if (movement.x < 0)
            spriteRenderer.flipX = true;
    }
}
