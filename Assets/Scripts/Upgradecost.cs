using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Upgradecost : MonoBehaviour
{
    [SerializeField] int healthCost = 100;
    [SerializeField] int strengthCost = 150;
    [SerializeField] int dexterityCost = 125;
    [SerializeField] int magicCost = 200;
    [SerializeField] int enduranceCost = 175;

    [SerializeField] int healthCostIncrease = 50;
    [SerializeField] int strengthCostIncrease = 50;
    [SerializeField] int dexterityCostIncrease = 50;
    [SerializeField] int magicCostIncrease = 50;
    [SerializeField] int enduranceCostIncrease = 50;

    [SerializeField] private TMP_Text currencyText;

    //[SerializeField] PlayerStats playerStats;
    //[SerializeField] PlayerStats currency;

    void Start()
    {
        //playerStats = PlayerStats.Instance;
    }

    public void HealthUpgrade()
    {
        if (PlayerStats.currency < healthCost)
            return;

        PlayerStats.currency -= healthCost;
        PlayerStats.healthStat++;

        healthCost += healthCostIncrease;
    }

    public void StrengthUpgrade()
    {
        if (PlayerStats.currency < strengthCost)
            return;

        PlayerStats.currency -= strengthCost;
        PlayerStats.strengthStat++;

        strengthCost += strengthCostIncrease;
    }

    public void DexterityUpgrade()
    {
        if (PlayerStats.currency < dexterityCost)
            return;

        PlayerStats.currency -= dexterityCost;
        PlayerStats.dexterityStat++;

        dexterityCost += dexterityCostIncrease;
    }

    public void MagicUpgrade()
    {
        if (PlayerStats.currency < magicCost)
            return;

        PlayerStats.currency -= magicCost;
        PlayerStats.magicStat++;

        magicCost += magicCostIncrease;
    }

    public void EnduranceUpgrade()
    {
        if (PlayerStats.currency < enduranceCost)
            return;

        PlayerStats.currency -= enduranceCost;
        PlayerStats.enduranceStat++;

        enduranceCost += enduranceCostIncrease;
    }
}
