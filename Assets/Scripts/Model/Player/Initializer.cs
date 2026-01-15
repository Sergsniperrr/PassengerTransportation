using System.Collections;
using UnityEngine;
using YG;

namespace Scripts.Model.Player
{
    public class Initializer : MonoBehaviour
    {
        private const int InitialLevel = 1;
        private const int InitialMoney = 0;
        private const int InitialScore = 0;
        private const int InitialTotalBusesCount = 0;
        private const int InitialTotalAdsViewsCount = 0;
        private const float InitialPlayerSkill = 1;

        private void Awake()
        {
            YG2.onDefaultSaves += FirstInitializePlayerData;
        }

        private void Start()
        {
            YG2.onDefaultSaves -= FirstInitializePlayerData;
        }

        private IEnumerator WaitForPluginInit()
        {
            while (YG2.isSDKEnabled == false)
            {
                yield return null;
            }

            YG2.SaveProgress();
        }

        private void FirstInitializePlayerData()
        {
            YG2.saves.WriteLevel(InitialLevel);
            YG2.saves.WriteMoney(InitialMoney);
            YG2.saves.WriteScore(InitialScore);
            YG2.saves.WriteTotalBusesCount(InitialTotalBusesCount);
            YG2.saves.WriteTotalAdsViewsCount(InitialTotalAdsViewsCount);
            YG2.saves.WritePlayerSkill(InitialPlayerSkill);

            StartCoroutine(WaitForPluginInit());
        }
    }
}