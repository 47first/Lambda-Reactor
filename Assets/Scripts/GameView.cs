using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Runtime
{
    public class GameView : MonoBehaviour, IGameView
    {
        [Inject] private IEnvironmentController _environmentController;

        [SerializeField] private TMP_Dropdown _dropdown;
        [SerializeField] private Button _passButton;
        [SerializeField] private GameObject _gameOverTable;
        [SerializeField] private TextMeshProUGUI _winerLabel;


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

            _dropdown.ClearOptions();
            _dropdown.AddOptions(options);
        }

        private void Start()
        {
            _passButton.onClick.AddListener(OnPassButtonClick);
            _dropdown.onValueChanged.AddListener(OnDropdownValueChanged);

            HideDropdown();
        }

        private void OnDropdownValueChanged(int index) => _dropdownObserver?.DropdownValueChanged(index);
        private void OnPassButtonClick() => _passObserver?.Pass();

        public void ShowDropdown() => _dropdown.gameObject.SetActive(true);

        public void HideDropdown() => _dropdown.gameObject.SetActive(false);

        public int GetDropdownValue() => _dropdown.value;

        public void ShowResult()
        {
            var winnerTeam = _environmentController.Units.First().Presenter.Team;

            _gameOverTable.SetActive(true);
            _winerLabel.text = $"{winnerTeam} wins";
        }
    }
}
