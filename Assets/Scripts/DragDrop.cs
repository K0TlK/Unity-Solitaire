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
            
            if (m_cardTargetController2 != null)
            {
                CheckingCardConnectivity();
                m_cardTargetController.PutCard();
                m_cardTargetController2.PutCard();
            }
            else
            {
                debugOutput.AppendLine("Can't find second card");
            }
            m_cardTargetController.PutCard();
        }

        Cursor.lockState = CursorLockMode.None;
        debugOutput.AppendLine($"OnDrag: {m_dragCount} updates");
        debugOutput.AppendLine($"{DateTime.Now} | OnDragEnd");
        Debug.Log(debugOutput.ToString());
        debugOutput.Clear();
    }

    private void CheckingCardConnectivity()
    {
        debugOutput.AppendLine($"Start Checking Card Connectivity. 1[{m_cardTargetController.additionalInformation}] - 2[{m_cardTargetController2.additionalInformation}] = " +
            $"{Mathf.Abs(m_cardTargetController2.Name - m_cardTargetController.Name).ToString()}");

        bool flag = m_cardTargetController.additionalInformation == "active";
        if (m_cardTargetController2.additionalInformation == "active")
        {
            flag = true;
            CardController swap = m_cardTargetController;
            m_cardTargetController = m_cardTargetController2;
            m_cardTargetController2 = swap;
        }

        if (flag)
        {
            if (m_cardTargetController2.additionalInformation == "bank")
            {
                m_cardTargetController2.UnlockCard();
                MergingCards();
            }

            if (m_cardTargetController2.additionalInformation != "active")
            {
                if (Mathf.Abs(m_cardTargetController2.Name - m_cardTargetController.Name) == 1)
                {
                    MergingCards();
                }
                else
                {
                    debugOutput.AppendLine($"Failed: [{m_cardTargetController2.Name}] - [{m_cardTargetController.Name}] = " +
                        Mathf.Abs(m_cardTargetController2.Name - m_cardTargetController.Name).ToString());
                    return;
                }
            }
            else
            {
                debugOutput.AppendLine("Failed: Two Active Cards");
                return;
            }
        }
        else
        {
            debugOutput.AppendLine("Failed: No active cards");
            return;
        }
    }

    private void MergingCards()
    {
        if (m_cardTargetController2.additionalInformation == "active")
        {
            debugOutput.AppendLine("Two Active Card");
            return;
        }

        m_cardTargetController.place.SetNewPlace(m_cardTargetController2);
        string tmp = m_cardTargetController2.additionalInformation;
    }

    /// <summary>
    /// Guaranteed to give the last card.
    /// </summary>
    /// <returns>Returns null if not found, returns the card if it exists and is the last card in the stack.</returns>
    private CardController GetTarget()
    {
        Vector3 camPosition = m_camera.transform.localPosition;
        Vector3 origin = m_camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_rayDistance));
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
