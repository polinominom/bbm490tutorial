using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class JsonConverter : MonoBehaviour 
{
	public static string JsonToString(string target, string s)
	{
		string[] result = Regex.Split (target, s);

		return result[1];
	}

	public static Vector3 JsonToVector3(string target)
	{
		Vector3 resultVector = new Vector3 ();

		string[] phaseOne = Regex.Split (target, "\"");


		string sVector = phaseOne[1].Substring(1, phaseOne[1].Length-2);
		string[] resultString = Regex.Split (sVector, ",");

		resultVector.x = float.Parse (resultString [0]);
		resultVector.y = float.Parse (resultString [1]);
		resultVector.z = float.Parse (resultString [2]);

		return resultVector;

	}


}
