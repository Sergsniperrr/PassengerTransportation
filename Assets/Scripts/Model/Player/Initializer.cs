using UnityEngine;
using YG;

public class Initializer : MonoBehaviour
{
    private readonly int _initialLevel = 1;
    private readonly int _initialMoney = 0;
    private readonly int _initialScore = 0;
    private readonly int _initialTotalBusesCount = 0;
    private readonly int _initialTotalAdsViewsCount = 0;
    private readonly float _initialPlayerSkill = 1;

    private void Awake()
    {
        YG2.onDefaultSaves += FirstInitializePlayerData;
    }

    private void Start()
    {
        YG2.onDefaultSaves -= FirstInitializePlayerData;
    }

    private void FirstInitializePlayerData()
    {
        YG2.saves.WriteLevel(_initialLevel);
        YG2.saves.WriteMoney(_initialMoney);
        YG2.saves.WriteScore(_initialScore);
        YG2.saves.WriteTotalBusesCount(_initialTotalBusesCount);
        YG2.saves.WriteTotalAdsViewsCount(_initialTotalAdsViewsCount);
        YG2.saves.WritePlayerSkill(_initialPlayerSkill);

        Debug.Log($"PlayerSkillInitialized: {YG2.saves.PlayerSkill} !!!");
    }
}
