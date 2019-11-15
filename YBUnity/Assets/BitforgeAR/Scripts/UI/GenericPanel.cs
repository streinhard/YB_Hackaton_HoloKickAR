using UnityEngine;
using UnityEngine.UI;
// ReSharper disable RedundantDefaultMemberInitializer

namespace UI
{
    public class GenericPanel : MonoBehaviour
    {
        [SerializeField]
        protected Button closeButton = null;

        private CanvasGroup _canvasGroup;
        
        public Button.ButtonClickedEvent OnCloseClick => closeButton.onClick;

        protected virtual void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        protected virtual void Start()
        {
            Hide();
        }

        public virtual void Show()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            gameObject.SetActive(false);
        }

        public virtual void TouchOnBlankScreen(Vector3 position) { }
    }
}
