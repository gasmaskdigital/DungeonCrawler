using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

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
    private bool canMove = true;
    private float dodgeSpeed = 25f;
    private float dashTime = 1.4f;

    [Header("Attack Parameters")]    
    private float lightAttackRadius = 1f;
    private float heavyAttackRadius = 2.5f;
    private float maxDistance = 1f;
    public LayerMask enemyMask;
    public bool canBeDamaged = true;
    


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

        // pick up item logic
        if (Input.GetKeyDown(KeyCode.E))
        {
            // sphere cast to pick up item
        }
        
        //pause button
        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            // pauses time and brings up ui screen
            // swap to esc when it come time to build
        }

        // open inventory screen
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // opens inventory screen - toggles the screen
        }

        // dodge input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerAnimator.SetTrigger("Dodge");
        }

        // health potion
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {

        }
        
        // speed potion
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {

        }

        // damage potion
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {

        }

        // defence potion
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {

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
        if (canMove)
        {
            GroundMovement();
            Turn();
        }
       
    }

    private void Turn()
    {
        if(Mathf.Abs(turnInput) > 0 || Mathf.Abs(moveInput) > 0)
        {
            Vector3 currentLookDirection = controller.velocity.normalized; 
            currentLookDirection.y = 0;

            if (currentLookDirection.sqrMagnitude < 0.001f) return;

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

    public void HeavyAttack()
    {
        Debug.Log("heavy attack");

        Collider[] colliders = Physics.OverlapSphere(transform.position, heavyAttackRadius, enemyMask);
        foreach(Collider c in colliders)
        {
            if (c.gameObject.CompareTag("Enemy"))
            {
                Destroy(c.gameObject);
            }
        }
    }

    public void LightAttack()
    {
        Debug.Log("Light Attack");

        Vector3 origin = transform.position + Vector3.up * 1.5f + transform.forward * 1.25f; ;

        Collider[] colliders = Physics.OverlapSphere(origin, lightAttackRadius, enemyMask);
        foreach (Collider c in colliders)
        {
            if (c.gameObject.CompareTag("Enemy"))
            {
                Destroy(c.gameObject);
            }
        }

    }

    public void CanMoveToggle()
    {
        if (canMove)
        {
            canMove = false;
        }
        else
        {
            canMove = true;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * 1.5f, heavyAttackRadius);
    }

    public void Dodge()
    {
        StartCoroutine(Dash());
    }

    IEnumerator Dash()
    {
        float startTime = Time.time;

        while (Time.time <startTime + dashTime)
        {
            controller.Move(transform.forward * dodgeSpeed * Time.deltaTime);

            yield return null;
        }
    }

    public void ToggleCanBeDamaged()
    {
        if (canBeDamaged)
        {
            canBeDamaged = false;
        }
        else
        {
            canBeDamaged = true;
        }
    }
}
