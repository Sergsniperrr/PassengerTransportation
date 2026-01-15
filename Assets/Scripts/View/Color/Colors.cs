using System.Collections.Generic;
using UnityEngine;

namespace Scripts.View.Color
{
    public class Colors : MonoBehaviour
    {
        [SerializeField] private List<Material> _materals;

        public int MaterialsCount => _materals.Count;

        public Material GetRandomColor() =>
            _materals[Random.Range(0, _materals.Count)];

        public int GetIndexOfMaterial(Material material) =>
            _materals.IndexOf(material);

        public Material GetMaterial(int index) =>
            _materals[index];

        public int[] GetMaterialsIndexes(List<Material> materials)
        {
            int[] indexes = new int[materials.Count];

            for (int i = 0; i < indexes.Length; i++)
                indexes[i] = GetIndexOfMaterial(materials[i]);

            return indexes;
        }
    }
}