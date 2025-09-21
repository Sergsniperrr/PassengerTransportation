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
        float maxSkillRatio = 1f;
        float minSkillRatio = 0.6f;
        float coefficient = 0.5f;
        float multiplier = 2f;
        float score = busCount / (coefficient * moneySpent + busCount) * busCount * multiplier;
        float skillRatioModified;

        if (playerSkillRatio >= maxSkillRatio)
        {
            skillRatioModified = maxSkillRatio;
        }
        else
        {
            float skillRatioMultiplier = maxSkillRatio - minSkillRatio;

            skillRatioModified = playerSkillRatio * skillRatioMultiplier + minSkillRatio;
        }

        return Mathf.RoundToInt(score / skillRatioModified);
    }
}
