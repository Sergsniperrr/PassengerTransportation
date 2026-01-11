using UnityEngine;

namespace Scripts.Sounds
{
    public class Effects : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _fireWork;
        [SerializeField] private ParticleSystem _crashParticle;
        [SerializeField] private AudioSource _crashAudio;
        [SerializeField] private AudioSource _boardingBusAudio;
        [SerializeField] private AudioSource _levelCompleteAudio;
        [SerializeField] private AudioSource _busFillingCompleteAudio;
        [SerializeField] private AudioSource _coinsAudio;
        [SerializeField] private CoinsOnBusStop _coins;

        private readonly float _minRandomPitch = 0.85f;
        private readonly float _maxRandomPitch = 1.15f;

        public void PlayCrash(Vector3 position, Transform busTransform)
        {
            Transform parent = _crashParticle.transform.parent;

            _crashParticle.transform.SetParent(busTransform);
            _crashParticle.transform.localPosition = position;
            _crashParticle.transform.SetParent(parent);

            _crashParticle.Play();
            _crashAudio.Play();
        }

        public void PlayBoardingBus()
        {
            _boardingBusAudio.pitch = Random.Range(_minRandomPitch, _maxRandomPitch);
            _boardingBusAudio.PlayOneShot(_boardingBusAudio.clip);
        }

        public void PlayLevelComplete()
        {
            _fireWork.Play();
            _levelCompleteAudio.Play();
        }

        public void PlayBusFillingComplete(int busStopIndex)
        {
            _coins.ShowCoin(busStopIndex);
            _busFillingCompleteAudio.PlayOneShot(_busFillingCompleteAudio.clip);
        }

        public void PlayCoinsAudio()
        {
            _coinsAudio.PlayOneShot(_coinsAudio.clip);
        }
    }
}
