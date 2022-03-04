using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/MaterialInfoTemplate")]
[Serializable]
public class MaterialInfoTemplate : ScriptableObject
{
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private string m_name;
	[SerializeField]
	private Material m_material;
	[SerializeField]
	public Sprite m_previewImage;

	#endregion Variables

	#region Accessors

	public string Name => m_name;
	public Material Material => m_material;
	public Sprite PreviewImage => m_previewImage;

	#endregion Accessors
}