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
	private CurrentModelMaterialsPanel m_currentMaterialsPanel;
	[SerializeField]
	private List<MaterialInfoTemplate> m_materialTemplates;
	[SerializeField]
	private Canvas m_uiCanvas;

	[Header("UI Prefabs")]
	[SerializeField]
	private GameObject m_materialPickerPrefab;

	//--- NonSerialized ---
	private static AppManager m_instance;

	private GameObject m_currentMeshInstance = null;
	private GameObject m_currentLightGroup = null;
	private int m_meshTemplateIndex = -1;
	private int m_lightGroupIndex = -1;
	private MaterialPicker m_materialPicker;

	#endregion Variables

	#region Accessors

	public static AppManager Instance => m_instance;

	public IList<MeshInfoTemplate> AllMeshTemplates => m_meshTempaltes.AsReadOnly();
	public IList<GameObject> AllLightGroups => m_lightGroups.AsReadOnly();
	public IList<MaterialInfoTemplate> AllMaterialTemplates => m_materialTemplates.AsReadOnly();

	#endregion Accessors

	#region Unity Messages

	private void OnEnable()
	{
		if (m_instance == null)
		{
			m_instance = this;
		}
		else
		{
			Debug.LogError("Multiple AppManagers were created! Remove duplicates to prevent data errors.");
		}
	}

	private void OnDisable()
	{
		if (m_instance == this)
		{
			m_instance = null;
		}
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

		UpdateCurrentMaterials();
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

	private void UpdateCurrentMaterials()
	{
		if (m_currentMeshInstance == null || m_currentMaterialsPanel == null)
			return;

		m_currentMaterialsPanel.RefreshOptions(m_currentMeshInstance, m_meshTempaltes[m_meshTemplateIndex]);
	}

	public void OpenMaterialPicker(System.Action<MaterialOption> a_optionSelectedCallback)
	{
		if (m_materialPicker == null)
		{
			if (m_materialPickerPrefab == null)
			{
				Debug.LogError("Could not create MaterialPicker UI due to invalid prefab definition in AppManager");
			}

			var ui = UnityEditor.PrefabUtility.InstantiatePrefab(m_materialPickerPrefab, m_uiCanvas.transform) as GameObject;
			if (ui == null)
			{
				Debug.LogError("Failed to isntantiate MaterialPicker prefab gameobject");
			}

			m_materialPicker = ui.GetComponent<MaterialPicker>();
		}

		m_materialPicker.SetCallback(a_optionSelectedCallback);
		m_materialPicker.gameObject.SetActive(true);
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