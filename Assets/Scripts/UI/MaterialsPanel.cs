using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the visual display of the materials that you can select from to use on the current mesh
/// </summary>
public class MaterialsPanel : MonoBehaviour
{
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private Transform m_optionsRoot;
	[SerializeField]
	private GameObject m_optionPrefab;

	//--- NonSerialized ---
	private MeshRenderer[] m_currentMeshRenderers;
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
		m_currentMeshRenderers = a_meshGameObject.GetComponentsInChildren<MeshRenderer>();

		ClearOptions();

		foreach (var materialSlot in a_meshTemplate.MaterialSlots)
		{
			CreateOption(materialSlot.MaterialIndex, materialSlot.DefaultMaterialTemplate);
		}
	}

	private void ClearOptions()
	{
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

		var option = Instantiate(m_optionPrefab, m_optionsRoot);
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
		foreach (var meshRenderer in m_currentMeshRenderers)
		{
			var materials = meshRenderer.materials;
			materials[m_replacedMaterialIndex] = a_optionSelected.MaterialTemplate.Material;
			meshRenderer.materials = materials;
		}
	}

	public void OnToggleOptionsOpen()
	{
		if (m_optionsRoot != null)
		{
			m_optionsRoot.gameObject.SetActive(!m_optionsRoot.gameObject.activeSelf);
		}
	}

	#endregion Callback Functions
}