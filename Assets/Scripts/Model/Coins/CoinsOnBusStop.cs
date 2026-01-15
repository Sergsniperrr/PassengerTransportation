using System;
using Scripts.Sounds;
using UnityEngine;

namespace Scripts.Model.Coins
{
    public class CoinsOnBusStop : MonoBehaviour
    {
        [SerializeField] private Coin _prefab;
        [SerializeField] private int _positionsCount;
        [SerializeField] private Vector3 _startingPosition;
        [SerializeField] private Vector3 _endingPosition;
        [SerializeField] private Transform _target;
        [SerializeField] private Effects _effects;

        private Coin[] _coins;
        private Vector3[] _positions;

        public event Action CoinResieved;

        private void Awake()
        {
            _positions = CalculatePositions();
            _coins = new Coin[_positions.Length];
            Coin coin;

            for (int i = 0; i < _positions.Length; i++)
            {
                coin = Instantiate(_prefab, _positions[i], Quaternion.identity, transform);

                coin.Died += FinishCoinMoving;

                coin.gameObject.SetActive(false);
                _coins[i] = coin;
            }
        }

        private void OnDisable()
        {
            foreach (Coin coin in _coins)
                coin.Died -= FinishCoinMoving;
        }

        public void ShowCoin(int busStopIndex)
        {
            _coins[busStopIndex].gameObject.SetActive(true);
            _coins[busStopIndex].Show(_target.position);
        }

        private Vector3[] CalculatePositions()
        {
            Vector3 step = (_endingPosition - _startingPosition) / (_positionsCount - 1);
            Vector3[] positions = new Vector3[_positionsCount];

            for (int i = 0; i < _positionsCount; i++)
                positions[i] = step * i + _startingPosition;

            return positions;
        }

        private void FinishCoinMoving(Coin coin)
        {
            _effects.PlayCoinsAudio();

            CoinResieved?.Invoke();
        }
    }
}