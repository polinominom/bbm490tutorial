using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DirectionController : MonoBehaviour {

	public delegate void OnMove(Vector3 pos);
	public event OnMove OnCommandMove;

	public Text playernamearea;
	public Text roomnamearea;

	public ButtonController Left;
	public ButtonController Right;
	public ButtonController Up;
	public ButtonController Down;

	[HideInInspector]
	public GameObject player;
	[HideInInspector]
	public float playerSpeed;

	public bool leftMove;
	public bool rightMove;
	public bool forwardMove;
	public bool backMove;



	void Update()
	{
		if (player != null) 
		{
			Vector3 dir = Vector3.zero;
			float speed = Time.deltaTime * playerSpeed;
			if (leftMove) {
				dir += new Vector3 (-1f, 0f, 0f) * speed;
			}

			if (rightMove) {
				dir += new Vector3 (1f, 0f, 0f) * speed;
			}

			if (forwardMove) {
				dir += new Vector3 (0f, 0f, 1f) * speed;
			}

			if (backMove) {
				dir += new Vector3 (0f, 0f, -1f) * speed;
			}

			if (leftMove || rightMove || forwardMove || backMove) {
				if (OnCommandMove != null) {
					OnCommandMove (player.transform.position + dir);
				}
			}

		}
	}

	public void DirectionAction()
	{
		Left.OnPress += OnPress;
		Right.OnPress += OnPress;
		Up.OnPress += OnPress;
		Down.OnPress += OnPress;
	}

	void OnPress (GameObject unit, bool state)
	{
		switch (unit.name) 
		{
			case("LeftButton"):
				leftMove = state;
				break;

			case("RightButton"):
				rightMove = state;
				break;

			case("UpButton"):
				forwardMove = state;
				break;

			case("DownButton"):
				backMove = state;
				break;
			default:
				break;
		}

//		Debug.Log ("button clicked unit name: "+unit.name);
	}


	public void SetPlayer(GameObject player, float speed)
	{
		this.player = player;
		this.playerSpeed = speed;

		playernamearea.text = player.name;

		roomnamearea.text = player.GetComponent<PlayerScript> ().room;

	}

}
