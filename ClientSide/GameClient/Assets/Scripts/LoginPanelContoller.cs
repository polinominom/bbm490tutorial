using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanelContoller : MonoBehaviour 
{

	public Button playBtn;
	public InputField inputField;
	public Dropdown roomDropdown;

	private Dictionary<string, int> rooms;

	void Start()
	{
		rooms = new Dictionary<string, int> ();
		rooms ["Room 1"] = 0;

		UpdateDropdown ();
	}

	public void CollectInformation(string room)
	{
		if (rooms.ContainsKey (room)) 
		{
			Debug.Log ("rooms [ "+room+"]: "+rooms [room] );
			//check for max room limit
			if (rooms [room] < 2) 
			{
				rooms [room] += 1;
			}
					
		}

		UpdateDropdown ();
	}


	void UpdateDropdown()
	{
		
		roomDropdown.ClearOptions ();
		List<Dropdown.OptionData> newOptions = new List<Dropdown.OptionData> ();

		int counter = 0;
		string lastKey = "";
		foreach (KeyValuePair<string,int> keyValue in rooms) 
		{
			string key = keyValue.Key;
			int value = keyValue.Value;

			Dropdown.OptionData item = new Dropdown.OptionData();
			item.text =  ""+key+" ("+value+" people online).";

			newOptions.Add (item);



			lastKey = key;
			counter += 1;
		}
			
		if(rooms[lastKey] == 2)
		{
			Debug.Log ("HEREEE!!");
			string newRoomName = "Room " + (counter+1);
			rooms [newRoomName] = 0;
			Dropdown.OptionData item = new Dropdown.OptionData();
			item.text =  ""+newRoomName+" ("+rooms [newRoomName]+" people online).";
			newOptions.Add (item);
		
		}

		roomDropdown.AddOptions (newOptions);
		roomDropdown.value = counter;
			
	}

	public string GetSelectedRoom()
	{
		string result = "";

		int wantedIndex = roomDropdown.value;

		int counter = 0;
		foreach (KeyValuePair<string,int> keyValue in rooms) 
		{
			if (wantedIndex == counter) 
			{
				result = keyValue.Key;
				break;
			}
			counter += 1;
		}

		return result;
	}

}
