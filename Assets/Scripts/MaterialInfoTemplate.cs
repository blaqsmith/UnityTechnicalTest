using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/MaterialInfoTemplate")]
[Serializable]
public class MaterialInfoTemplate : ScriptableObject
{
	#region Definitions
	#endregion Definitions

	#region Variables

	//--- Serialized ---
	[SerializeField]
	private string m_name;
	[SerializeField]
	private Material m_material;
	[SerializeField]
	public Sprite m_previewImage;


	//--- NonSerialized ---

	#endregion Variables

	#region Accessors

	public string Name => m_name;
	public Material Material => m_material;
	public Sprite PreviewImage => m_previewImage;

	#endregion Accessors

	#region Unity Messages
	#endregion Unity Messages

	#region Runtime Functions
	#endregion Runtime Functions

	#region Callback Functions
	#endregion Callback Functions

	#region Editor Functions

#if UNITY_EDITOR

#endif

	#endregion Editor Functions
}