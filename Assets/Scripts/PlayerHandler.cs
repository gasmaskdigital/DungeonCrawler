using Unity.Mathematics;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    [Header("Input")]
    private float moveInput;
    private float turnInput;

    [Header("References")]
    private CharacterController controller;
    [SerializeField] Transform cameraTransform;
    public Animator playerAnimator;

    [Header("Movement Settings")]
    private float walkSpeed = 6f;
    private float currentSpeed;
    private float turningSpeed = 100f;
    private float gravityForce = 9.8f;

    private float verticalVelocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        InputMagangement();
        Movement();

        playerAnimator.SetFloat("Speed", currentSpeed, 0, Time.deltaTime);

        // Light Attack
        if (Input.GetMouseButtonDown(0))
        {
            playerAnimator.SetTrigger("Light Attack");
        }

        // Heavy Attack
        if (Input.GetMouseButtonDown(1))
        {
            playerAnimator.SetTrigger("Heavy Attack");
        }
    }

    private void InputMagangement()
    {
        moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
    }

    private void GroundMovement()
    {
        Vector3 move = new Vector3(turnInput, 0, moveInput);
        move = cameraTransform.transform.TransformDirection(move);

        move.y = VerticalForceCalculation();

        move *= walkSpeed;

        controller.Move(move * Time.deltaTime);

        currentSpeed = controller.velocity.magnitude;
    }

    private void Movement()
    {
        GroundMovement();
        Turn();
    }

    private void Turn()
    {
        if(Mathf.Abs(turnInput) > 0 || Mathf.Abs(moveInput) > 0)
        {
            Vector3 currentLookDirection = controller.velocity.normalized; ;
            currentLookDirection.y = 0;

            currentLookDirection.Normalize();

            Quaternion targetRotation = Quaternion.LookRotation(currentLookDirection);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turningSpeed);
        }        
    }

    private float VerticalForceCalculation()
    {
        if (controller.isGrounded)
        {
            verticalVelocity = -1f;
        }
        else
        {
            verticalVelocity -= gravityForce * Time.deltaTime;
        }
        return verticalVelocity;
    }

}
