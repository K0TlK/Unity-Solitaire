using Cards;
using System;
using System.Text;
using UnityEngine;


public class DragDrop : MonoBehaviour
{
    [SerializeField] private int m_rayDistance = 10;
    [SerializeField] private Camera m_camera;

    [SerializeField]
    private Transform dragTarget;

    private bool m_isDraged = false;
    private StringBuilder debugOutput = new StringBuilder();
    private int m_drugCount = 0;

    private Vector3 m_oldPos;
    private Transform m_cardTarget;

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
            OnDrug();
        }
    }

    private bool CheckClick()
    {
        bool isClick = Input.GetMouseButton(0);

        if (isClick)
        {
            if (!m_isDraged)
            {
                OnDrugStart();
                m_isDraged = true;
            }
        }
        else
        {
            if (m_isDraged)
            {
                OnDrugEnd();
                m_isDraged = false;
            }
        }

        return isClick;
    }

    private void OnDrugStart()
    {
        m_drugCount = 0;
        m_oldPos = Vector3.zero;
        m_cardTarget = null;
        Cursor.lockState = CursorLockMode.Confined;
        debugOutput.AppendLine($"{DateTime.Now} | OnDrugStart");
    }

    private void OnDrug()
    {
        if (m_oldPos == Input.mousePosition)
        {
            return;
        }

        m_drugCount++;

        if (m_cardTarget == null)
        {
            GetTarget();
        }
        else
        {
            DrugCard();
        }

        m_oldPos = Input.mousePosition;
    }

    private void OnDrugEnd()
    {
        Cursor.lockState = CursorLockMode.None;
        debugOutput.AppendLine($"OnDrug: {m_drugCount} updates");
        debugOutput.AppendLine($"{DateTime.Now} | OnDrugEnd");
        Debug.Log(debugOutput.ToString());
        debugOutput.Clear();
    }

    private void GetTarget()
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
                debugOutput.AppendLine("Raycast find card: " + obj.collider.gameObject.name);
                m_cardTarget = obj.collider.transform;
                m_cardTarget.GetComponent<CardController>().UnlockCard();
                break;
            }
        }

        debugOutput.AppendLine($"{DateTime.Now} | Raycast can't find card");
    }

    private void DrugCard()
    {

        Vector3 newPos = m_camera.ScreenToWorldPoint(new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            0));

        newPos.z = m_cardTarget.position.z;

        m_cardTarget.position = newPos;
    }
}
