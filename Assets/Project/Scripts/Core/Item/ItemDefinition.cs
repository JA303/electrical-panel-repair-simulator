using UnityEngine;

namespace Project.Scripts.Core.Item
{
    [CreateAssetMenu(menuName = "Mission/Item")]
    public class ItemDefinition : ScriptableObject
    {
        [SerializeField] private string id;
        [SerializeField] private string displayName;
        [SerializeField] private Vector3 holdLocalPosition;
        [SerializeField] private Vector3 holdLocalEulerAngles;

        public string Id => id;
        public string DisplayName => displayName;
        public Vector3 HoldLocalPosition => holdLocalPosition;
        public Vector3 HoldLocalEulerAngles => holdLocalEulerAngles;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(id))
                id = name;
        }
#endif
    }
}