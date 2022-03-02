using System.Collections.Generic;
using UnityEngine;

public class MaterialPicker : MonoBehaviour
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
	private List<GameObject> m_optionItems = new List<GameObject>();
	private System.Action<MaterialOption> m_optionSelectedCallback;

	#endregion Variables

	#region Accessors
	#endregion Accessors

	#region Unity Messages

	private void OnEnable()
	{
		RefreshOptions(AppManager.Instance.AllMaterialTemplates);
	}

	private void OnDisable()
	{

	}

	#endregion Unity Messages

	#region Runtime Functions

	public void SetCallback(System.Action<MaterialOption> a_optionSelectedCallback)
	{
		m_optionSelectedCallback = a_optionSelectedCallback;
	}

	public void RefreshOptions(IList<MaterialInfoTemplate> a_templates)
	{
		ClearOptions();

		foreach (var materialTemplate in a_templates)
		{
			CreateOption(materialTemplate);
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

	private void CreateOption(MaterialInfoTemplate a_materialTemplate)
	{
		if (m_optionPrefab == null || a_materialTemplate == null)
			return;

		var option = UnityEditor.PrefabUtility.InstantiatePrefab(m_optionPrefab, m_optionsRoot) as GameObject;
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

	#region Editor Functions

#if UNITY_EDITOR

#endif

	#endregion Editor Functions
}