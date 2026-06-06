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

    private float speed = 10f;
    private float currentSpeed;

    [Header("Scripts / Components")]
    private CharacterController controller;
    public Animator animator;
    


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        OnEnable();
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
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        lightAttackAction.action.Disable();
        heavyAttackAction.action.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        //Movement logic
        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();

        float vInput = moveInput.y;
        float hInput = moveInput.x;

        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y);

        controller.Move(movement * speed * Time.deltaTime);

        currentSpeed = movement.magnitude;
        animator.SetFloat("Speed", currentSpeed, 0.1f, Time.deltaTime);

        if (movement.magnitude != 0f)
        {
            transform.forward = movement;
        }

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
    }


    private void FixedUpdate()
    {
        
    }

}
