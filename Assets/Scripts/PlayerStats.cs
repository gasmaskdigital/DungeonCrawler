using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int healthStat = 1;
    public int strengthStat = 1;
    public int dexterityStat = 1;
    public int magicStat = 1;
    public int enduranceStat = 1;
    public int playerLevel = 1;
    public int currentXP = 0;
    public int requiredXP = 150;

    public int earntXP;
    private int randomStat;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToXP()
    {
        currentXP += earntXP;
        if(currentXP > requiredXP)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        randomStat = Random.Range(0, 4);

        switch (randomStat)
        {
            case 0:
                healthStat++;
                break;
            case 1:
                strengthStat++;
                break;
            case 2:
                dexterityStat++;
                break;
            case 3:
                magicStat++;
                break;
            case 4:
                enduranceStat++;
                break;                
        }

        currentXP = 0;
        requiredXP = requiredXP + 125;
        playerLevel++;
    }
}
