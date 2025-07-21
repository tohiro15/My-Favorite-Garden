using UnityEngine;

[DisallowMultipleComponent]
public class Holdable : MonoBehaviour
{
    [Header("Hand Transform Settings")]
    public Vector3 positionOffset = Vector3.zero;
    public Vector3 rotationOffset = Vector3.zero;
}
