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
        m_oldPos = Input.mousePosition;
        debugOutput.AppendLine($"{DateTime.Now} | OnDrugStart");
    }

    private void OnDrug()
    {
        m_drugCount++;
        drawRay();
    }

    private void OnDrugEnd()
    {
        debugOutput.AppendLine($"OnDrug: {m_drugCount}");
        debugOutput.AppendLine($"{DateTime.Now} | OnDrugEnd");
        Debug.Log(debugOutput.ToString());
        debugOutput.Clear();
    }

    void drawRay()
    {
        Vector3 camPosition = m_camera.transform.localPosition;
        Vector3 mousez = m_camera.WorldToScreenPoint(new Vector3(0, 0, Input.mousePosition.z));
        Vector3 origin = m_camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_rayDistance));
        Vector3 direction = new Vector3(0, 0, camPosition.z);

        Debug.DrawRay(origin, direction, Color.red);

        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, m_rayDistance);

        StringBuilder stringBuilder = new StringBuilder();
        foreach (RaycastHit2D hit in hits)
        {
            stringBuilder.AppendLine(hit.collider.gameObject.name);
        }

        if (stringBuilder.Length != 0)
        { 
            Debug.Log(stringBuilder.ToString());
        }
    }
}
