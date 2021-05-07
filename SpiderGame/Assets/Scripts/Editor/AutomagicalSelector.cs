using UnityEditor;
using UnityEngine;


public class AutomagicalSelector : ScriptableWizard // EditorWindow - Check out!
{
	public string gameObjectNameToFind = "Enter Exact Name of Object Here";
	public GameObject[] gameObjectsToSelect ;

	[MenuItem("Toolbox/Automagical Selector")]

	private static void CreateWindow()
	{
		ScriptableWizard.DisplayWizard<AutomagicalSelector>
		(
			title: "Automagically Select Item of Choice",
			createButtonName: "Select Now"
		);
	}

	private void OnEnable()
	{
		gameObjectsToSelect = new GameObject[1];
	}

	private void OnWizardCreate()
	{
		Selection.objects = gameObjectsToSelect;
	}

	private void OnWizardUpdate()
	{
		helpString = "Drag a GameObject here to be able to Select it Instantly!";
		helpString = "Set the number to at least 1";
	}
}