using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace CardGame.UI
{
    public class MainMenuEventHandler : MonoBehaviour
    {
        public string mapScene;
        static readonly string saveName = "continue";

        [SerializeField]
        private UIDocument m_UIDocument;

        private Button m_ContinueButton;
        private Button m_NewButton;
        private Button m_SettingsButton;
        private Button m_QuitButton;


        // Start is called before the first frame update
        void Start()
        {
            var rootElement = m_UIDocument.rootVisualElement;

            m_ContinueButton = rootElement.Q<Button>("ContinueButton");
            if(GameLoader.SaveFileExist(saveName))
            {
                m_ContinueButton.clickable.clicked += OnContinueButtonClicked;
            }
            else
            {
                m_ContinueButton.SetEnabled(false);
            }

            m_NewButton = rootElement.Q<Button>("NewButton");
            m_NewButton.clickable.clicked += OnNewButtonClicked;

            m_SettingsButton = rootElement.Q<Button>("SettingsButton");
            m_SettingsButton.clickable.clicked += OnSettingsButtonClicked;

            m_QuitButton = rootElement.Q<Button>("QuitButton");
            m_QuitButton.clickable.clicked += OnQuitButtonClicked;
        }

        private void OnContinueButtonClicked()
        {
            throw new NotImplementedException();
        }

        private void OnNewButtonClicked()
        {
            SceneManager.LoadScene(mapScene);
        }

        private void OnSettingsButtonClicked()
        {
            throw new NotImplementedException();
        }

        private void OnQuitButtonClicked()
        {
            throw new NotImplementedException();
        }
    }
}
