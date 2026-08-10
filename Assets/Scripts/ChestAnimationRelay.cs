using UnityEngine;

public class ChestAnimationRelay : MonoBehaviour
{
    private chestScript chestScript;
    [SerializeField] GameObject glowVFX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        chestScript = GetComponentInParent<chestScript>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    
    public void TurnOnGlow()
    {
        glowVFX.gameObject.SetActive(true);
    }

    public void TurnOffGlow()
    {
        glowVFX.gameObject.SetActive(false);
    }

}
