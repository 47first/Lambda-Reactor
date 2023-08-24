using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime
{
    public class GameView : MonoBehaviour, IGameView
    {
        [SerializeField] private TMP_Dropdown _dropDown;
        [SerializeField] private Button _passButton;

        private IGameViewObservable _gameViewObserver;

        public void SetObserver(IGameViewObservable gameViewObserver) => _gameViewObserver = gameViewObserver;

        public void ResetObserver() => _gameViewObserver = null;

        public void SetOptions(IEnumerable<string> strings)
        {
            var options = new List<TMP_Dropdown.OptionData>();

            foreach (var text in strings)
                options.Add(new TMP_Dropdown.OptionData(text));

            _dropDown.ClearOptions();
            _dropDown.AddOptions(options);
        }

        private void Start()
        {
            _passButton.onClick.AddListener(OnPassButtonClick);
            _dropDown.onValueChanged.AddListener(OnDropdownValueChanged);
        }

        private void OnDropdownValueChanged(int index) => _gameViewObserver?.DropdownValueChanged(index);
        private void OnPassButtonClick() => _gameViewObserver?.PassButtonClicked();

        public void HideDropdown()
        {
            throw new NotImplementedException();
        }

        public void SetOptions()
        {
            throw new NotImplementedException();
        }
    }

    public interface IGameView
    {
        public void HideDropdown();
        public void SetObserver(IGameViewObservable gameViewObserver);
        public void ResetObserver();
        public void SetOptions();
    }

    public interface IGameViewObservable
    {
        public void PassButtonClicked();
        public void DropdownValueChanged(int index);
    }
}
