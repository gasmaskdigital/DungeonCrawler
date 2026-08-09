using NUnit.Framework.Internal;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;
using static UnityEngine.UI.Image;

public class PlayerHandler : MonoBehaviour
{
    [Header("Input")]
    private float moveInput;
    private float turnInput;

    [Header("References")]
    private CharacterController controller;
    private gameManager gameManager;
    [SerializeField] Transform cameraTransform;
    public Animator playerAnimator;
    private AttackHandler attackHandler;
    private SphereCollider detectionSphere;
    private PlayerStats playerStats;
    public UnityEngine.Camera mainCamera;
    public LayerMask terrainLayer;
    private VFXManager vfxManager;
    public GameObject[] potionsObjects; // 0-Health 1-Stregth 2-Dex 3-Magic
    public EffectHandler effectHandler;

    [Header("Weapon Trails")]
    [SerializeField] GameObject swordTrail;
    [SerializeField] GameObject axeTrail;
    [SerializeField] GameObject clawTrailL;
    [SerializeField] GameObject clawTrailR;
    [SerializeField] GameObject[] potionVFXs;

    [Header("Movement Settings")]
    private float walkSpeed = 7f;
    private float currentSpeed;
    private float turningSpeed = 5f;
    private float gravityForce = 9.8f;
    private float verticalVelocity;
    private bool canMove = true;
    private float dodgeSpeed = 10f;
    private float dashTime = 0.8f;
    public bool bowAiming = false;
    public bool canDodge = true;

    [Header("Attack Parameters")]    
    private float lightAttackRadius = 1.5f;
    private float heavyAttackRadius = 2.5f;
    public LayerMask enemyMask;
    public bool canBeDamaged = true;
    public bool canAttack = true;
    public float knockbackForce = 10000f;
    //private float knockbackDelay = 0.3f;
    public potionUsed currentPotion;

    [Header("Current Equipment")]
    [SerializeField] string curWeapon;
    [SerializeField] string curHelm;
    [SerializeField] string curUB;
    [SerializeField] string curLB;

    [Header("Audio Clips")]
    [SerializeField] AudioClip swordLightSwoosh;
    [SerializeField] AudioClip dodge;
    [SerializeField] AudioClip drinkPotion;
    [SerializeField] AudioClip outOfPotions;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        detectionSphere = GetComponent<SphereCollider>();
        attackHandler = GetComponent<AttackHandler>();
        playerStats = GetComponent<PlayerStats>();
        effectHandler = GetComponent<EffectHandler>();
        vfxManager = FindAnyObjectByType<VFXManager>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameManager>();

