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
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }

    public void EnableLevelUpScreen()
    {
        levelUpScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void HealthUpgrade()
    {
        PlayerStats.healthStat++;
        playerStats.HealthLevelUp();
        levelUpScreen.SetActive(false);
        Time.timeScale = 1f;
    }

    public void StrengthUpgrade()
    {
        PlayerStats.strengthStat++;
        levelUpScreen.SetActive(false);
        Time.timeScale = 1f;
    }

    public void DexterityUpgrade()
    {
        PlayerStats.dexterityStat++;
        levelUpScreen.SetActive(false);
        Time.timeScale = 1f;
    }

    public void MagicUpgrade()
    {
        PlayerStats.magicStat++;
        levelUpScreen.SetActive(false);
        Time.timeScale = 1f;
    }

    public void EnduranceUpgrade()
    {
        PlayerStats.enduranceStat++;
        levelUpScreen.SetActive(false);
        Time.timeScale = 1f;
    }

}
