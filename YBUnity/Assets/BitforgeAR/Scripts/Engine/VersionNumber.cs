using UnityEngine;
using System.Reflection;
using UnityEngine.UI;

/// <summary>
/// Automatically provides a version number to a project and displays it on a text
/// </summary>
/// <remarks>
/// Change the first two number to update the major and minor version number.
/// The following number are the build number (which is increased automatically
///  once a day, and the revision number which is increased every second). 
/// </remarks>

//GENERATED SCRIPT DON’t EDIT
[assembly:AssemblyVersion ("1.0.0.*")]
public class VersionNumber : MonoBehaviour
{
	/// <summary>
	/// Gets the version.
	/// </summary>
	/// <value>The version.</value>
	public string Version {
		get {
			if (version == null) {
				version = Assembly.GetExecutingAssembly ().GetName ().Version.ToString ();
			}
			return version;
		}
	}
	string version;
	
	
	void Start ()
	{
		var text = GetComponent<Text>();
		if(text != null)
		{
			text.text = "v"+Version;
		}
	}
}
