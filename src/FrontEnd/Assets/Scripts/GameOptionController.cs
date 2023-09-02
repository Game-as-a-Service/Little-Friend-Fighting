using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOptionController : MonoBehaviour
{
    public GameObject[] images;
    private int _currentIndex;
    private bool _isLock;
    private readonly Dictionary<int, int[]> _showDic = new Dictionary<int, int[]>()
    {
        {0, new int[] {1,2,4}},
        {1, new int[] {0,3,4}},
        {2, new int[] {0,2,5}},
    };
    
    private AudioManager _audioManager;
    void Start()
    {
        SetSelectedImage();
        // GameObject
        //     .Find("BackgroundAudioObject")
        //     .GetComponent<AudioManager>()
        //     .PlayLoopSoundFromTo(243.0f, 251.6f);
        
        _audioManager = GameObject
            .Find("AudioObject")
            .GetComponent<AudioManager>();
        
    }

    void Update()
    {
        SelectNextOption();
        SelectLastOption();
        EnterOption();
    }

    private void EnterOption()
    {
        if (!Input.GetKeyDown(KeyCode.Return) && !Input.GetKeyDown(KeyCode.KeypadEnter)) return;
        _isLock = true;
        _audioManager.PlaySoundFromTo(241.0f, 241.9318820861678f, onMusicCompleteCallback: MusicCompleteCallback);
    }

    private void SelectLastOption()
    {
        if (!Input.GetKeyDown(KeyCode.UpArrow) || _isLock) return;
        if (_currentIndex == 0) _currentIndex = 2;
        else _currentIndex--;
        SetSelectedImage();
        _audioManager.PlaySoundFromTo(232.0f, 232.16f);
    }

    private void SelectNextOption()
    {
        
        if (!Input.GetKeyDown(KeyCode.DownArrow) || _isLock) return;
        if (_currentIndex == 2) _currentIndex = 0;
        else _currentIndex++;
        SetSelectedImage();
        _audioManager.PlaySoundFromTo(232.0f, 232.16f);
    }


    private void SetSelectedImage()
    {
        for (var i = 0; i < images.Length; i++)
            images[i].SetActive(_showDic[_currentIndex].Contains(i));
    }
    
   
    private void MusicCompleteCallback()
    {
        // 依照_currentIndex 決定切換場景 先寫死
        SwitchToTargetScene("WalkScene");
    }
    private void SwitchToTargetScene(string targetSceneName) => SceneManager.LoadScene(targetSceneName);
    
    
}