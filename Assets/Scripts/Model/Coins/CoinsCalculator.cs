using UnityEngine;

public static class CoinsCalculator
{
    public static int CalculateMoney(int level, int busCount, float playerSkillRatio)
    {
        float rewardRatioIncrement = 0.1f;
        int maxNonRewardLevel = 14;
        float rewardRatio = Mathf.Max(0, level - maxNonRewardLevel) * rewardRatioIncrement;

        return Mathf.RoundToInt(busCount * rewardRatio * playerSkillRatio);
    }

    public static int CalculateScore(int busCount, int moneySpent, float playerSkillRatio)
    {
        float _coefficient = 0.5f;
        float _multiplier = 2f;

        return (int)Mathf.Round(busCount / (_coefficient * moneySpent + busCount) * busCount * _multiplier);
    }
}
