

using UnityEngine;

public class ColorGenerator
{
    private int _next;
    private readonly Color[] _colors =
    {
        new Color(0.956f, 0.262f, 0.211f),
        new Color(0.913f, 0.117f, 0.388f),
        new Color(0.611f, 0.152f, 0.654f),
        new Color(0.403f, 0.227f, 0.717f),
        new Color(0.247f, 0.317f, 0.709f),
        new Color(0.129f, 0.588f, 0.952f),
        new Color(0.011f, 0.662f, 0.956f),
        new Color(0f, 0.737f, 0.831f),
        new Color(0f, 0.588f, 0.533f),
        new Color(0.298f, 0.686f, 0.313f),
        new Color(0.545f, 0.764f, 0.290f),
        new Color(0.803f, 0.862f, 0.223f),
        new Color(1.0f, 0.921f, 0.231f),
        new Color(1.0f, 0.756f, 0.027f)
    };

    public Color GetNext()
    {
        return _colors[_next++ % _colors.Length];
    }
}
