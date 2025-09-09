using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;
using SnakeGame.Gameplay.Skinning;

namespace SnakeGame.UI.Windows
{
    public class SkinWindow : AbstractWindow
    {
        [SerializeField] private Toggle _skinTogglePrefab;
        [SerializeField] private Button _confirmButton;
        [SerializeField] private Button _cancelButton;
        [SerializeField] private ToggleGroup _toggleGroup;

        private ISkinService _skinService;
        private SkinCatalog _catalog;
        private readonly List<Toggle> _spawned = new List<Toggle>();
        private string _pendingSkinId;

        [Inject]
        private void Construct(ISkinService skinService, SkinCatalog catalog = null)
        {
            _skinService = skinService;
            _catalog = catalog;
        }

        private void Awake()
        {
            if (_confirmButton != null)
            {
                _confirmButton.onClick.AddListener(HandleConfirmClick);
            }
            if (_cancelButton != null)
            {
                _cancelButton.onClick.AddListener(HandleCloseClick);
            }
        }

        public override void Open()
        {
            base.Open();
            RebuildList();
        }

        private void RebuildList()
        {
            ClearList();

            var current = _skinService.CurrentSkinId;
            var firstId = default(string);
            foreach (var id in GetSkinIds())
            {
                if (firstId == null) firstId = id;
                var t = Instantiate(_skinTogglePrefab, _toggleGroup.transform);
                var text = t.GetComponentInChildren<TextMeshProUGUI>();
                if (text != null) text.text = _skinService.GetBundleName(id);
                t.group = _toggleGroup;
                var captured = id;
                t.onValueChanged.AddListener(isOn => { if (isOn) _pendingSkinId = captured; });
                _spawned.Add(t);
                t.isOn = !string.IsNullOrEmpty(current) ? (captured == current) : (captured == firstId);
            }

            // Initialize pending selection
            _pendingSkinId = !string.IsNullOrEmpty(current) ? current : firstId;
        }

        private void ClearList()
        {
            foreach (var t in _spawned)
            {
                if (t != null) Destroy(t.gameObject);
            }
            _spawned.Clear();
        }

        private IEnumerable<string> GetSkinIds()
        {
            if (_catalog == null)
            {
                yield return _skinService.CurrentSkinId ?? "default";
                yield break;
            }

            foreach (var id in _catalog.GetAllIds())
            {
                yield return id;
            }
        }

        private void HandleConfirmClick()
        {
            if (!string.IsNullOrEmpty(_pendingSkinId))
            {
                _skinService.SetSkin(_pendingSkinId);
                HandleCloseClick();
            }
        }
    }
}


