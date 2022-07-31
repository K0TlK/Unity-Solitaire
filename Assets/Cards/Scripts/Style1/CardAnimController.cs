using UnityEngine;
using DG.Tweening;


namespace Cards
{
    public class CardAnimController : MonoBehaviour, ICardAnim
    {
        private Sequence sequence;
        [SerializeField] Transform m_back;
        [SerializeField] Transform m_top;

        private const float m_cardOffset = -0.5f;
        private const float m_onPut = -0.7f;
        private const float m_animSpeed = 1f;

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
            OnAnimStart();
            sequence.Append(transform.DOScale(new Vector3(1.2f, 1.2f), m_animSpeed / 2));
        }

        public void OnTake(float offset = 0)
        {
            OnAnimStart();
            sequence.Append(transform.DOLocalMove(new Vector3(0, 0, 0), m_animSpeed / 2));
        }

        public void OnPut(float offset = 0)
        {
            OnAnimStart();
            sequence.Append(transform.DOLocalMove(new Vector3(0, m_onPut * offset, m_cardOffset * offset), m_animSpeed / 2));
        }


        public void OnUnlock()
        {
            OnAnimStart();
            m_back.gameObject.SetActive(true);
            m_top.gameObject.SetActive(true);

            m_back.localScale = new Vector3(1f, 1f, 1f);
            m_top.localScale = new Vector3(0f, 1f, 1f);

            sequence.Append(m_back.DOScale(new Vector3(0f, 1f, 1f), m_animSpeed / 2));
            sequence.Append(m_top.DOScale(new Vector3(1f, 1f, 1f), m_animSpeed / 2));
            sequence.AppendCallback(() =>
            {
                m_back.gameObject.SetActive(false);
            });
        }

        public void OnLock()
        {
            OnAnimStart();

            m_back.gameObject.SetActive(true);
            m_top.gameObject.SetActive(true);

            m_back.localScale = new Vector3(0f, 1f, 1f);
            m_top.localScale = new Vector3(1f, 1f, 1f);

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

        public void OnNewPos(Vector3 newpos)
        {
            OnAnimStart();
            sequence.Append(transform.DOMove(newpos, m_animSpeed));

        }

        private void OnAnimStart()
        {
            if (sequence == null || !sequence.IsActive())
            {
                sequence = DOTween.Sequence();
            }
        }

        public void StopAnim()
        {
            sequence.Complete();
            sequence.Kill();
            sequence = DOTween.Sequence();
        }

        private void OnDestroy()
        {
            sequence.Kill();
        }
    }

}