using UnityEngine;

public enum PopperColor
{
    None = 0,
    Purple = 1, // 1 hit
    Blue = 2,   // 2 hits
    Yellow = 3  // 3 hits
}

[CreateAssetMenu(fileName = "LevelDefinition", menuName = "CrazyPoppers/LevelDefinition")]
public class LevelDefinition : ScriptableObject
{
    public int levelIndex;
    public int width;
    public int height;
    public int maxTaps;

    // size = width * height, row-major
    public PopperColor[] grid;
}
