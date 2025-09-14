using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_Picker : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 360.0f;
    public float jumpForce = 7f;
    private Rigidbody rb;

    public float turnSmoothTime = 0.1f;  // Adjustable smoothing time for rotation
    private float turnSmoothVelocity;
    
    
    public LayerMask groundLayer;
    public Transform groundCheck;
    private bool isGrounded;
    private float groundCheckRadius = 0.2f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()  
    {
        rb.isKinematic = false;  // Ensure Rigidbody is not kinematic if controlling velocity
    }

    void Update()
    {
        // Additional rotation using Q and E keys
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        } 
        
        CheckGround();

    }

    void FixedUpdate()
    {
        // Apply movement in FixedUpdate for consistent physics updates
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(x, 0, z).normalized;
        Vector3 velocity = direction * moveSpeed;

        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
        
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }
    private void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
    }
}