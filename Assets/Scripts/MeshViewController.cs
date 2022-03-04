using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Manages the movement of the mesh and the camera viewing it
/// </summary>
public class MeshViewController : MonoBehaviour
{
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

	[Header("Control references")]
	[SerializeField]
	private Slider m_angleSlider;
	[SerializeField]
	private Slider m_zoomSlider;

	//--- NonSerialized ---
	private InputActions m_input;
	private InputAction m_moveInput;
	private bool m_moveEnabled;
	private float m_speedMultiplier = 1f;   //Magic number multiplier used to tamp device movement speed

	#endregion Variables

	#region Unity Messages

	private void Awake()
	{
		m_input = new InputActions();
#if UNITY_ANDROID && !UNITY_EDITOR
		m_speedMultiplier = .25f;
#endif
	}

	private void OnEnable()
	{
		m_moveInput = m_input.Movement.Move;
		m_moveInput.Enable();

		//Register click events
		m_input.Movement.Fire.started += OnClick;
		m_input.Movement.Fire.canceled += OnClick;
		m_input.Movement.Fire.Enable();
	}

	private void OnDisable()
	{
		//Unregister click events
		m_input.Movement.Fire.performed -= OnClick;
		m_input.Movement.Fire.canceled -= OnClick;
		m_input.Movement.Fire.Disable();
	}

	private void FixedUpdate()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		//This isn't checked on OnClick for devices, so do so here
		//Enable movement on click/touch start, but not if it's over a UI element
		if (EventSystem.current.IsPointerOverGameObject())
			return;
#endif

#if !UNITY_ANDROID || UNITY_EDITOR
		//On PC, only move when the mouse button is down and the point 
		if (m_moveEnabled)
#endif
		{
			var delta = m_moveInput.ReadValue<Vector2>();
			if (delta.x != 0)
			{
				transform.Rotate(Vector3.up, -delta.x * m_rotationSpeed * m_speedMultiplier);
			}

			if (delta.y != 0)
			{
				delta.x = 0;
				transform.Translate(delta * m_moveSpeed * m_speedMultiplier);
			}
		}
	}

	#endregion Unity Messages

	#region Runtime Functions

	public void DirectSetAngle01(float a_value)
	{
		m_angleSlider.value = Mathf.Clamp01(a_value);
	}

	public void DirectSetZoom01(float a_value)
	{
		m_zoomSlider.value = Mathf.Clamp01(a_value);
	}


	#endregion Runtime Functions

	#region Callback Functions

	private void OnClick(InputAction.CallbackContext a_context)
	{
		if (a_context.started && !EventSystem.current.IsPointerOverGameObject())
		{
			m_moveEnabled = true;
		}

		if (m_moveEnabled && a_context.canceled)
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
}