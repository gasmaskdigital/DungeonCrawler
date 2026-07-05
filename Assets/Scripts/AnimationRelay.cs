using UnityEngine;

public class AnimationRelay : MonoBehaviour
{
    //Refence
    private PlayerHandler playerHandler;
    private AttackHandler attackHandler;

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
}
