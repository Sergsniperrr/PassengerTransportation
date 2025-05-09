using UnityEngine;

public class ColorSetter : MonoBehaviour
{
    private MaterialSetter[] _colorers;

    public Material Material { get; private set; }

    private void Awake()
    {
        _colorers = GetComponentsInChildren<MaterialSetter>();
    }

    public void SetMateral(Material material)
    {
        Material = material;

        foreach (MaterialSetter colorer in _colorers)
            colorer.SetMateral(material);
    }
}