        Cursor.lockState = CursorLockMode.Confined;

        
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            curWeapon = PlayerStats.currentWeapon.weaponName;
            curLB = PlayerStats.currentLowerBody.armourName;
            curUB = PlayerStats.currentUpperBody.armourName;
            curHelm = PlayerStats.currentHelmet.armourName;
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
                AttackBoolsOff();
                }
            }

            // Heavy Attack
            if (Input.GetMouseButtonDown(1))
            {
                if (canAttack)
                {
                    attackHandler.attackType = AttackType.HeavyAttack;
                    CheckWeaponForAnimTrigger();
                AttackBoolsOff();
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
                SoundManager.Instance.PlaySound(dodge, transform, 0.9f);
                
                }
            }

            // Health potion
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
            if (PlayerStats.healthPotionStack > 0)
            {
                if (canMove)
                {
                    currentPotion = potionUsed.Health;
                    playerAnimator.SetTrigger("Drink");
                    SoundManager.Instance.PlaySound(drinkPotion, transform, 0.8f);

                    int newHealth = PlayerStats.currentHealth + Mathf.CeilToInt(levelManager.currentLevel / 4f) * 10;
                    PlayerStats.currentHealth = Mathf.Min(PlayerStats.maxHealth, newHealth);
                    PlayerStats.healthPotionStack--;
                    gameManager.updateEquipment();
                    potionVFXs[0].gameObject.SetActive(true);
                }
            }
            else
            {
                SoundManager.Instance.PlaySound(outOfPotions, transform, 0.75f);
            }
            }

            // Strength potion
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
            if (PlayerStats.strengthPotionStack > 0)
            {
                if (canMove)
                {
                    currentPotion = potionUsed.Strength;
                    playerAnimator.SetTrigger("Drink");
                    SoundManager.Instance.PlaySound(drinkPotion, transform, 0.8f);
                    StatusEffect strength = new("Strength", Mathf.CeilToInt(levelManager.currentLevel / 4f), 30);
                    addEffectToPlayer(strength);
                    PlayerStats.strengthPotionStack--;
                    gameManager.updateEquipment();
                    potionVFXs[1].gameObject.SetActive(true);
                }
            }
            else
            {
                SoundManager.Instance.PlaySound(outOfPotions, transform, 0.75f);
            }
        }

            // Dex potion
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
            if (PlayerStats.dexterityPotionStack > 0)
            {
                if (canMove)
                {
                    currentPotion = potionUsed.Dexterity;
                    playerAnimator.SetTrigger("Drink");
                    SoundManager.Instance.PlaySound(drinkPotion, transform, 0.8f);
                    StatusEffect agility = new("Agility", Mathf.CeilToInt(levelManager.currentLevel / 4f), 30);
                    addEffectToPlayer(agility);
                    PlayerStats.dexterityPotionStack--;
                    gameManager.updateEquipment();
                    potionVFXs[2].gameObject.SetActive(true);
                }
            }
            else
            {
                SoundManager.Instance.PlaySound(outOfPotions, transform, 0.75f);
            }
        }

            // Magic potion
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
            if (PlayerStats.magicPotionStack > 0)
            {
                if (canMove)
                {
                    currentPotion = potionUsed.Magic;
                    playerAnimator.SetTrigger("Drink");
                    SoundManager.Instance.PlaySound(drinkPotion, transform, 0.8f);
                    StatusEffect mana = new("Mana", Mathf.CeilToInt(levelManager.currentLevel / 4f), 30);
                    addEffectToPlayer(mana);
                    PlayerStats.magicPotionStack--;
                    gameManager.updateEquipment();
                    potionVFXs[3].gameObject.SetActive(true);
                }
            }
            else
            {
                SoundManager.Instance.PlaySound(outOfPotions, transform, 0.75f);
            }
        }

        if (bowAiming)
        {
            // Debug.Log("bow aiming " + bowAiming);
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
        else if (other.CompareTag("Terrain"))
        {
            if (other.gameObject.GetComponent<BoxCollider>()) other.gameObject.GetComponent<BoxCollider>().enabled = true;
        }
    }

    private void CheckWeaponForAnimTrigger()
    {
        switch(PlayerStats.currentWeapon.weaponType)
        {
            case WeaponType.TwoHandedSword:
                if(attackHandler.attackType == AttackType.LightAttack)
                {
                    playerAnimator.SetTrigger("THSLightAttack");
                    SoundManager.Instance.PlaySound(swordLightSwoosh, transform, 0.85f);
                }
                else
                {
                    playerAnimator.SetTrigger("THSHeavyAttack");
                    SoundManager.Instance.PlaySound(swordLightSwoosh, transform, 0.95f);
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
            case WeaponType.Axe:
                if (attackHandler.attackType == AttackType.LightAttack)
                {
                    playerAnimator.SetTrigger("AxeLightAttack");
                }
                else
                {
                    playerAnimator.SetTrigger("AxeHeavyAttack");
                }
                break;
            case WeaponType.Claw:
                if(attackHandler.attackType == AttackType.LightAttack)
                {
                    playerAnimator.SetTrigger("ClawLightAttack");
                }
                else
                {
                    playerAnimator.SetTrigger("ClawHeavyAttack");
                }
                break;
            case WeaponType.IceStaff:
                if(attackHandler.attackType == AttackType.LightAttack)
                {
                    playerAnimator.SetTrigger("IceLightAttack");
                    Debug.Log("ice light attack");
                }
                else
                {
                    playerAnimator.SetTrigger("IceHeavyAttack");
                }
                break;

        }        
    }

    public void BowAimingToggle()
    {
        bowAiming = !bowAiming;

    }

    private void BowAiming()
    {

        // Debug.Log("Bow aiming method");

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out RaycastHit hit, 10000f, terrainLayer))
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
        canAttack = !canAttack;
    }

    public void DamagedReactOff()
    {
        canAttack = false;
        canBeDamaged = false;
        canDodge = false;
        canMove = false;
        bowAiming = false;
    }

    public void DamagedReactOn()
    {
        canAttack = true;
        canBeDamaged = true;
        canDodge = true;
        canMove = true;
    }

    public void AttackBoolsOff()
    {
        canAttack = false;
        bowAiming = true;
        canDodge = false;
        canMove = false;
    }

    public void AttackBoolsOn()
    {
        canAttack = true;
        bowAiming = false;
        canDodge = true;
        canMove = true;
    }

    public void DodgeBoolsOff()
    {
        canAttack = false;
        bowAiming = false;
        canDodge = false;
        canMove = false;
        canBeDamaged = false;
    }

    public void DodgeBoolOn()
    {
        canAttack = true;
        canDodge = true;
        canMove = true;
        canBeDamaged = true;
    }

    public void DamagedTrigger()
    {
        playerAnimator.SetTrigger("Damaged");
    }

    public void DeathTrigger()
    {
        playerAnimator.SetTrigger("Death");
    }
    public void DeathDestoy()
    {
        Destroy(gameObject);
    }

    public void SwordTrailOn()
    {
        swordTrail.gameObject.SetActive(true);
    }
    
    public void SwordTrailOff()
    {
        swordTrail.gameObject.SetActive(false);
    }

    public void AxeTrailOn()
    {
        axeTrail.gameObject.SetActive(true);
    }

    public void AxeTrailOff()
    {
        axeTrail.gameObject.SetActive(false);
    }

    public void ClawTrailOn()
    {
        clawTrailL.gameObject.SetActive(true);
        clawTrailR.gameObject.SetActive(true);
    }

    public void ClawTrailOff()
    {
        clawTrailL.gameObject.SetActive(false);
        clawTrailR.gameObject.SetActive(false);
    }

    public void AllTrailsOff()
    {
        clawTrailL.gameObject.SetActive(false);
        clawTrailR.gameObject.SetActive(false);
        axeTrail.gameObject.SetActive(false);
        swordTrail.gameObject.SetActive(false);
    }

    public void PotionsBoolsOff()
    {
        canAttack = false;
        canBeDamaged = false;
        canDodge = false;
        canMove = false;
        bowAiming = false;
        PotionModelOn();
        playerStats.TurnOffWeaponModels();
    }

    public void PotionBoolsOn()
    {
        canAttack = true;
        canBeDamaged = true;
        canDodge = true;
        canMove = true;
        PotionModelOff();
        playerStats.UpdateWeaponSocket();
        TurnOffPotionVFX();
    }

    private void PotionModelOn()
    {
        switch (currentPotion)
        {
            case potionUsed.Health:
                potionsObjects[0].gameObject.SetActive(true);
                break;
            case potionUsed.Strength:
                potionsObjects[1].gameObject.SetActive(true);
                break;
            case potionUsed.Dexterity:
                potionsObjects[2].gameObject.SetActive(true);
                break;
            case potionUsed.Magic:
                potionsObjects[3].gameObject.SetActive(true);
                break;
        }            
    }

    private void PotionModelOff()
    {
        switch (currentPotion)
        {
            case potionUsed.Health:
                potionsObjects[0].gameObject.SetActive(false);
                break;
            case potionUsed.Strength:
                potionsObjects[1].gameObject.SetActive(false);
                break;
            case potionUsed.Dexterity:
                potionsObjects[2].gameObject.SetActive(false);
                break;
            case potionUsed.Magic:
                potionsObjects[3].gameObject.SetActive(false);
                break;
        }
    }

    public void addEffectToPlayer(StatusEffect effect) 
    {
        bool hasEffect = false;
        int index = 0;

        foreach (StatusEffect e in effectHandler.activeEffects)
        {
            if (e.name == effect.name)
            {
                hasEffect = true;
                break;
            }
            else index++;
        }

        if (hasEffect)
        {
            effectHandler.activeEffects[index] = effect;
        }
        else effectHandler.addEffect(effect);
    }

    private void TurnOffPotionVFX()
    {
        foreach(GameObject g in potionVFXs)
        {
            g.gameObject.SetActive(false);
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Trap"))
        {
            Debug.Log(hit.gameObject.name);
            playerStats.TakeDamage(2);
        }
    }
}



public enum potionUsed
{
    Health, Strength, Dexterity, Magic
}
