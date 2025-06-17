using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColorsHandler : MonoBehaviour, IColorGetter
{
    private Queue<Material> _colors;

    public int ColorsCount => _colors.Count;
    public List<Material> Colors => _colors.ToList();

    public void InitializePassengerColors() =>
        _colors = CreateRandomColors(ReadColors());

    public Material DequeuePassengerColor() =>
        _colors.Dequeue();

    public void SetColorsQueue(List<Material> colors)
    {
        _colors.Clear();
        _colors = new Queue<Material>(colors);
    }

    private List<Material> ReadColors()
    {
        List<Material> materials = new();
        Bus[] buses = GetComponentsInChildren<Bus>();

        foreach (Bus bus in buses)
            for (int i = 0; i < bus.SeatsCount; i++)
                materials.Add(bus.Material);

        return materials;
    }

    private Queue<Material> CreateRandomColors(List<Material> materials)
    {
        Queue<Material> randomMaterials = new();
        int randomIndex;

        while (materials.Count > 0)
        {
            randomIndex = UnityEngine.Random.Range(0, materials.Count);
            randomMaterials.Enqueue(materials[randomIndex]);
            materials.RemoveAt(randomIndex);
        }

        return randomMaterials;
    }
}
