using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MaterialOption : MonoBehaviour
{
	#region Definitions
	#endregion Definitions

	#region Variables

	//--- Serialized ---
	[SerializeField]
	private TextMeshProUGUI m_name;
	[SerializeField]
	private Image m_image;

	//--- NonSerialized ---
	private MaterialInfoTemplate m_template;
	private System.Action<MaterialOption> m_clickCallback;

	#endregion Variables

	#region Accessors

	public MaterialInfoTemplate MaterialTemplate => m_template;
	public int MaterialIndex { get; private set; }

	#endregion Accessors

	#region Unity Messages
	#endregion Unity Messages

	#region Runtime Functions

	public void Init(int a_materialIndex, MaterialInfoTemplate a_template, System.Action<MaterialOption> a_clickCallback = null)
	{
		if (a_template == null)
			return;

		MaterialIndex = a_materialIndex;
		m_template = a_template;
		m_name.text = m_template.Name;
		m_image.sprite = a_template.PreviewImage;
		m_clickCallback = a_clickCallback;
	}

	#endregion Runtime Functions

	#region Callback Functions

	public void OnClicked()
	{
		m_clickCallback?.Invoke(this);
	}

	#endregion Callback Functions

	#region Editor Functions

#if UNITY_EDITOR

	private void OnValidate()
	{
		if (m_image == null)
			m_image = GetComponent<Image>();
	}

#endif

	#endregion Editor Functions
}