using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoIntroController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextSceneName;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(2);
        }
    }


    void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene(2);
    }
}
