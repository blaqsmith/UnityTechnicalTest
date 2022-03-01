using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MaterialOptionItem : MonoBehaviour
{
	#region Definitions
	#endregion Definitions

	#region Variables

	//--- Serialized ---
	[SerializeField]
	private Image m_image;

	//--- NonSerialized ---

	#endregion Variables

	#region Accessors
	#endregion Accessors

	#region Unity Messages
	#endregion Unity Messages

	#region Runtime Functions

	public void Init(Material a_material)
	{
		if (m_image == null)
			return;

		m_image.material = a_material;
	}

	#endregion Runtime Functions

	#region Callback Functions
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