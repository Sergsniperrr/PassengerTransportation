using UnityEngine;

namespace Scripts.View.Color
{
    public class ColorSetter : MonoBehaviour
    {
        private MaterialSetter[] _colorers;

        public Material Material { get; private set; }

        private void Awake()
        {
            _colorers = GetComponentsInChildren<MaterialSetter>();
        }

        public void SetMaterial(Material material)
        {
            Material = material;

            foreach (MaterialSetter colorer in _colorers)
                colorer.SetMaterial(material);
        }
    }
}
