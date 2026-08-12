using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Upgradecost : MonoBehaviour
{
    static int healthCost = 100;
    static int strengthCost = 150;
    static int dexterityCost = 125;
    static int magicCost = 200;
    static int enduranceCost = 175;

    [SerializeField] int healthCostIncrease = 50;
    [SerializeField] int strengthCostIncrease = 50;
    [SerializeField] int dexterityCostIncrease = 50;
    [SerializeField] int magicCostIncrease = 50;
    [SerializeField] int enduranceCostIncrease = 50;

    public static int startingHealth = 1;
    public static int startingStrength = 1;
    public static int startingDexterity = 1;
    public static int startingMagic = 1;
    public static int startingEndurance = 1;

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
        startingHealth++;

        healthCost += healthCostIncrease;
    }

    public void StrengthUpgrade()
    {
        if (PlayerStats.currency < strengthCost)
            return;

        PlayerStats.currency -= strengthCost;
        startingStrength++;

        strengthCost += strengthCostIncrease;
    }

    public void DexterityUpgrade()
    {
        if (PlayerStats.currency < dexterityCost)
            return;

        PlayerStats.currency -= dexterityCost;
        startingDexterity++;

        dexterityCost += dexterityCostIncrease;
    }

    public void MagicUpgrade()
    {
        if (PlayerStats.currency < magicCost)
            return;

        PlayerStats.currency -= magicCost;
        startingMagic++;

        magicCost += magicCostIncrease;
    }

    public void EnduranceUpgrade()
    {
        if (PlayerStats.currency < enduranceCost)
            return;

        PlayerStats.currency -= enduranceCost;
        startingEndurance++;

        enduranceCost += enduranceCostIncrease;
    }
}
