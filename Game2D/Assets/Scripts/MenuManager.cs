using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject settingsPanel;

    [SerializeField] private Button newGameBtn;
    [SerializeField] private Button continueBtn;
    [SerializeField] private Button settingsBtn;
    [SerializeField] private Button quitBtn;
    [SerializeField] private Button languageBtn;

    [SerializeField] private Button backBtn;
    [SerializeField] private Slider music;
    [SerializeField] private Slider sfx;

    [SerializeField] private string firstLevelScene = "GameScene";

    private InputAction cancelAction;
    private const string SAVE_LEVEL_KEY = "SavedLevel";
    private bool isLanguageTurkish = true;
    private string btnBuildText = "Ýnþaa Et";
    private string btnBuildHASH = "BTN_BUILD_HASH";

    private void Awake()
    {
        cancelAction = new InputAction("Cancel", binding: "<Keyboard>/escape");
        cancelAction.AddBinding("<Gamepad>/buttonEast");
    }

    private void OnEnable()
    {
        cancelAction.Enable();
        cancelAction.performed += OnCancelPerformed;
    }

    private void OnCancelPerformed(InputAction.CallbackContext obj)
    {
        cancelAction.performed -= OnCancelPerformed;
        cancelAction.Disable();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainPanel.SetActive(true);
        settingsPanel.SetActive(false);

        bool hasSave = PlayerPrefs.HasKey(SAVE_LEVEL_KEY);
        continueBtn.interactable = hasSave;

        newGameBtn.onClick.AddListener(OnNewGameClicked);
        continueBtn.onClick.AddListener(OnContinueClicked);
        settingsBtn.onClick.AddListener(OnSettingsClicked);
        quitBtn.onClick.AddListener(OnQuitClicked);
        backBtn.onClick.AddListener(OnBackClicked);
        languageBtn.onClick.AddListener(OnLanguageClicked);


        music.value = PlayerPrefs.GetFloat("MusicVol", 0.5f);
        music.value = PlayerPrefs.GetFloat("SFXVol", 0.5f);
        PlayerPrefs.SetString(btnBuildHASH, btnBuildText);

        music.onValueChanged.AddListener(OnMusicSliderChanged);
        sfx.onValueChanged.AddListener(OnSFXSliderChanged);

        SelectUIElement(newGameBtn.gameObject);
    }

    private void OnNewGameClicked()
    {
        PlayerPrefs.DeleteKey("SavedTownData");
        PlayerPrefs.SetString(SAVE_LEVEL_KEY, firstLevelScene);
        PlayerPrefs.Save();
        SceneManager.LoadScene(firstLevelScene);
    }

    private void OnContinueClicked()
    {
        if(PlayerPrefs.HasKey(SAVE_LEVEL_KEY))
        {
            string sceneToLoad = PlayerPrefs.GetString(SAVE_LEVEL_KEY);
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    private void OnSettingsClicked()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
        SelectUIElement(music.gameObject);
    }

    private void OnBackClicked()
    {
        settingsPanel.SetActive(false);
        mainPanel.SetActive(true);
        SelectUIElement(newGameBtn.gameObject);
    }

    private void OnQuitClicked()
    {
        Application.Quit();
    }

    private void OnLanguageClicked()
    {
        if (isLanguageTurkish)
        {
            languageBtn.GetComponentInChildren<TMP_Text>().text = "EN";
            newGameBtn.GetComponentInChildren<TMP_Text>().text = "New Game";
            continueBtn.GetComponentInChildren<TMP_Text>().text = "Continue";
            settingsBtn.GetComponentInChildren<TMP_Text>().text = "Settings";
            quitBtn.GetComponentInChildren<TMP_Text>().text = "Quit";
            backBtn.GetComponentInChildren<TMP_Text>().text = "Back";
            music.GetComponentInChildren<TMP_Text>().text = "Music Level";
            sfx.GetComponentInChildren<TMP_Text>().text = "SFX Level";
            PlayerPrefs.SetString(btnBuildHASH, "Build");
            isLanguageTurkish = false;
        }
        else
        {
            languageBtn.GetComponentInChildren<TMP_Text>().text = "TR";
            newGameBtn.GetComponentInChildren<TMP_Text>().text = "Yeni Oyun";
            continueBtn.GetComponentInChildren<TMP_Text>().text = "Devam Et";
            settingsBtn.GetComponentInChildren<TMP_Text>().text = "Ayarlar";
            quitBtn.GetComponentInChildren<TMP_Text>().text = "Çýkýþ";
            backBtn.GetComponentInChildren<TMP_Text>().text = "Geri";
            music.GetComponentInChildren<TMP_Text>().text = "Müzik Seviyesi";
            sfx.GetComponentInChildren<TMP_Text>().text = "SFX Seviyesi";
            PlayerPrefs.SetString(btnBuildHASH, btnBuildText);
            isLanguageTurkish = true;
        }
        
    }

    private void OnMusicSliderChanged(float value)
    {
        if (AudioManagerScript.Instance != null)
        {
            AudioManagerScript.Instance.SetMusicVolume(value);
        }
    }

    private void OnSFXSliderChanged(float value)
    {
        if (AudioManagerScript.Instance != null)
        {
            AudioManagerScript.Instance.SetSFXVolume(value);
        }
    }

    // Bir nesne/elemente odaklama
    private void SelectUIElement(GameObject element)
    {
        if (EventSystem.current != null && element != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(element);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
