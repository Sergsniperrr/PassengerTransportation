using System;
using System.Collections;
using Scripts.Model.Coins;
using Scripts.Model.Score;
using UnityEngine;

namespace Scripts.View.Windows
{
    public class CoinsRecipient : MonoBehaviour
    {
        private const float DelayBeforeTransfer = 0.5f;
        private const float ScoreTransferDelay = 0.5f;
        private const float DurationPerOne = 0.07f;

        [SerializeField] private CoinsSpawner _moneyCoinsSpawner;
        [SerializeField] private CoinsSpawner _scoreCoinsSpawner;
        [SerializeField] private TemporaryCounter _money;
        [SerializeField] private TemporaryCounter _score;
        [SerializeField] private MoneyCounter _moneyWallet;
        [SerializeField] private ScoreCounter _scoreWallet;
        [SerializeField] private float _maxTransferDuration = 3f;

        public event Action TransferCompleted;

        public void Transfer() =>
            StartCoroutine(StartTransferAfterDelay());

        private IEnumerator StartTransferAfterDelay()
        {
            WaitForSeconds wait = new (DelayBeforeTransfer);

            yield return wait;

            StartTransfer();
        }

        private void StartTransfer()
        {
            if (_money.Value == 0)
            {
                StartTransferScore();
                return;
            }

            _moneyCoinsSpawner.StartSpawn();
            StartCoroutine(StartReceiptAfterDelay(_moneyWallet, _money.Value, _moneyCoinsSpawner));
            _money.ResetValue(CalculateTransferDuration(_money.Value));
            _money.ResetCompleted += StartTransferScore;
        }

        private void StartTransferScore()
        {
            _money.ResetCompleted -= StartTransferScore;
            _moneyCoinsSpawner.StopSpawn();


            if (_score.Value == 0)
            {
                StartCoroutine(FinishTransferAfterDelay());
                return;
            }

            StartCoroutine(TransferScoreAfterDelay());
        }

        private IEnumerator TransferScoreAfterDelay()
        {
            WaitForSeconds wait = new (ScoreTransferDelay);

            yield return wait;

            _scoreCoinsSpawner.StartSpawn();
            StartCoroutine(StartReceiptAfterDelay(_scoreWallet, _score.Value, _scoreCoinsSpawner));
            _score.ResetValue(CalculateTransferDuration(_score.Value));

            _score.ResetCompleted += FinishTransfer;
        }

        private void FinishTransfer()
        {
            _score.ResetCompleted -= FinishTransfer;

            _scoreCoinsSpawner.StopSpawn();
            StartCoroutine(FinishTransferAfterDelay());
        }

        private IEnumerator FinishTransferAfterDelay()
        {
            WaitForSeconds wait = new (ScoreTransferDelay);

            yield return wait;

            TransferCompleted?.Invoke();
        }

        private IEnumerator StartReceiptAfterDelay(CoinsCounter counter, int coinsForTransfer, CoinsSpawner spawner)
        {
            if (counter == null)
                throw new ArgumentNullException(nameof(counter));

            WaitForSeconds wait = new (spawner.MoveDuration);

            yield return wait;

            counter.Add(coinsForTransfer, CalculateTransferDuration(coinsForTransfer));
        }

        private float CalculateTransferDuration(int coinsCount) =>
            Mathf.Min(DurationPerOne * coinsCount, _maxTransferDuration);
    }
}