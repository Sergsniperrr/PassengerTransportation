using UnityEngine;

public class Effects : MonoBehaviour
{
    [SerializeField] private ParticleSystem _crashParticle;
    [SerializeField] private ParticleSystem _fireWork;
    [SerializeField] private AudioSource _crashAudio;
    [SerializeField] private AudioSource _boardingBusAudio;
    [SerializeField] private AudioSource _levelCompleteAudio;
    [SerializeField] private AudioSource _busFillingCompleteAudio;

    private readonly float _minRandomPitch = 0.85f;
    private readonly float _maxRandomPitch = 1.15f;

    public void PlayCrash(Vector3 position)
    {
        _crashParticle.transform.position = position;
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

    public void PlayBusFillingComplete()
    {
        _busFillingCompleteAudio.PlayOneShot(_busFillingCompleteAudio.clip);
    }
}
