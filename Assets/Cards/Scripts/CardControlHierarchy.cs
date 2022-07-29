using UnityEngine;

namespace Cards
{
    public class CardControlHierarchy : MonoBehaviour
    {
        [SerializeField] Vector3 offset = new Vector3(0, 0, 0.3f);
        private Transform m_parent = null;
        private Transform m_child = null;

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
            transform.localPosition = offset;
            m_parent = parent;
        }
    }
}
