using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class ScreenFader : MonoBehaviour
    {
        [FormerlySerializedAs("_fadeImg")]
        [SerializeField]
        private Image _fadeImage;
        
        [SerializeField]
        private float _fadeSpeed = 2f;
        
        // private bool _sceneStarting = true;


        private void Awake()
        {
            // _fadeImg.rectTransform.localScale = new Vector2(Screen.width, Screen.height);
        }


        private IEnumerator Start()
        {
            _fadeImage.enabled = true;
            _fadeImage.color = Color.black;
            yield return new WaitForSeconds(1f);
         
            _fadeImage.DOFade(0f, _fadeSpeed).onComplete += () => _fadeImage.enabled = false;
        }
        
        
        public void EndScene(int sceneNumber)
        {
            _fadeImage.enabled = true;
            _fadeImage.color = Color.clear;
            _fadeImage.DOFade(1f, _fadeSpeed).onComplete += () => SceneManager.LoadScene(sceneNumber);
        }


        /*private void Update()
        {
            if (_sceneStarting)
                StartScene();
        }


        public void EndScene(int sceneNumber)
        {
            _sceneStarting = false;
            StartCoroutine(EndSceneRoutine(sceneNumber));
        }


        private void FadeToClear()
        {
            // Lerp the colour of the image between itself and transparent.
            _fadeImg.color = Color.Lerp(_fadeImg.color, Color.clear, _fadeSpeed * Time.deltaTime);
        }


        private void FadeToBlack()
        {
            // Lerp the colour of the image between itself and black.
            _fadeImg.color = Color.Lerp(_fadeImg.color, Color.black, _fadeSpeed * Time.deltaTime);
        }


        private void StartScene()
        {
            // Fade the texture to clear.
            _fadeImg.color = Color.black;
            yield return new WaitForSeconds(1f);
            FadeToClear();

            // If the texture is almost clear...
            if (_fadeImg.color.a <= 0.05f)
            {
                // ... set the colour to clear and disable the RawImage.
                _fadeImg.color = Color.clear;
                _fadeImg.enabled = false;

                // The scene is no longer starting.
                _sceneStarting = false;
            }
        }


        private IEnumerator EndSceneRoutine(int sceneNumber)
        {
            _fadeImg.enabled = true;
            _fadeImg.color = Color.clear;
            do
            {
                // Start fading towards black.
                FadeToBlack();

                // If the screen is almost black...
                if (_fadeImg.color.a >= 0.95f)
                {
                    _fadeImg.color = Color.black;
                    yield return new WaitForSeconds(1f);
                    // ... reload the level
                    SceneManager.LoadScene(sceneNumber);
                    yield break;
                }
                else
                {
                    yield return null;
                }
            }
            while (true);
        }*/
    }
}