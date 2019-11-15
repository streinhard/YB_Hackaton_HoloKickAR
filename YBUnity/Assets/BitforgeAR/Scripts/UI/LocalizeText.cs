using UnityEngine;
using UnityEngine.UI;

namespace UI {
    
    [RequireComponent(typeof(Text))]
    public class LocalizeText : MonoBehaviour
    {
        public string key;

        private void Start()
        {
            GetComponent<Text>().text = Localization.GetText(key);
        }
    }
}
