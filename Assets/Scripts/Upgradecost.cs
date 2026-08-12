using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Upgradecost : MonoBehaviour
{
    static int healthCost = 100;
    static int strengthCost = 100;
    static int dexterityCost = 100;
    static int magicCost = 100;
    static int enduranceCost = 100;

    //cost increase

    [SerializeField] int healthCostIncrease = 50;
    [SerializeField] int strengthCostIncrease = 50;
    [SerializeField] int dexterityCostIncrease = 50;
    [SerializeField] int magicCostIncrease = 50;
    [SerializeField] int enduranceCostIncrease = 50;

    //prices

    [SerializeField] private TMP_Text Health;
    [SerializeField] private TMP_Text Strength;
    [SerializeField] private TMP_Text Dexterity;
    [SerializeField] private TMP_Text Magic;
    [SerializeField] private TMP_Text Endurance;

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
        UpdatePrice();

    }

    public void UpdatePrice()
    {
        Health.text = healthCost.ToString();
        Strength.text = strengthCost.ToString();
        Dexterity.text = dexterityCost.ToString();
        Magic.text = magicCost.ToString();
        Endurance.text = enduranceCost.ToString();
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

        UpdatePrice();
    }

    public void DexterityUpgrade()
    {
        if (PlayerStats.currency < dexterityCost)
            return;

        PlayerStats.currency -= dexterityCost;
        startingDexterity++;

        dexterityCost += dexterityCostIncrease;

        UpdatePrice();
    }

    public void MagicUpgrade()
    {
        if (PlayerStats.currency < magicCost)
            return;

        PlayerStats.currency -= magicCost;
        startingMagic++;

        magicCost += magicCostIncrease;

        UpdatePrice();
    }

    public void EnduranceUpgrade()
    {
        if (PlayerStats.currency < enduranceCost)
            return;

        PlayerStats.currency -= enduranceCost;
        startingEndurance++;

        enduranceCost += enduranceCostIncrease;

        UpdatePrice();
    }
}
