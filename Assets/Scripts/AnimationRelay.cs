using UnityEngine;

public class AnimationRelay : MonoBehaviour
{
    //Refence
    private PlayerHandler playerHandler;

    private void Awake()
    {
        playerHandler = GetComponentInParent<PlayerHandler>();
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
