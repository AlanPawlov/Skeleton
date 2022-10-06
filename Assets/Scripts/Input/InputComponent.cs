using UnityEngine;

[System.Serializable]
public struct InputComponent
{
	public Vector2 Move;
	public Vector2 Look;
	public bool Jump;
	public bool Shot;
	public bool Pause;
}
