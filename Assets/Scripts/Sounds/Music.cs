using UnityEngine;

namespace Scripts.Sounds
{
    public class Music : MonoBehaviour
    {
        private const int MinVolume = 0;
        private const int MaxVolume = 1;

        [SerializeField] private AudioSource _music;

        private float _duration;

        public void Play()
        {
            _music.volume = MaxVolume;
            _music.Play();
        }

        public void Stop()
        {
            _music.volume = MinVolume;
        }
    }
}