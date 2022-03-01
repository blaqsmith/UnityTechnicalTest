using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/MeshInfoTemplate")]
[Serializable]
public class MeshInfoTemplate : ScriptableObject
{
	#region Definitions
	#endregion Definitions

	#region Variables

	//--- Serialized ---
	[SerializeField]
	private string m_name;
	[SerializeField]
	private GameObject m_basePrefab;
	[SerializeField]
	private Sprite m_previewSprite;

	//--- NonSerialized ---

	#endregion Variables

	#region Accessors

	public string Name => m_name;
	public GameObject BasePrefab => m_basePrefab;
	public Sprite PreviewSprite => m_previewSprite;

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