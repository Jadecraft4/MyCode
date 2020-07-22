using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
public class MenuScript : MonoBehaviour
{
    public GameObject LoadingScreen;
    public GameObject MenuScreen;
    public Slider LoadingBar;
    public Text ProgressText;
    public AudioMixer audioMixer;
    public Dropdown ResolutionDropdown;
    Resolution[] resolutions;
    private void Start()
    {
        resolutions = Screen.resolutions;
        ResolutionDropdown.ClearOptions();
        int CurrentResolutionIndex = 0;
        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                CurrentResolutionIndex = i;
            }
        }
        ResolutionDropdown.AddOptions(options);
        ResolutionDropdown.value = CurrentResolutionIndex;
        ResolutionDropdown.RefreshShownValue();
    }
    public void SetResolution(int ResolutionIndex)
    {
        Resolution resolution = resolutions[ResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void StartGame()
    {
        StartCoroutine(LoadGame());
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }
    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }
    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
    IEnumerator LoadGame()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("Level01");
        MenuScreen.SetActive(false);
        LoadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            float Progress = Mathf.Clamp01(operation.progress / 0.9f);
            LoadingBar.value = Progress;
            ProgressText.text = Progress * 100f + "%";
            yield return null;
        }
    }
}
