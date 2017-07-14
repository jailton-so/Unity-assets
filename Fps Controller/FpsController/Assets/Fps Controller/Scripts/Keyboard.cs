using UnityEngine;
using System.Collections;

public class Keyboard : MonoBehaviour
{
	[Header("Moves")]
	public KeyCode mForward = KeyCode.W;
	public KeyCode mBackward = KeyCode.S;
	public KeyCode mRight = KeyCode.D;
	public KeyCode mLeft = KeyCode.A;

	[Header("Change State")]
	public KeyCode jump = KeyCode.Space;
	public KeyCode run = KeyCode.LeftShift;
	public KeyCode crouch = KeyCode.LeftControl;

	[Header("Hold/Toggle")]
	public bool holdToRun = true;
	public bool holdToCrouch = true;

}