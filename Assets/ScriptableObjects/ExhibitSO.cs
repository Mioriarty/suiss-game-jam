using UnityEngine;

[CreateAssetMenu(fileName = "Exhibit", menuName = "Scriptable Objects/Exhibit")]
public class Exhibit : ScriptableObject
{
	[Header("Core data")]
	[SerializeField] private Sprite image;
	[SerializeField] private string exhibitName = "New Exhibit";
	[TextArea] [SerializeField] private string interest = "";

	// Public read-only accessors
	public Sprite Image => image;
	public string ExhibitName => exhibitName;
	public string Interest => interest;

	// Simple validation to ensure name is set when changed in inspector
	private void OnValidate()
	{
        if (string.IsNullOrWhiteSpace(exhibitName))
            exhibitName = "New Exhibit";
	}

	[ContextMenu("Clear Image")]
	private void ClearImage()
	{
		image = null;
	}
}
