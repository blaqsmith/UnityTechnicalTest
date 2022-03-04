using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides housing for the main data collections, and general use refrences for the rest of the application
/// </summary>
public class AppManager : MonoBehaviour
{
	#region Variables

	//--- Serialized ---

	[Header("Refrences")]
	[SerializeField, Tooltip("The visualizer responsible for displaying meshes")]
	private MeshVisualizer m_meshVisualizer;
	[SerializeField, Tooltip("Main UI canvas")]
	private Canvas m_uiCanvas;
	[SerializeField, Tooltip("Prefab to use to instantial this material picker")]
	private GameObject m_materialPickerPrefab;

	[Header("App Data")]
	[SerializeField, Tooltip("A material that will be available to be applied to meshes")]
	private List<MaterialInfoTemplate> m_materialTemplates;
	[SerializeField, Tooltip("A meshe that will be cyceled through when you click the Next Mesh button")]
	private List<MeshInfoTemplate> m_meshTemplates;
	[SerializeField, Tooltip("A light that will be cyceled through when you click the Next LIght button")]
	private List<GameObject> m_lightGroups;


	//--- NonSerialized ---
	private static AppManager m_instance;   //For cheapo singleton behaviour

	private GameObject m_currentLightGroup = null;
	private int m_meshTemplateIndex = -1;
	private int m_lightGroupIndex = -1;
	private MaterialPicker m_materialPicker;

	#endregion Variables

	#region Accessors

	public static AppManager Instance => m_instance;
	public IList<MeshInfoTemplate> AllMeshTemplates => m_meshTemplates.AsReadOnly();
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
		m_currentLightGroup = Instantiate(a_lightGroup);
		m_currentLightGroup.transform.localPosition = Vector3.zero;
	}

	private void ReleaseGameObject(GameObject a_gameObject)
	{
		if (a_gameObject != null)
			Destroy(a_gameObject);
	}

	//While not the best choice for this demonstration, this is example of how a central location can be used
	//to maintain refresnces for prefabs that need to be instantiated. Any function that needs to display the prefab
	//can call this function rather than maintaining its own reference. If the prefab ever changes, it can just be 
	//updated in this one location, without having to track down all the references throughout the app.
	public void OpenMaterialPicker(System.Action<MaterialOption> a_optionSelectedCallback)
	{
		if (m_materialPicker == null)
		{
			if (m_materialPickerPrefab == null)
			{
				Debug.LogError("Could not create MaterialPicker UI due to invalid prefab definition in AppManager");
			}

			var ui = Instantiate(m_materialPickerPrefab, m_uiCanvas.transform);
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

	public void OnNextMeshButtonClicked()
	{
		if (m_meshTemplates == null || m_meshTemplates.Count <= 0)
			return;

		m_meshTemplateIndex = (m_meshTemplateIndex + 1) % m_meshTemplates.Count;
		m_meshVisualizer.LoadMesh(m_meshTemplates[m_meshTemplateIndex]);

		//Make sure we have some lighting
		if (m_currentLightGroup == null)
			LoadNextLightGroup();
	}

	public void OnNextLightButtonClicked()
	{
		LoadNextLightGroup();
	}

	#endregion Callback Functionss
}