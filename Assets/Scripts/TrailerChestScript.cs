using UnityEngine;

public class TrailerChestScript : MonoBehaviour
{
    private Animator chestAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        chestAnimator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            chestAnimator.SetTrigger("Opened");
        }
    }
}
