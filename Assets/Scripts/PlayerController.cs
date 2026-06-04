using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;


public class PlayerController : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] InputActionReference moveAction;
    [SerializeField] InputActionReference lightAttackAction;
    [SerializeField] InputActionReference heavyAttackAction;

    private float speed = 10f;
    private CharacterController controller;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();

        float vInput = moveInput.y;
        float hInput = moveInput.x;

        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y);

        controller.Move(movement * speed * Time.deltaTime);

       // Debug.Log("vInput: " + vInput);
       // Debug.Log("HInput: " + hInput);
    }


    private void FixedUpdate()
    {
        
    }

}
