using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;

public class ChatController : MonoBehaviour 
{
	public SocketIOComponent socket;

	public Transform messageArea;
	public InputField inputField;
	public Button sendButton;

	[HideInInspector]
	public int currentMessage = 0;


	public void ActivateChat()
	{
		sendButton.onClick.AddListener (SendButtonClick);
		socket.On ("chat", OnChatRecieve);
	}

	void SendButtonClick()
	{
		if (inputField.text != "") 
		{
			// send to the server via socket
			Dictionary<string, string> data = new Dictionary<string, string>();
			data ["message"] = inputField.text;

			socket.Emit("chat", new JSONObject(data));
		}
	}
		

	public void OnChatRecieve(SocketIOEvent evt)
	{
		currentMessage += 1;
		Debug.Log ("currentMessage: "+currentMessage);

		string name = JsonConverter.JsonToString (evt.data.GetField ("name").ToString (), "\"");
		string message = JsonConverter.JsonToString (evt.data.GetField ("message").ToString (), "\"");

		string sendingMessage = " " + name + ":   " + message;
		if (currentMessage > 0 && currentMessage < 9) 
		{
			Transform theMessageObj = messageArea.GetChild (currentMessage - 1);
			theMessageObj.GetComponent<Text> ().text = sendingMessage;
			theMessageObj.GetComponent<Text> ().enabled = true;
		} 
		else if (currentMessage >= 9) 
		{
			string temp;
			for (int i = 1; i < messageArea.childCount; i++) 
			{
				temp = messageArea.GetChild (i).GetComponent<Text> ().text;
				messageArea.GetChild (i - 1).GetComponent<Text> ().text = temp;
			}
				
			messageArea.GetChild(messageArea.childCount -1).GetComponent<Text>().text = sendingMessage;

		}

	}


}
