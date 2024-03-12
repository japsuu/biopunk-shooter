using Thirdparty.UnityTooltips.Scripts.UI;
using UnityEngine;
using UnityEngine.Assertions;

namespace Thirdparty.UnityTooltips.Scripts
{
    public class TwoPmLogoViewController : MonoBehaviour
    {
        public ReactToPointer ReactToPointer;

        // Start is called before the first frame update
        void Start()
        {
            Assert.IsNotNull(ReactToPointer);

            ReactToPointer.OnPointerSelected += () => {
                Application.OpenURL("https://twopm.studio");
            };
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
