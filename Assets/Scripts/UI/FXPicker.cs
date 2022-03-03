using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FXPicker : MonoBehaviour
{
	#region Definitions
	#endregion Definitions

	#region Variables

	//--- Serialized ---
	[SerializeField]
	private GameObject m_fxOptionsRoot;

	[SerializeField]
	private Volume m_volume;

	[Header("Intensity roots")]
	[SerializeField]
	private GameObject m_bloomIntensityRoot;

	[SerializeField]
	private GameObject m_chromaticIntensityRoot;

	[SerializeField]
	private GameObject m_lensDistortionIntensityRoot;

	[SerializeField]
	private GameObject m_vignetteIntensityRoot;

	//--- NonSerialized ---

	private Bloom m_bloom;
	private ChromaticAberration m_chromatic;
	private LensDistortion m_lensDistortion;
	private Vignette m_vignette;

	#endregion Variables

	#region Accessors
	#endregion Accessors

	#region Unity Messages

	private void Awake()
	{
		if (m_volume == null)
			return;

		m_volume.profile.TryGet<Bloom>(out m_bloom);
		m_volume.profile.TryGet<ChromaticAberration>(out m_chromatic);
		m_volume.profile.TryGet<LensDistortion>(out m_lensDistortion);
		m_volume.profile.TryGet<Vignette>(out m_vignette);
	}

	#endregion Unity Messages

	#region Runtime Functions
	#endregion Runtime Functions

	#region Callback Functions

	public void OnFxButtonClicked()
	{
		if (m_fxOptionsRoot != null)
		{
			m_fxOptionsRoot.SetActive(!m_fxOptionsRoot.activeSelf);
		}
	}

	public void OnBloomToggled(bool a_toggleValue)
	{
		m_bloomIntensityRoot.SetActive(a_toggleValue);
		m_bloom.active = a_toggleValue;
	}

	public void OnChromaticAbberationToggled(bool a_toggleValue)
	{
		m_chromaticIntensityRoot.SetActive(a_toggleValue);
		m_chromatic.active = a_toggleValue;
	}

	public void OnLensDistortionToggled(bool a_toggleValue)
	{
		m_lensDistortionIntensityRoot.SetActive(a_toggleValue);
		m_lensDistortion.active = a_toggleValue;
	}

	public void OnVignetteToggled(bool a_toggleValue)
	{
		m_vignetteIntensityRoot.SetActive(a_toggleValue);
		m_vignette.active = a_toggleValue;
	}

	public void OnBloomIntensityChanged(float a_value)
	{
		if (m_bloom.active)
		{
			m_bloom.intensity.overrideState = true;
			m_bloom.intensity.value = Mathf.Lerp(0, 20, a_value);
		}
	}

	public void OnChromaticAbberationIntensityChanged(float a_value)
	{
		if (m_chromatic.active)
		{
			m_chromatic.intensity.overrideState = true;
			m_chromatic.intensity.value = a_value;
		}
	}

	public void OnLensDistortionIntensityChanged(float a_value)
	{
		if (m_lensDistortion.active)
		{
			m_lensDistortion.intensity.overrideState = true;
			m_lensDistortion.intensity.value = a_value;
		}
	}

	public void OnVignetteIntensityChanged(float a_value)
	{
		if (m_vignette.active)
		{
			m_vignette.intensity.overrideState = true;
			m_vignette.intensity.value = a_value;
		}
	}


	#endregion Callback Functions

	#region Editor Functions

#if UNITY_EDITOR

#endif

	#endregion Editor Functions
}