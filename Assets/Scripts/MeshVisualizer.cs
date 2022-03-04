using UnityEngine;

/// <summary>
/// Responsible for handling the display of the mesh itself
/// </summary>
public class MeshVisualizer : MonoBehaviour
{
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private MeshViewController m_meshViewController;
	[SerializeField]
	private MaterialsPanel m_currentMaterialsPanel;

	//--- NonSerialized ---
	private MeshInfoTemplate m_meshTemplate;

	#endregion Variables

	#region Accessors

	public GameObject CurrentMeshInstance { get; private set; }

	#endregion Accessors

	#region Runtime Functions

	public void LoadMesh(MeshInfoTemplate a_meshTemplate)
	{
		if (a_meshTemplate == null)
		{
			Debug.LogError("Attempted to create a mesh from a null meshTemplate. Aborting creation.");
			return;
		}

		m_meshTemplate = a_meshTemplate;

		//Create mesh
		ReleaseGameObject(CurrentMeshInstance);
		CurrentMeshInstance = Instantiate(a_meshTemplate.BasePrefab, transform);
		CurrentMeshInstance.transform.localPosition = Vector3.zero;

		//Set default values
		transform.position = a_meshTemplate.DefaultPosition;
		transform.rotation = Quaternion.Euler(a_meshTemplate.DefaultRotation);
		m_meshViewController.DirectSetAngle01(a_meshTemplate.DefaultCameraAngle);
		m_meshViewController.DirectSetZoom01(a_meshTemplate.DefaultCameraZoom);

		UpdateCurrentMaterials();
	}

	private void UpdateCurrentMaterials()
	{
		if (CurrentMeshInstance == null || m_currentMaterialsPanel == null)
		{
			return;
		}

		m_currentMaterialsPanel.RefreshOptions(CurrentMeshInstance, m_meshTemplate);
	}

	//While this is fairly simple in its current form, I prefer to put "cleanup" type code in its own function to 
	//allow for easy expansion if/when a class becomes more complicated, while still maintaining a readable name
	private void ReleaseGameObject(GameObject a_gameObject)
	{
		if (a_gameObject != null)
			Destroy(a_gameObject);
	}

	#endregion Runtime Functions
}