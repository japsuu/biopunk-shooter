using System;
using System.Collections;
using JSAM;
using Saving;
using Scenes;
using TMPro;
using UnityEngine;

namespace UI.Cinematic
{
    public class TMPTypewriter : MonoBehaviour
    {   //TODO: Refactor this abomination
        
        private const string FIRST_SKIP_TEXT = "Press [SPACE] to skip";
        private const string SECOND_SKIP_TEXT = "Press [SPACE] to continue";

        [SerializeField]
        private AudioLibrarySounds _typeSound;
        
        /// <summary>
        /// First press shows all the text, second press loads the specified scene.
        /// </summary>
        [SerializeField]
        private KeyCode _skipKey;

        [SerializeField]
        private int _sceneIndexToLoad;

        [SerializeField]
        private TMP_Text _skipText;
        
        [SerializeField]
        private TMP_Text _targetTextField;

        [SerializeField]
        private TypewriteableTextData _textData;

        [SerializeField]
        private PauseInfo _pauseInfo;
        
        private int _index;
        private string _textFieldText = "";


        private void Awake()
        {
            if (Cinematics.HasPlayerSeenCinematic())
                SceneChanger.ChangeSceneInstant(_sceneIndexToLoad);
            else
                AudioListener.volume = 0.5f;
        }


        private void Start()
        {
            _targetTextField.text = "";
            _skipText.text = FIRST_SKIP_TEXT;
            
            ReproduceText();
        }


        private void Update()
        {
            if (!Input.GetKeyDown(_skipKey))
                return;
            
            if (_index < _textData.finalText.Length)
            {
                _index = _textData.finalText.Length;
                _targetTextField.text = _textData.finalText;
                _skipText.text = SECOND_SKIP_TEXT;
            }
            else
            {
                Cinematics.SetPlayerHasSeenCinematic(true);
                SceneChanger.ChangeSceneFaded(_sceneIndexToLoad);
            }
        }


        private void ReproduceText()
        {
            if (_index >= _textData.finalText.Length)
                return;

            //get one letter
            char letter = _textData.finalText[_index];

            //Actualize on screen
            _targetTextField.text = Write(letter);
            
            // Play sound
            //if (_index % 2 == 0)
                AudioManager.PlaySound(_typeSound);

            //set to go to the next
            _index += 1;
            StartCoroutine(PauseBetweenChars(letter));
        }


        private string Write(char letter)
        {
            _textFieldText += letter;
            return _textFieldText;
        }


        private IEnumerator PauseBetweenChars(char letter)
        {
            switch (letter)
            {
                case '.':
                    yield return new WaitForSeconds(_pauseInfo.DotPause);
                    ReproduceText();
                    yield break;
                case ',':
                    yield return new WaitForSeconds(_pauseInfo.CommaPause);
                    ReproduceText();
                    yield break;
                case ' ':
                    yield return new WaitForSeconds(_pauseInfo.SpacePause);
                    ReproduceText();
                    yield break;
                default:
                    yield return new WaitForSeconds(_pauseInfo.NormalPause);
                    ReproduceText();
                    yield break;
            }
        }
    }
}