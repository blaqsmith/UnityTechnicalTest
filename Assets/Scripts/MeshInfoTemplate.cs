using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/MeshInfoTemplate")]
[Serializable]
public class MeshInfoTemplate : ScriptableObject
{
	#region Definitions

	/// <summary>
	/// Used to specify materials that we are allowed to change on this model
	/// </summary>
	[Serializable]
	public class MaterialSlot
	{
		public int MaterialIndex;
		public MaterialInfoTemplate DefaultMaterialTemplate;
	}

	#endregion Definitions

	#region Variables

	//--- Serialized ---
	[SerializeField, Tooltip("Display name for this mesh")]
	private string m_name;
	[SerializeField, Tooltip("Prefab to use to instantiate this mesh in our view")]
	private GameObject m_basePrefab;
	[SerializeField, Tooltip("Image to use when displaying the mesh collection")]
	private Sprite m_previewSprite;
	[SerializeField, Tooltip("When this mesh it loaded, the mesh visualizer position will be set to this value")]
	private Vector3 m_defaultPosition;
	[SerializeField, Tooltip("When this mesh it loaded, the mesh visualizer rotation will be set to this value")]
	private Vector3 m_defaultRotation;
	[SerializeField, Tooltip("When this mesh it loaded, the camera angle will be set to this value")]
	private float m_defaultCameraAngle = 0.5f;
	[SerializeField, Tooltip("When this mesh it loaded, the camera zoom will be set to this value")]
	private float m_defaultCameraZoom = 0.33f;
	[SerializeField, Tooltip("List of the materials that are eligable to be swapped")]
	private List<MaterialSlot> m_materialSlots;

	#endregion Variables

	#region Accessors

	public string Name => m_name;
	public GameObject BasePrefab => m_basePrefab;
	public Sprite PreviewSprite => m_previewSprite;
	public Vector3 DefaultPosition => m_defaultPosition;
	public Vector3 DefaultRotation => m_defaultRotation;
	public float DefaultCameraAngle => m_defaultCameraAngle;
	public float DefaultCameraZoom => m_defaultCameraZoom;
	public IList<MaterialSlot> MaterialSlots => m_materialSlots.AsReadOnly();

	#endregion Accessors
}