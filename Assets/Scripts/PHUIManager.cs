using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PHUIManager : MonoBehaviour
{
    [SerializeField] GameObject levelUpScreen;
    [SerializeField] PlayerStats playerStats;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void EnableLevelUpScreen()
    {
        levelUpScreen.SetActive(true);
    }

    public void HealthUpgrade()
    {
        PlayerStats.healthStat++;
        playerStats.UpdateMaxHealth();
        levelUpScreen.SetActive(false);
    }

    public void StrengthUpgrade()
    {
        PlayerStats.strengthStat++;
        levelUpScreen.SetActive(false);
    }

    public void DexterityUpgrade()
    {
        PlayerStats.dexterityStat++;
        levelUpScreen.SetActive(false);
    }

    public void MagicUpgrade()
    {
        PlayerStats.magicStat++;
        levelUpScreen.SetActive(false);
    }

    public void EnduranceUpgrade()
    {
        PlayerStats.enduranceStat++;
        levelUpScreen.SetActive(false);
    }

}
