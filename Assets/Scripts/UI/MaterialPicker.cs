using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the display of the available materials that you can replace with another selection
/// </summary>
public class MaterialPicker : MonoBehaviour
{
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private Transform m_optionsRoot;
	[SerializeField]
	private GameObject m_optionPrefab;

	//--- NonSerialized ---
	private List<GameObject> m_optionItems = new List<GameObject>();
	private System.Action<MaterialOption> m_optionSelectedCallback;

	#endregion Variables

	#region Unity Messages

	private void OnEnable()
	{
		RefreshOptions(AppManager.Instance.AllMaterialTemplates);
	}

	#endregion Unity Messages

	#region Runtime Functions

	public void SetCallback(System.Action<MaterialOption> a_optionSelectedCallback)
	{
		m_optionSelectedCallback = a_optionSelectedCallback;
	}

	public void RefreshOptions(IList<MaterialInfoTemplate> a_templates)
	{
		ClearSelectionOptions();

		foreach (var materialTemplate in a_templates)
		{
			CreateSelectionOption(materialTemplate);
		}
	}

	private void ClearSelectionOptions()
	{
		for (int i = m_optionItems.Count - 1; i >= 0; i--)
		{
			Destroy(m_optionItems[i]);
		}

		m_optionItems.Clear();
	}

	private void CreateSelectionOption(MaterialInfoTemplate a_materialTemplate)
	{
		if (m_optionPrefab == null || a_materialTemplate == null)
			return;

		var option = Instantiate(m_optionPrefab, m_optionsRoot);
		var optionItem = option.GetComponent<MaterialOption>();
		if (optionItem != null)
			optionItem.Init(-1, a_materialTemplate, OnMaterialPicked);
		m_optionItems.Add(option);
	}


	#endregion Runtime Functions

	#region Callback Functions

	private void OnMaterialPicked(MaterialOption a_option)
	{
		m_optionSelectedCallback?.Invoke(a_option);
		gameObject.SetActive(false);
	}

	#endregion Callback Functions
}