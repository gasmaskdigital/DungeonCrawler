using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [Header("VFXs")]
    [SerializeField] GameObject death;
    [SerializeField] GameObject blood;
    [SerializeField] GameObject fireBall;
    [SerializeField] GameObject fireExplosion;
    [SerializeField] GameObject fireWall;
    [SerializeField] GameObject SwordSwish;
    [SerializeField] GameObject heavySwordSwish;

    public void DeathEffect(Vector3 spawnPos)
    {
        spawnPos.y += 3f;
        GameObject vfxInstance = Instantiate(death, spawnPos, Quaternion.identity);
        Destroy(vfxInstance, 1.5f);
    }

    public void BlodEffect(Vector3 spawnPos)
    {
        spawnPos.y += 1.75f;
        GameObject vfxInstance = Instantiate(blood, spawnPos, Quaternion.identity);
        Destroy(vfxInstance, 1f);
    }

}
