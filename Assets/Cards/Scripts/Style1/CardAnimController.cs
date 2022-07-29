using UnityEngine;
using DG.Tweening;


namespace Cards
{
    public class CardAnimController : MonoBehaviour, ICardAnim
    {
        private Sequence sequence;
        [SerializeField] Transform m_back;
        [SerializeField] Transform m_top;

        private float m_cardOffset = -1f;
        private float m_onPut = -0.3f;
        private float m_onTake = -0.7f;
        private float m_animSpeed = 1f;

        private void Start()
        {
            sequence = DOTween.Sequence();
            if (m_back == null || m_top == null)
            {
                Debug.LogError("Back or Top == NULL!");
            }
        }

        public void OnFirstTake()
        {
            sequence.Complete();
            sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(new Vector3(1.2f, 1.2f), m_animSpeed / 2));
        }

        public void OnTake()
        {
            sequence.Complete();
            sequence = DOTween.Sequence();
            sequence.Append(transform.DOLocalMove(new Vector3(0, m_cardOffset, m_onTake), m_animSpeed / 2));
        }

        public void OnPut()
        {
            sequence.Complete();
            sequence = DOTween.Sequence();
            sequence.Append(transform.DOLocalMove(new Vector3(0, m_cardOffset, m_onPut), m_animSpeed / 2));
        }


        public void OnUnlock()
        {
            m_back.gameObject.SetActive(true);
            m_top.gameObject.SetActive(true);

            m_back.localScale = new Vector3(1f, 1f, 1f);
            m_top.localScale = new Vector3(0f, 1f, 1f);

            sequence.Complete();
            sequence = DOTween.Sequence();
            sequence.Append(m_back.DOScale(new Vector3(0f, 1f, 1f), m_animSpeed / 2));
            sequence.Append(m_top.DOScale(new Vector3(1f, 1f, 1f), m_animSpeed / 2));
            sequence.AppendCallback(() =>
            {
                m_back.gameObject.SetActive(false);
            });
        }

        public void OnLock()
        {
            m_back.gameObject.SetActive(true);
            m_top.gameObject.SetActive(true);

            m_back.localScale = new Vector3(0f, 1f, 1f);
            m_top.localScale = new Vector3(1f, 1f, 1f);

            sequence.Complete();
            sequence = DOTween.Sequence();
            sequence.Append(m_top.DOScale(new Vector3(0f, 1f, 1f), m_animSpeed / 4));
            sequence.Append(m_back.DOScale(new Vector3(1f, 1f, 1f), m_animSpeed / 4));
            sequence.Append(m_back.DORotate(new Vector3(0f, 0f, -10f), m_animSpeed / 8));
            sequence.Append(m_back.DORotate(new Vector3(0f, 0f, 0f), m_animSpeed / 8));
            sequence.Append(m_back.DORotate(new Vector3(0f, 0f, 10f), m_animSpeed / 8));
            sequence.Append(m_back.DORotate(new Vector3(0f, 0f, 0f), m_animSpeed / 8));
            sequence.AppendCallback(() =>
            {
                m_top.gameObject.SetActive(false);
            });
        }


        void OnDestroy()
        {
            sequence.Complete();
            sequence.Kill();
        }

        public void OnNewPos(Vector3 newpos)
        {
            sequence.Complete();
            sequence = DOTween.Sequence();
            sequence.Append(transform.DOMove(newpos, m_animSpeed));

        }
    }

}