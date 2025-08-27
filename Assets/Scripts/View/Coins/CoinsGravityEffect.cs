using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class CoinsGravityEffect : MonoBehaviour
{
    [SerializeField] private Transform _busCoinsPosition;
    [SerializeField] private Transform _scoreCoinsPosition;
    [SerializeField] private Transform _busCoinsTarget;
    [SerializeField] private Transform _scoreCoinsTarget;
    [SerializeField] private Material _busCoinsMateral;
    [SerializeField] private Material _scoreCoinsMateral;
    [SerializeField] private Transform _parent;

    private ParticleSystem _particleSystem;
    private Renderer _renderer;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _renderer = _particleSystem.GetComponent<Renderer>();
        //transform.SetParent(_parent);
    }

    public void PlayMoney() =>
        Play(_busCoinsTarget, _busCoinsMateral, _busCoinsPosition.position);

    public void PlayScore() =>
        Play(_scoreCoinsTarget, _scoreCoinsMateral, _scoreCoinsPosition.position);

    public void Stop() =>
        _particleSystem.Stop();

    private void Play(Transform target, Material materal, Vector3 position)
    {
        transform.position = position;
        transform.LookAt(target);
        _renderer.material = materal;
        _particleSystem.Play();
    }
}
