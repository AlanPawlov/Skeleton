using Voody.UniLeo.Lite;

public class AutodestructableProvider : MonoProvider<Autodestructable>
{
    private void Awake()
    {
        value.SpawnTime = System.DateTime.Now;
    }
}
