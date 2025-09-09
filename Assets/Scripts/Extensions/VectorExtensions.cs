using UnityEngine;

public static class VectorExtensions
{
    /// <summary>
    /// Конвертує Vector2 в Vector2Int з округленням до найближчого цілого
    /// </summary>
    public static Vector2Int ToVector2Int(this Vector2 vector)
    {
        return new Vector2Int(
            Mathf.RoundToInt(vector.x),
            Mathf.RoundToInt(vector.y)
        );
    }
}