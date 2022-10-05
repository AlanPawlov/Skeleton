using UnityEngine;

[System.Serializable]
public struct MeleeAttack
{
    public int Damage;
    public float Distance;
    public Vector3 Offset;
    public LayerMask LayerMask;
}
