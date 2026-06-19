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

    public void HeavyAttack()
    {
        playerHandler.HeavyAttack();
    }

    public void LightAttack()
    {
        playerHandler.LightAttack();
    } 

    public void CanMoveToggle()
    {
        playerHandler.CanMoveToggle();
    }

    public void Invincibilty()
    {
        playerHandler.ToggleCanBeDamaged();
    }
}
