using UnityEngine;


public class AnimationRelay : MonoBehaviour
{
    //Refence
    private PlayerHandler playerHandler;
    private AttackHandler attackHandler;

    [Header("Sounds")]
    [SerializeField] AudioClip footstep;

    private void Awake()
    {
        playerHandler = GetComponentInParent<PlayerHandler>();
        attackHandler = GetComponentInParent<AttackHandler>();
    }

   public void THSHeavyAttack()
    {
        attackHandler.CheckPlayerWeaponType();
        
    }

    public void THSLightAttack()
    {
        attackHandler.CheckPlayerWeaponType();
        
    }

    public void BowAttack()
    {
        attackHandler.CheckPlayerWeaponType();
    }

    public void BowAimingToggle()
    {
        playerHandler.BowAimingToggle();
    }

    public void FireLightAttack()
    {
        
        attackHandler.SpawnLightFireball();
    }

    public void FireHeavyAttack()
    {
        
        attackHandler.SpawnHeavyFireWave();
    }


    public void CanMoveToggle()
    {
        playerHandler.CanMoveToggle();
    }

    public void CanBeDamaged()
    {
        playerHandler.ToggleCanBeDamaged();
    }

    public void CanAttackToggle()
    {
        playerHandler.CanAttackToggle();
    }

    public void DamagedReactOff()
    {
        playerHandler.DamagedReactOff();
    }

    public void DamagedReactOn()
    {
        playerHandler.DamagedReactOn();
    }

    public void Death()
    {
        playerHandler.DeathDestoy();
    }

    public void AttackBoolsOff()
    {
        playerHandler.AttackBoolsOff();       
    }

    public void AttackBoolsOn()
    {
        playerHandler.AttackBoolsOn();
    }

    public void DodgeBoolsOff()
    {
        playerHandler.DodgeBoolsOff();
    }

    public void DodgeBoolsOn()
    {
        playerHandler.DodgeBoolOn();        
    }

    public void SwordTrailOn()
    {
        playerHandler.SwordTrailOn();
    }

    public void SwordTrailOff()
    {
        playerHandler.SwordTrailOff();
    }

    public void Footstep()
    {
        SoundManager.Instance.PlaySound(footstep, transform);
    }

    public void AxeLightAttack()
    {
        attackHandler.AxeLightAttack();
    }

    public void AxeHeavyAttack()
    {
        attackHandler.AxeHeavyAttack();
    }

    public void AxeTrailOn()
    {
        playerHandler.AxeTrailOn();
    }

    public void AxeTrailOff()
    {
        playerHandler.AxeTrailOn();
    }

    public void ClawLightAttack()
    {
        attackHandler.ClawLightAttack();
    }

    public void ClawHeavyAttack()
    {
        attackHandler.ClawHeavyAttack();
    }

    public void ClawTrailOn()
    {
        playerHandler.ClawTrailOn();
    }

    public void ClawTrailOff()
    {
        playerHandler.ClawTrailOff();
    }

}
