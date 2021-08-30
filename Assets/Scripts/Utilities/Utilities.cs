
using UnityEngine;

/// <summary>
/// Contains static utility methods which are universally useful throughout the program.
/// </summary>
public class Utilities {
    public static T RequireNonNull<T>(T obj) {
        if (obj == null) {
            Debug.LogError("Object cannot be null.");
        }
        return obj;
    }
}