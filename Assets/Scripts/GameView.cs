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

        private IDropdownObserver _dropdownObserver;
        private IPassObserver _passObserver;

        public void SetObserver<T>(T observer)
        {
            if (observer is IDropdownObserver dropdownObserver)
                _dropdownObserver = dropdownObserver;

            if(observer is IPassObserver passObserver)
                _passObserver = passObserver;
        }

        public void ResetObserver()
        {
            _dropdownObserver = null;
            _passObserver = null;
        }

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

        private void OnDropdownValueChanged(int index) => _dropdownObserver?.DropdownValueChanged(index);
        private void OnPassButtonClick() => _passObserver?.Pass();

        public void HideDropdown()
        {
            throw new NotImplementedException();
        }

        public void SetOptions()
        {
            throw new NotImplementedException();
        }
    }
}
