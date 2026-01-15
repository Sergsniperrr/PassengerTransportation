using System.Linq;
using UnityEngine;

namespace Scripts.View.Color
{
    [RequireComponent(typeof(Renderer))]
    public class MaterialSetter : MonoBehaviour
    {
        [SerializeField] private int _materalIndex;

        private Renderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        public void SetMaterial(Material material)
        {
            if (_renderer == null)
                return;

            Material[] materials = _renderer.materials.ToArray();
            materials[_materalIndex] = material;
            _renderer.materials = materials;
        }
    }
}