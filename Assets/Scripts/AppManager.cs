using System.Collections.Generic;
using UnityEngine;

public class AppManager : MonoBehaviour
{
	#region Definitions
	#endregion Definitions

	#region Variables

	//--- Serialized ---
	[SerializeField]
	private Transform m_meshRoot;
	[SerializeField]
	private List<MeshInfoTemplate> m_meshTempaltes;
	[SerializeField]
	private List<GameObject> m_lightGroups;
	[SerializeField]
	private MaterialsPanel m_materialsPanel;

	//--- NonSerialized ---
	private GameObject m_currentMeshInstance = null;
	private GameObject m_currentLightGroup = null;
	private int m_meshTemplateIndex = -1;
	private int m_lightGroupIndex = -1;

	#endregion Variables

	#region Accessors

	public List<MeshInfoTemplate> AllMeshTemplates => m_meshTempaltes;
	public List<GameObject> AllLightGroups => m_lightGroups;

	#endregion Accessors

	#region Unity Messages

	private void OnEnable()
	{
	}

	#endregion Unity Messages

	#region Runtime Functions

	private void LoadNextMesh()
	{
		if (m_meshTempaltes == null || m_meshTempaltes.Count <= 0)
			return;

		m_meshTemplateIndex = (m_meshTemplateIndex + 1) % m_meshTempaltes.Count;
		LoadMesh(m_meshTempaltes[m_meshTemplateIndex]);
	}

	private void LoadMesh(MeshInfoTemplate a_template)
	{
		ReleaseGameObject(m_currentMeshInstance);
		m_currentMeshInstance = UnityEditor.PrefabUtility.InstantiatePrefab(a_template.BasePrefab, m_meshRoot) as GameObject;
		m_currentMeshInstance.transform.localPosition = Vector3.zero;

		//Make sure we have some lighting
		if (m_currentLightGroup == null)
			LoadNextLightGroup();

		UpdateMaterials();
	}

	private void LoadNextLightGroup()
	{
		if (m_lightGroups == null || m_lightGroups.Count <= 0)
			return;

		m_lightGroupIndex = (m_lightGroupIndex + 1) % m_lightGroups.Count;
		LoadLightGroup(m_lightGroups[m_lightGroupIndex]);
	}

	private void LoadLightGroup(GameObject a_lightGroup)
	{
		ReleaseGameObject(m_currentLightGroup);
		m_currentLightGroup = UnityEditor.PrefabUtility.InstantiatePrefab(a_lightGroup) as GameObject;
		m_currentLightGroup.transform.localPosition = Vector3.zero;
	}

	private void ReleaseGameObject(GameObject a_gameObject)
	{
		if (a_gameObject != null)
			Destroy(a_gameObject);
	}

	private void UpdateMaterials()
	{
		if (m_currentMeshInstance == null || m_materialsPanel == null)
			return;

		m_materialsPanel.RefreshOptions(m_currentMeshInstance.GetComponent<MeshRenderer>());
	}

	#endregion Runtime Functions

	#region Callback Functions

	public void OnLoadMeshButtonClicked()
	{
		LoadNextMesh();
	}

	public void OnLoadLightButtonClicked()
	{
		LoadNextLightGroup();
	}

	#endregion Callback Functions

	#region Editor Functions

#if UNITY_EDITOR

#endif

	#endregion Editor Functions
}