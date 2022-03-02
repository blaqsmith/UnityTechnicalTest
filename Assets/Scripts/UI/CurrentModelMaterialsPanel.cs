using System.Collections.Generic;
using UnityEngine;

public class CurrentModelMaterialsPanel : MonoBehaviour
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
	private List<MaterialOption> m_optionItems = new List<MaterialOption>();
	private int m_replacedMaterialIndex;

	#endregion Variables

	#region Accessors
	#endregion Accessors

	#region Unity Messages
	#endregion Unity Messages

	#region Runtime Functions

	public void RefreshOptions(GameObject a_meshGameObject, MeshInfoTemplate a_meshTemplate)
	{
		m_currentMeshRenderer = a_meshGameObject.GetComponent<MeshRenderer>();

		ClearOptions();

		foreach (var materialSlot in a_meshTemplate.MaterialSlots)
		{
			CreateOption(materialSlot.MaterialIndex, materialSlot.DefaultMaterialTemplate);
		}
	}

	private void ClearOptions()
	{
		//TODO: do we need to restore default materials before clearing?

		for (int i = m_optionItems.Count - 1; i >= 0; i--)
		{
			Destroy(m_optionItems[i].gameObject);
		}

		m_optionItems.Clear();
	}

	private void CreateOption(int a_materialIndex, MaterialInfoTemplate a_materialTemplate)
	{
		if (m_optionPrefab == null || a_materialTemplate == null)
			return;

		var option = UnityEditor.PrefabUtility.InstantiatePrefab(m_optionPrefab, m_optionsRoot) as GameObject;
		var optionItem = option.GetComponent<MaterialOption>();
		if (optionItem != null)
		{
			optionItem.Init(a_materialIndex, a_materialTemplate, OnMaterialClicked);
			m_optionItems.Add(optionItem);
		}
		else
		{
			Debug.LogError($"Failed to create option for {a_materialTemplate.Name} due to lack of MaterialOption script on the generated gameObject");
			Destroy(optionItem);
		}
	}

	#endregion Runtime Functions

	#region Callback Functions

	private void OnMaterialClicked(MaterialOption a_optionSelected)
	{
		m_replacedMaterialIndex = a_optionSelected.MaterialIndex;
		AppManager.Instance.OpenMaterialPicker(OnNewMaterialClicked);
	}

	private void OnNewMaterialClicked(MaterialOption a_optionSelected)
	{
		Debug.Log("new matieral selected: " + a_optionSelected.MaterialTemplate.Name);
		var materials = m_currentMeshRenderer.materials;
		materials[m_replacedMaterialIndex] = a_optionSelected.MaterialTemplate.Material;
		m_currentMeshRenderer.materials = materials;
	}

	#endregion Callback Functions

	#region Editor Functions

#if UNITY_EDITOR

#endif

	#endregion Editor Functions
}