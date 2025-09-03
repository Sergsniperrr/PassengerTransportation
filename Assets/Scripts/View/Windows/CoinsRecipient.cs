using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsRecipient : MonoBehaviour
{
    [SerializeField] private CoinsSpawner _moneyCoinsSpawner;
    [SerializeField] private CoinsSpawner _scoreCoinsSpawner;
    [SerializeField] private TemporaryCounter _money;
    [SerializeField] private TemporaryCounter _score;
    [SerializeField] private MoneyCounter _moneyWallet;
    [SerializeField] private ScoreCounter _scoreWallet;

    private float _delayBeforeTransfer = 0.5f;

    private float _scoreTransferDelay = 0.5f;
    private float _durationPerOne = 0.07f; //0.06f
    //private float _receiptDelay = 0.7f;

    public event Action TransferCompleted;

    public void Transfer() =>
        StartCoroutine(StartTransferAfterDelay());

    private IEnumerator StartTransferAfterDelay()
    {
        WaitForSeconds wait = new(_delayBeforeTransfer);

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

        _moneyCoinsSpawner.StartSpawn(_durationPerOne);
        StartCoroutine(StartReceiptAfterDelay(_moneyWallet, _money.Value, _moneyCoinsSpawner));
        _money.ResetValue(_durationPerOne);
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
        WaitForSeconds wait = new(_scoreTransferDelay);

        yield return wait;

        _scoreCoinsSpawner.StartSpawn(_durationPerOne);
        StartCoroutine(StartReceiptAfterDelay(_scoreWallet, _score.Value, _scoreCoinsSpawner));
        _score.ResetValue(_durationPerOne);

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
        WaitForSeconds wait = new(_scoreTransferDelay);

        yield return wait;

        TransferCompleted?.Invoke();
    }

    private IEnumerator StartReceiptAfterDelay(CoinsCounter counter, int coinsForTransfer, CoinsSpawner spawner)
    {
        if (counter == null)
            throw new ArgumentNullException(nameof(counter));

        WaitForSeconds wait = new(spawner.MoveDuration);

        yield return wait;

        counter.Add(coinsForTransfer);
    }
}
