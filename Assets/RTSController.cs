using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class RTSController : MonoBehaviour
{
    public float boxWidth;
    public float boxHeight;
    public float boxTop;
    public float boxLeft;
    //vector2
    public Vector2 boxStart;
    public Vector2 boxFinish;

    Vector3 p1;
    Vector3 mouseDownPoint;
    Vector3 currentMousePoint;

    public Material selectedMat;
    public Material unSelectedMat;

    bool selecting = false;
    public List<Unit_> selectedUnits = new List<Unit_>();
    public List<Unit_> selectableUnits = new List<Unit_>();

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            p1 = Input.mousePosition;
            selecting = true;

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                currentMousePoint = hit.point;
                 mouseDownPoint = hit.point;
                if (hit.transform.gameObject.GetComponent<Unit>())
                {
                    if (Input.GetAxis("MultiSelect") <= 0f)
                        ClearSelection();

                    selectedUnits.Add(hit.transform.gameObject.GetComponent<Unit_>());
                    hit.transform.gameObject.GetComponent<Renderer>().material = selectedMat;
                }
                else
                    ClearSelection();
            }
        }
        if(Input.GetMouseButton(0))
        {
            bool mouseDragging = true;
            DrawDragBox();
            SelectUnitsInDrag();
        }

        if (Input.GetMouseButtonUp(0))
        {
            selecting = false;

        }

        Unit[] units = GameObject.FindObjectsOfType(typeof(Unit)) as Unit[];

        if (selecting)
        {
            /*if (UIUtils.IsWithBounds(units[0].gameObject, p1) && !selectedUnits.Contains(units[0]))
            {
                Debug.Log(units[0].name);
                selectedUnits.Add(units[0]);
                units[0].GetComponent<Renderer>().material = selectedMat;
            }*/

        }

        if (Input.GetMouseButtonDown(1))
        {
            MoveUnit();
        }
    }

    void SelectUnitsInDrag()
    {
        for (int i = 0; i < selectableUnits.Count; i++)
        {
            if (selectableUnits[i].GetComponent<Unit>().renderer.isVisible)
            {
                Vector2 unitScreenPosition = Camera.main.WorldToScreenPoint(selectableUnits[i].transform.position);

                if (unitScreenPosition.x < boxFinish.x && unitScreenPosition.y > boxFinish.y && unitScreenPosition.x > boxStart.x && unitScreenPosition.y < boxStart.y)
                {
                    AddToCurrentlySelectedUnits(selectableUnits[i]);

                }
                else
                {
                    RemoveFromCurrentlySelectedUnits(selectableUnits[i]);
                }

            }
        }
    }

    private void AddToCurrentlySelectedUnits(Unit_ unitToAdd)
    {
        if (!selectedUnits.Contains(unitToAdd))
        {
            selectedUnits.Add(unitToAdd);
            unitToAdd.transform.Find("SelectionCircle").gameObject.SetActive(true);
        }
    }

    private void RemoveFromCurrentlySelectedUnits(Unit_ unitToRemove)
    {
        if (selectedUnits.Count > 0)
        {
            unitToRemove.transform.Find("SelectionCircle").gameObject.SetActive(false);
            selectedUnits.Remove(unitToRemove);
        }
    }

    void DrawDragBox()
    {
        boxWidth = Camera.main.WorldToScreenPoint(mouseDownPoint).x - Camera.main.WorldToScreenPoint(currentMousePoint).x;
        boxHeight = Camera.main.WorldToScreenPoint(mouseDownPoint).y - Camera.main.WorldToScreenPoint(currentMousePoint).y;
        boxLeft = Input.mousePosition.x;
        boxTop = (Screen.height - Input.mousePosition.y) - boxHeight; //need to invert y as GUI space has 0,0 at top left, but Screen space has 0,0 at bottom left. x is the same. 

        if (boxWidth > 0 && boxHeight < 0f)
            boxStart = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        else if (boxWidth > 0 && boxHeight > 0f)
            boxStart = new Vector2(Input.mousePosition.x, Input.mousePosition.y + boxHeight);
        else if (boxWidth < 0 && boxHeight < 0f)
            boxStart = new Vector2(Input.mousePosition.x + boxWidth, Input.mousePosition.y);
        else if (boxWidth < 0 && boxHeight > 0f)
            boxStart = new Vector2(Input.mousePosition.x + boxWidth, Input.mousePosition.y + boxHeight);

        boxFinish = new Vector2(boxStart.x + Mathf.Abs(boxWidth), boxStart.y - Mathf.Abs(boxHeight));

    }

    private void OnGUI()
    {
        if (selecting)
        {
            UIUtils.DrawRect(p1, Color.white, 2f, Color.white, 0.5f);
        }
    }

    public void ClearSelection()
    {
        foreach (Unit_ unit in selectedUnits)
        {
            unit.renderer.material = unSelectedMat;
        }

        selectedUnits.Clear();
    }

    void MoveUnit()
    {
        if (selectedUnits.Any())
        {

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                foreach (Unit_ unit in selectedUnits)
                {
                    Vector3 startPos = unit.transform.position;
                    unit.transform.position = Vector3.Lerp(startPos, hit.point, unit.MovementSpeed);
                }
            }
        }
    }
}
