using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DebugCanvas : MonoBehaviour
    {
        [SerializeField] private Button _nextLevel;

        private void Awake()
        {
            #if !DEBUG
            gameObject.SetActive(false);
            return;
            #endif

            _nextLevel.onClick.AddListener(NextLevelClick);
        }

        private void NextLevelClick()
        {
            Services.Get<GameFlowService>().GoToNextLevel();
        }
    }
}