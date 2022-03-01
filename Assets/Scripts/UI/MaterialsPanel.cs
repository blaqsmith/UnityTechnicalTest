using System.Collections.Generic;
using UnityEngine;

public class MaterialsPanel : MonoBehaviour
{
	#region Definitions
	#endregion Definitions

	#region Variables

	//--- Serialized ---
	[SerializeField]
	private Transform m_optionsRoot;
	[SerializeField]
	private GameObject m_optionPrefab;

	//--- NonSerialized ---
	private MeshRenderer m_currentMeshRenderer;
	private List<GameObject> m_optionItems = new List<GameObject>();

	#endregion Variables

	#region Accessors
	#endregion Accessors

	#region Unity Messages
	#endregion Unity Messages

	#region Runtime Functions

	public void RefreshOptions(MeshRenderer a_meshRenderer)
	{
		ClearOptions();

		m_currentMeshRenderer = a_meshRenderer;
		foreach (var material in a_meshRenderer.sharedMaterials)
		{
			CreateOption(material);
		}
	}

	private void ClearOptions()
	{
		//TODO: do we need to restore default materials before clearing?

		for (int i = m_optionItems.Count - 1; i >= 0; i--)
		{
			Destroy(m_optionItems[i]);
		}

		m_optionItems.Clear();
	}

	private void CreateOption(Material a_material)
	{
		if (m_optionPrefab == null)
			return;

		var option = UnityEditor.PrefabUtility.InstantiatePrefab(m_optionPrefab, m_optionsRoot) as GameObject;
		var optionItem = option.GetComponent<MaterialOptionItem>();
		if (optionItem != null)
			optionItem.Init(a_material);
		m_optionItems.Add(option);
	}

	#endregion Runtime Functions

	#region Callback Functions
	#endregion Callback Functions

	#region Editor Functions

#if UNITY_EDITOR

#endif

	#endregion Editor Functions
}