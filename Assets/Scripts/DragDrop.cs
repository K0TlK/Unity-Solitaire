using Cards;
using System;
using System.Text;
using UnityEngine;


public class DragDrop : MonoBehaviour
{
    [SerializeField] private float m_rayDistance = 10.0f;
    [SerializeField] private Camera m_camera;
    [SerializeField] private float m_activeCardOffset = -5.0f;
    [SerializeField] private GameController m_controller;

    private Transform m_dragTarget;
    private bool m_isDraged = false;
    private StringBuilder debugOutput = new StringBuilder();
    private int m_dragCount = 0;

    private Vector3 m_oldPos;
    private Transform m_cardTarget;
    private CardController m_cardTargetController;
    private CardController m_cardTargetController2;


    private void Start()
    {
        if (m_camera == null)
        {
            m_camera = Camera.main;
        }
    }

    void Update()
    {
        if (CheckClick())
        {
            OnDrag();
        }
    }

    private bool CheckClick()
    {
        bool isClick = Input.GetMouseButton(0);

        if (isClick)
        {
            if (!m_isDraged)
            {
                OnDragStart();
                m_isDraged = true;
            }
        }
        else
        {
            if (m_isDraged)
            {
                OnDragEnd();
                m_isDraged = false;
            }
        }

        return isClick;
    }

    private void OnDragStart()
    {
        m_dragCount = 0;
        m_oldPos = Vector3.zero;
        m_cardTarget = null;
        m_cardTargetController = null;
        Cursor.lockState = CursorLockMode.Confined;
        debugOutput.AppendLine($"{DateTime.Now} | OnDragStart");
    }

    private void OnDrag()
    {
        if (m_oldPos == Input.mousePosition)
        {
            return;
        }

        m_dragCount++;

        if (m_cardTarget == null)
        {
            m_cardTargetController = GetTarget();
            if (m_cardTargetController != null)
            {
                m_cardTargetController.TakeCard();
                m_cardTarget = m_cardTargetController.transform;
            }
        }
        else
        {
            DragCard();
        }

        m_oldPos = Input.mousePosition;
    }

    private void OnDragEnd()
    {
        if (m_cardTargetController != null)
        {
            m_cardTargetController2 = GetTarget();
            m_controller.MergingCards(m_cardTargetController, m_cardTargetController2);
            m_cardTargetController.PutCard();
        }
        

        Cursor.lockState = CursorLockMode.None;
        debugOutput.AppendLine($"OnDrag: {m_dragCount} updates");
        debugOutput.AppendLine($"{DateTime.Now} | OnDragEnd");
        Debug.Log(debugOutput.ToString());
        debugOutput.Clear();
    }

    /// <summary>
    /// Guaranteed to give the last card.
    /// </summary>
    /// <returns>Returns null if not found, returns the card if it exists and is the last card in the stack.</returns>
    private CardController GetTarget()
    {
        Vector3 camPosition = m_camera.transform.localPosition;
        Vector3 origin = Vector3.zero;

#if UNITY_STANDALONE_WIN
        origin = m_camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_rayDistance));
#elif UNITY_ANDROID
        if (Input.touchCount == 1)
        {
            origin = m_camera.ScreenToWorldPoint(new Vector3(Input.touches[0].position.x, Input.touches[0].position.y, m_rayDistance));
        }
        else
        {
            return null;
        }

#else
        
#endif
        if (origin == Vector3.zero)
        {
            return null;
        }

        Vector3 direction = new Vector3(0, 0, camPosition.z);

        Debug.DrawRay(origin, direction, Color.red);

        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, m_rayDistance);

        foreach (RaycastHit2D obj in hits)
        {
            if (obj.transform.tag.Contains("Card"))
            {
                CardController newTarget = obj.collider.GetComponent<CardController>();
                if (newTarget.index == newTarget.place.indexLastCard)
                {
                    debugOutput.AppendLine("Raycast find card: " + obj.collider.gameObject.name);
                    return newTarget;
                }
            }
        }

        return null;
    }

    private void DragCard()
    {

        Vector3 newPos = m_camera.ScreenToWorldPoint(new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            0));

        newPos.z = m_activeCardOffset; //m_cardTarget.position.z

        m_cardTarget.position = newPos;
    }
}
