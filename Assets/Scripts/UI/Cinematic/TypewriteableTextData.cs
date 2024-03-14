using UnityEngine;

namespace UI.Cinematic
{
    [CreateAssetMenu(menuName = "Create TypewriteableTextData", fileName = "TypewriteableTextData", order = 0)]
    public class TypewriteableTextData : ScriptableObject
    {
        [TextArea]
        public string finalText;
    }
}