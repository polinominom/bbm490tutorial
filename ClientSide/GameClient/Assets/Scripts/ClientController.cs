using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using SocketIO;

public class ClientController : MonoBehaviour 
{	
	// client side of the project.
	public SocketIOComponent socket;
	
	public LoginPanelContoller loginController;
	public DirectionController directionController;
	public ChatController chat;

	public GameObject player;

	public float speed;


	void Start () 
	{
		StartCoroutine (ServerConnect ());

		socket.On ("user_connected", OnUserConnected );
		socket.On ("move", OnUserMove);
		socket.On ("user_disconnected", OnUserDisconnect);
		socket.On ("play", OnUserPlay);
		socket.On ("user_awake", OnServerRoomInfo);
	
		loginController.playBtn.onClick.AddListener (OnPlayClick);
		 
		directionController.OnCommandMove += OnCommandMove;
		directionController.gameObject.SetActive (false);
		chat.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		

	}

	IEnumerator ServerConnect()
	{
		yield return new WaitForSeconds (0.5f);


		// to recieve the players that entered this game before
		socket.Emit ("user_connect");
	}

	// The client moved, moved it in this client and inform the client!
	void OnCommandMove(Vector3 pos)
	{
		Dictionary<string, string> data = new Dictionary<string, string> ();

		data ["position"] = pos.ToString ();

		socket.Emit ("move", new JSONObject(data));
	}


	// Other user connected! The server informs this client in here.
	// So create an object that represents the connected player and add it to this scene.
	private void OnUserConnected(SocketIOEvent evt)
	{
		string name = JsonConverter.JsonToString (evt.data.GetField("name").ToString(), "\"");
		string room = JsonConverter.JsonToString(evt.data.GetField("room").ToString(), "\"");
		Vector3 pos = JsonConverter.JsonToVector3 (evt.data.GetField ("position").ToString ());

		Debug.Log ("A user has just connected to the server!");
		string currentUserRoom = directionController.player.GetComponent<PlayerScript> ().room;

		GameObject newPlayer = Instantiate (player, player.transform.position, Quaternion.identity);
		PlayerScript otherPlayer = newPlayer.GetComponent<PlayerScript> ();

		otherPlayer.SetPlayer (name, room, pos);
		otherPlayer.transform.rotation = Quaternion.identity;
	}


	// the client must knows the how many rooms avaliable.
	private void OnServerRoomInfo(SocketIOEvent evt)
	{
		string room = JsonConverter.JsonToString (evt.data.GetField("room").ToString(), "\"");
		loginController.CollectInformation (room);
	}


	// A player moved in their scene, recieve the information and move it in this clients scene too.
	private void OnUserMove(SocketIOEvent evt)
	{
		string name = JsonConverter.JsonToString (evt.data.GetField ("name").ToString(), "\"");
		Vector3 pos = JsonConverter.JsonToVector3 (evt.data.GetField ("position").ToString ());

		Debug.Log("playerName: "+name);
		Debug.Log("playerPosition: "+pos);

		GameObject playerObj = GameObject.Find (name) as GameObject;
		playerObj.transform.position = pos;

	}


	// Player Disconnected!
	// Tries to delete a user who just disconnected from the scene!
	private void OnUserDisconnect(SocketIOEvent evt)
	{
		string playerName = JsonConverter.JsonToString (evt.data.GetField ("name").ToString (), "\"");
		Destroy (GameObject.Find (playerName));
	}


	// Play clicked!
	// Tries to send the name and position to the server!
	private void OnPlayClick()
	{
		Debug.Log ("OnPlayClick");
		string name = loginController.inputField.text;
		string room = loginController.GetSelectedRoom ();
		if (name != "") {
			Dictionary<string, string> data = new Dictionary<string, string> ();
			data ["name"] = name;
			data ["position"] = new Vector3 (0f, 0f, 0f).ToString ();
			data ["room"] = room;

			socket.Emit ("play", new JSONObject (data));
		} else {
			loginController.inputField.text = "Please enter your name";
		}
	}

	// Player clicked play -> then server calls this callback function.
	private void OnUserPlay(SocketIOEvent evt)
	{
		Debug.Log ("OnUserPlay");
		loginController.gameObject.SetActive (false);

		directionController.gameObject.SetActive (true);
		directionController.DirectionAction ();

		chat.gameObject.SetActive (true);
		chat.ActivateChat ();

		GameObject selfObj = Instantiate (player, player.transform.position, Quaternion.identity);

		PlayerScript self = selfObj.GetComponent<PlayerScript> ();
	
		string name = JsonConverter.JsonToString (evt.data.GetField ("name").ToString(), "\"");
		string room = JsonConverter.JsonToString (evt.data.GetField ("room").ToString (), "\"");
		Vector3 pos = JsonConverter.JsonToVector3 (evt.data.GetField ("position").ToString ());

		self.SetPlayer(name, room, pos);

		directionController.SetPlayer (self.gameObject, speed);

		// get other players to render them.
		socket.Emit ("get_players");

	}


}
