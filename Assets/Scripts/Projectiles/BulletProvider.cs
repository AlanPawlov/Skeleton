using UnityEngine;
using Voody.UniLeo.Lite;

public class BulletProvider : MonoProvider<Bullet>
{
    private void Awake()
    {
        value.Direction = transform.TransformDirection(Vector3.forward);
    }
}
