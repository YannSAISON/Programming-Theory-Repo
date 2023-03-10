using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private MeshRenderer m_mesh;

    [SerializeField] private Color m_hoverColor;
    [SerializeField] private float m_maxDistance;

    protected bool isHovered;
    protected Color m_baseColor;

    protected void Awake()
    {
        m_mesh = GetComponent<MeshRenderer>();

        if (m_mesh == null)
        {
            Debug.LogError("Object must have a mesh to be interactable");
            Destroy(gameObject);
            return;
        }

        m_baseColor = m_mesh.material.color;
    }

    protected void Update()
    {
        OnLooking();
        OnUnlooking();
    }

    private void HoverObject()
    {
        m_mesh.material.color = m_hoverColor;
        isHovered = true;
    }

    private void UnhoverObject()
    {
        m_mesh.material.color = m_baseColor;
        isHovered = false;
    }

    // ABSTRACTION
    private void OnLooking()
    {
        float x = Screen.width / 2;
        float y = Screen.height / 2;

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(x, y, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, m_maxDistance))
        {
            if (hit.transform == transform)
            {
                HoverObject();
            }
        }
    }

    // ABSTRACTION
    private void OnUnlooking()
    {
        float x = Screen.width / 2;
        float y = Screen.height / 2;

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(x, y, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, m_maxDistance))
        {
            if (hit.transform != transform)
            {
                UnhoverObject();
            }
        } else
        {
            UnhoverObject();
        }
    }

    public void ResetMaterial()
    {
        UnhoverObject();
    }
}
