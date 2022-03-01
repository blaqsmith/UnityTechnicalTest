using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ViewController : MonoBehaviour
{
	#region Definitions
	#endregion Definitions

	#region Variables

	//--- Serialized ---
	[SerializeField]
	private float m_rotationSpeed = 5f;
	[SerializeField]
	private float m_moveSpeed = 5f;
	[SerializeField]
	private Transform m_cameraRoot;
	[SerializeField]
	private Transform m_camera;
	[SerializeField]
	private float m_cameraMinAngle = -45;
	[SerializeField]
	private float m_cameraMaxAngle = 45;
	[SerializeField]
	private float m_cameraMinDist = 4;
	[SerializeField]
	private float m_cameraMaxDist = 16;

	//--- NonSerialized ---
	private InputActions m_input;
	private InputAction m_moveInput;
	private bool m_moveEnabled;

	#endregion Variables

	#region Accessors
	#endregion Accessors

	#region Unity Messages

	private void Awake()
	{
		m_input = new InputActions();
	}

	private void OnEnable()
	{
		m_moveInput = m_input.Movement.Move;
		m_moveInput.Enable();

		m_input.Movement.Fire.started += OnClick;
		m_input.Movement.Fire.canceled += OnClick;
		m_input.Movement.Fire.Enable();
	}

	private void OnDisable()
	{
		m_input.Movement.Fire.performed -= OnClick;
		m_input.Movement.Fire.canceled -= OnClick;
		m_input.Movement.Fire.Disable();
	}

	private void FixedUpdate()
	{
		//Only move when the mouse button is down and the point 
		if (m_moveEnabled)
		{
			var delta = m_moveInput.ReadValue<Vector2>();
			transform.Rotate(Vector3.up, -delta.x * m_rotationSpeed);

			delta.x = 0;
			transform.Translate(delta * m_moveSpeed);
		}
	}

	#endregion Unity Messages

	#region Runtime Functions
	#endregion Runtime Functions

	#region Callback Functions

	private void OnClick(InputAction.CallbackContext a_context)
	{
		//Enable movement on click/touch start, but not if it's over a UI element
		if (a_context.started && !EventSystem.current.IsPointerOverGameObject())
		{
			m_moveEnabled = true;
		}

		if (a_context.canceled)
		{
			m_moveEnabled = false;
		}
	}

	public void OnAngleValueChanged(float a_value)
	{
		var angle = new Vector3(Mathf.Lerp(m_cameraMinAngle, m_cameraMaxAngle, a_value), 0f, 0f);
		m_cameraRoot.rotation = Quaternion.Euler(angle);
	}

	public void OnZoomValueChanged(float a_value)
	{
		var pos = new Vector3(0f, 0f, Mathf.Lerp(m_cameraMinDist, m_cameraMaxDist, a_value));
		m_camera.localPosition = pos;
	}

	#endregion Callback Functions

	#region Editor Functions

#if UNITY_EDITOR
#endif

	#endregion Editor Functions
}