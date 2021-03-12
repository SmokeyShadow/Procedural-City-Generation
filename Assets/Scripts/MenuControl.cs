using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuControl : MonoBehaviour
{
    #region SERIALIZED FIELDS
    [SerializeField]
    private GameObject howtoPlay;

    [SerializeField]
    private GameObject startPanel;

    [SerializeField]
    private Button startBtn;

    [SerializeField]
    private Button howtoBtn;

    [SerializeField]
    private Button quitBtn;

    [SerializeField]
    private Button backBtn;

    [SerializeField]
    private AudioSource sfxSource;
    #endregion

    #region MONO BEHAVIOURS
    void Start()
    {
        startBtn.onClick.AddListener(StartGame);
        quitBtn.onClick.AddListener(QuitGame);
        howtoBtn.onClick.AddListener(HowToPlay);
        backBtn.onClick.AddListener(BackToStart);
    }
    #endregion

    #region PRIVATE METHODS
    void BackToStart()
    {
        sfxSource.PlayOneShot(sfxSource.clip);
        howtoPlay.SetActive(false);
        startPanel.SetActive(true);
    }

    void HowToPlay()
    {
        sfxSource.PlayOneShot(sfxSource.clip);
        howtoPlay.SetActive(true);
        startPanel.SetActive(false);
    }

    void StartGame()
    {
        sfxSource.PlayOneShot(sfxSource.clip);
        SceneManager.LoadScene("main");
    }

    void QuitGame()
    {
        sfxSource.PlayOneShot(sfxSource.clip);
        Application.Quit();
    }
    #endregion
}
