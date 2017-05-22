using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

	public string playerName;
	public string id;
	public string room;

	public void SetPlayer(string name, string room, Vector3 pos)
	{
		playerName = name;
		this.name = name;
		this.room = room;
		transform.position = pos;
		transform.GetChild (0).GetComponent<TextMesh> ().text = name;
	}
}
