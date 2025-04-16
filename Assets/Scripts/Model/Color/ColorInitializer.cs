using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorInitializer : MonoBehaviour, IColorGetter
{
    private Queue<Material> _colors;

    public int ColorsCount => _colors.Count;

    public void InitializeColors() =>
        _colors = CreateRandomColors(ReadColors());

    public int MaterialsCount => _colors.Count;

    public Material DequeuePassengerColor() =>
        _colors.Dequeue();

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
            randomIndex = Random.Range(0, materials.Count);
            randomMaterials.Enqueue(materials[randomIndex]);
            materials.RemoveAt(randomIndex);
        }

        return randomMaterials;
    }
}
