using System;
using System.Collections;
using DG.Tweening;
using Scripts.Model.Coins;
using UnityEngine;

namespace Scripts.View.Windows
{
    public class LevelCompleteWindow : SimpleWindow
    {
        private const float ViewBusDuration = 2f;
        private const float ViewStatisticDuration = 1f;

        [SerializeField] private CanvasGroup _victoryImage;
        [SerializeField] private CanvasGroup _statisticsPanel;
        [SerializeField] private CoinsRecipient _coinsRecipient;
        [SerializeField] private TemporaryCounter _moneyCounter;
        [SerializeField] private TemporaryCounter _scoreCounter;

        private int _money;
        private int _score;

        public override void Open(float delay = 0)
        {
            DisableOkButton();
            _statisticsPanel.alpha = MinTransparent;
            base.Open(delay);
            StartCoroutine(ShowVictoryImage());
        }

        public void InitializeCoins(int money, int score)
        {
            _money = money >= 0 ? money : throw new ArgumentOutOfRangeException(nameof(money));
            _score = score >= 0 ? score : throw new ArgumentOutOfRangeException(nameof(score));

            _moneyCounter.SetValue(_money);
            _scoreCounter.SetValue(_score);
        }

        private void FadeCanvasGroup(CanvasGroup canvasGroup, float targetFade)
        {
            canvasGroup.DOFade(targetFade, FadeDuration);
        }

        private IEnumerator ShowVictoryImage()
        {
            WaitForSeconds wait = new (ViewBusDuration);

            _victoryImage.alpha = MaxTransparent;

            yield return wait;

            FadeCanvasGroup(_victoryImage, MinTransparent);
            FadeCanvasGroup(_statisticsPanel, MaxTransparent);

            StartCoroutine(ShowStatisticAfterDelay());
        }

        private IEnumerator ShowStatisticAfterDelay()
        {
            WaitForSeconds wait = new (ViewStatisticDuration);

            yield return wait;

            _coinsRecipient.Transfer();

            _coinsRecipient.TransferCompleted += ShowOkButton;
        }

        private void ShowOkButton()
        {
            _coinsRecipient.TransferCompleted -= ShowOkButton;

            EnableOkButton();
        }
    }
}