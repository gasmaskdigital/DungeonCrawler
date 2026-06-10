using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;


public class PlayerController : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] InputActionReference moveAction;
    [SerializeField] InputActionReference lightAttackAction;
    [SerializeField] InputActionReference heavyAttackAction;
    [SerializeField] InputActionReference lookAction;

    private float speed = 10f;
    private float currentSpeed;
    private float mouseX;
    private float cameraSensitivity = 100f;

    [Header("Scripts / Components")]
    private CharacterController controller;
    public Animator animator;
    public Camera playerCamera;
    [SerializeField] Transform cameraPivotPoint;
    [SerializeField] Transform playerVisual;
    


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        lightAttackAction.action.Enable();
        heavyAttackAction.action.Enable();
        lookAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        lightAttackAction.action.Disable();
        heavyAttackAction.action.Disable();
        lookAction.action.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        //Movement logic
        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();

        Vector3 cameraForward = cameraPivotPoint.forward;
        Vector3 cameraRight = cameraPivotPoint.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        // Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y);
        Vector3 movement = cameraRight * moveInput.x + cameraForward * moveInput.y;

        controller.Move(movement * speed * Time.deltaTime);

        currentSpeed = movement.magnitude;
        animator.SetFloat("Speed", currentSpeed, 0.1f, Time.deltaTime);

        

        // light attack
        if (lightAttackAction.action.triggered)
        {
            animator.SetTrigger("Light Attack");
        }

        // heavy attack
        if (heavyAttackAction.action.triggered)
        {
            animator.SetTrigger("Heavy Attack");
        }

        // Camera Logic
        Vector2 mouseInput = lookAction.action.ReadValue<Vector2>();
        mouseX = mouseInput.x * cameraSensitivity * Time.deltaTime;


        cameraPivotPoint.Rotate(Vector3.up * mouseX);

        if (movement.magnitude > 0.1f)
        {
            playerVisual.forward = cameraForward;
        }
        
        
    }

    private void FixedUpdate()
    {
        
    }

}
