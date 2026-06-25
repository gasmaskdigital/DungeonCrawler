using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
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
    private AttackHandler attackHandler;
    private SphereCollider detectionSphere;
    private PlayerStats playerStats;
    public UnityEngine.Camera mainCamera;
    public LayerMask terrainLayer;

    [Header("Movement Settings")]
    private float walkSpeed = 6f;
    private float currentSpeed;
    private float turningSpeed = 100f;
    private float gravityForce = 9.8f;
    private float verticalVelocity;
    private bool canMove = true;
    private float dodgeSpeed = 10f;
    private float dashTime = 0.8f;
    public bool bowAiming = false;

    [Header("Attack Parameters")]    
    private float lightAttackRadius = 1.5f;
    private float heavyAttackRadius = 2.5f;
    public LayerMask enemyMask;
    public bool canBeDamaged = true;
    public bool canAttack = true;
    public float knockbackForce = 10000f;
    private float knockbackDelay = 0.3f;

    [Header("Current Equipment")]
    [SerializeField] string curWeapon;
    [SerializeField] string curHelm;
    [SerializeField] string curUB;
    [SerializeField] string curLB;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        detectionSphere = GetComponent<SphereCollider>();
        attackHandler = GetComponent<AttackHandler>();
        playerStats = GetComponent<PlayerStats>();

        Cursor.lockState = CursorLockMode.Confined;

        
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            curWeapon = playerStats.currentWeapon.weaponName;
            curLB = playerStats.currentLowerBody.armourName;
            curUB = playerStats.currentUpperBody.armourName;
            curHelm = playerStats.currentHelmet.armourName;
        }
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
            if (canAttack)
            {
                attackHandler.attackType = AttackType.LightAttack;
                CheckWeaponForAnimTrigger();
            }
        }

        // Heavy Attack
        if (Input.GetMouseButtonDown(1))
        {
            if (canAttack)
            {
                attackHandler.attackType = AttackType.HeavyAttack;
                CheckWeaponForAnimTrigger();
            }
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
            if (canMove)
            {
                playerAnimator.SetTrigger("Dodge");
                Dodge();
            }
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

    private void LateUpdate()
    {
        if (bowAiming)
        {
            BowAiming();
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
        if (canAttack)
        {
            Debug.Log("heavy attack");

            Collider[] colliders = Physics.OverlapSphere(transform.position, heavyAttackRadius, enemyMask);
            foreach (Collider c in colliders)
            {
                if (c.gameObject.CompareTag("Enemy"))
                {
                    Destroy(c);
                }
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 origin = transform.position + Vector3.up * 1f + transform.forward * 0.75f;
        Gizmos.DrawWireSphere(origin, lightAttackRadius);
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
            Debug.Log("Dodgeing");

            yield return null;
        }
    }

    public void ToggleCanBeDamaged()
    {
        if (canBeDamaged)
        {
            canBeDamaged = false;
            canAttack = false;
        }
        else
        {
            canBeDamaged = true;
            canAttack = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            AINavigation enemy = other.GetComponent<AINavigation>();

            if(enemy != null)
            {
                enemy.ChasePlayer();
            }
        }
    }

    private void CheckWeaponForAnimTrigger()
    {
        switch(playerStats.currentWeapon.weaponType)
        {
            case WeaponType.TwoHandedSword:
                if(attackHandler.attackType == AttackType.LightAttack)
                {
                    playerAnimator.SetTrigger("THSLightAttack");
                }
                else
                {
                    playerAnimator.SetTrigger("THSHeavyAttack");
                }
                break;
            case WeaponType.Bow:
                if (attackHandler.attackType == AttackType.LightAttack)
                {
                    playerAnimator.SetTrigger("BowLightAttack");
                }
                else
                {
                    playerAnimator.SetTrigger("BowHeavyAttack");
                }
                break;
            case WeaponType.FireSpellBook:
                if(attackHandler.attackType == AttackType.LightAttack)
                {
                    playerAnimator.SetTrigger("FireLightAttack");
                }
                else
                {
                    playerAnimator.SetTrigger("FireHeavyAttack");
                }
                break;
        }        
    }

    public void BowAimingToggle()
    {
        if (bowAiming)
        {
            bowAiming = false;
        }
        else
        {
            bowAiming = true;
        }

        Debug.Log("bowAiming is " + bowAiming);
    }

    private void BowAiming()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out RaycastHit hit, 100f, terrainLayer))
        {
            Vector3 targetPos = hit.point;

            Vector3 direction = targetPos - transform.position;
            direction.y = 0f;

            if(direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y, 0f);
            }
        }
        
    }

    public void CanAttackToggle()
    {
        if (canAttack)
        {
            canAttack = false;
        }
        else
        {
            canAttack = true;
        }
    }
}
