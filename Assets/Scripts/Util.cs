using UnityEngine;

public class Util 
{
    public static Vector2 ClampVectorX(Vector2 vector, float min, float max)
    {
        if (vector.x < min)
            return new Vector2(min, vector.y);
        else if (vector.x > max)
            return new Vector2(max, vector.y);

        return vector;
    }
    
    public static Vector2 ClampVectorY(Vector2 vector, float min, float max)
    {
        if (vector.y < min)
            return new Vector2(vector.x, min);
        else if (vector.y > max)
            return new Vector2(vector.x, max);

        return vector;
    }
}