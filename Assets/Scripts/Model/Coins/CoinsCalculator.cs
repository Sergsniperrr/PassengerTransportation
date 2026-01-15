using UnityEngine;

namespace Scripts.Model.Coins
{
    public static class CoinsCalculator
    {
        public static int CalculateMoney(int level, int busCount, float playerSkillRatio)
        {
            const float RewardRatioIncrement = 0.1f;
            const int MaxNonRewardLevel = 14;
            float rewardRatio = Mathf.Max(0, level - MaxNonRewardLevel) * RewardRatioIncrement;

            return Mathf.RoundToInt(busCount * rewardRatio * playerSkillRatio);
        }

        public static int CalculateScore(int busCount, int moneySpent, float playerSkillRatio)
        {
            const float MaxSkillRatio = 1f;
            const float MinSkillRatio = 0.6f;
            const float Coefficient = 0.5f;
            const float Multiplier = 2f;
            float score = busCount / (Coefficient * moneySpent + busCount) * busCount * Multiplier;
            float skillRatioModified;

            if (playerSkillRatio >= MaxSkillRatio)
            {
                skillRatioModified = MaxSkillRatio;
            }
            else
            {
                const float SkillRatioMultiplier = MaxSkillRatio - MinSkillRatio;

                skillRatioModified = playerSkillRatio * SkillRatioMultiplier + MinSkillRatio;
            }

            return Mathf.RoundToInt(score / skillRatioModified);
        }
    }
}