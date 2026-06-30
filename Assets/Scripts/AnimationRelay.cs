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
        Debug.Log("AnimationRelay");
    }

    public void THSLightAttack()
    {
        attackHandler.CheckPlayerWeaponType();
        Debug.Log("Animation Relay");
    }

    public void BowAttack()
    {
        attackHandler.CheckPlayerWeaponType();
        //Debug.Log("Bow AnimationRelay");
    }

    public void BowAimingToggle()
    {
        playerHandler.BowAimingToggle();
        //Debug.Log("Bow Aiming Toggle");
    }

    public void FireLightAttack()
    {
        Debug.Log("FireLightAttack");
        attackHandler.SpawnLightFireball();
    }

    public void FireHeavyAttack()
    {
        Debug.Log("Fire Heavy Attack");
        attackHandler.SpawnHeavyFireWave();
    }


    public void CanMoveToggle()
    {
        playerHandler.CanMoveToggle();
    }

    public void Invincibilty()
    {
        playerHandler.ToggleCanBeDamaged();
    }

    public void CanAttackToggle()
    {
        playerHandler.CanAttackToggle();
    }
}
