using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropManager : MonoBehaviour 
{
    //	#########################################
    //	O			DragAndDropManager			O
    //	O---------------------------------------O
    //	O	Author: Martin Lohse				O
    //	O	Date: 18.04.2018					O
    //	O	Edited: X							O
    //	O	Description: A Manager Class for	O
    //	O	             all the operations of	O
    //	O                the Drag and Drop.  	O
    //	O---------------------------------------O
    //	O	Name:								O
    //	O	Date:								O
    //  O 	Chanes:								O
    //	O---------------------------------------O
    //	O										O
    //	#########################################

    private GameObject m_currentDragged;
    private Transform m_originalParent;
    private Vector3 m_originalPosition;
    private Vector2 _lastMousePosition;

    private void Reset()
    {
        m_currentDragged.transform.parent = m_originalParent;
        m_currentDragged.transform.position = m_originalPosition;
    }

    private void Drop()
    {
        // Finds all GameObjects at the MousePosition.
        GameObject[] RaycastHits = RaycastAtMousePosition();

        // If there are more than, or 2 Objects hit...
        if (RaycastHits.Length >= 2)
        {
            // Get the first hit!
            GameObject Container = RaycastHits[1];

            // If the Container has a Idragable on it...
            if (Container.GetComponent<IDragable>() != null)
            {
                // Take another Gameobject instead.
                Container = RaycastHits[2];
            }

            // Gets the ITargetDrop from the Container.
            ITargetDrop Target = Container.GetComponent<ITargetDrop>();

            // If the Target is not null and the draggable Item as been accepted...
            if (Target != null && Target.Accept(m_currentDragged.GetComponent<IDragable>()))
            {
                // If the Object has to replace another Item...
                if (Target.NeedToChange())
                {
                    // Take the originalTarget.
                    ITargetDrop OriginalTarget = m_originalParent.GetComponent<ITargetDrop>();

                    // And take also the Object to Swap
                    IDragable ItemToSwap = Container.GetComponentInChildren<IDragable>();

                    // If the original target does not accept the Item you want to swap...
                    if (!OriginalTarget.Accept(ItemToSwap))
                    {
                        // Reset the current Dragged.
                        Reset();
                        return;
                    }

                    // Sets the Parent of the current Dragged to the transform of the Container.
                    ItemToSwap.gameObject.transform.parent = m_originalParent;
                    // Finishes the Dragging-Process
                    OriginalTarget.OnDragFinished(ItemToSwap.gameObject);
                }

                // Sets the Parent of the current dragged Object to the Container.
                m_currentDragged.transform.parent = Container.transform;

                // Finishes the current dragging-Process.
                Target.OnDragFinished(m_currentDragged);
            }
            else
            {
                Reset();
            }
        }
        else
        {
            Reset();
        }

        // Stops the Dragging-Process
        m_currentDragged = null;
    }

    private GameObject[] RaycastAtMousePosition()
    {
        // A new PointerEventData.
        PointerEventData pointereventdata = new PointerEventData(EventSystem.current);

        // Sets the Data-Position to the Mouse-Position.
        pointereventdata.position = Input.mousePosition;

        // A new List of Objects that got hit by a Rayhast.
        List<RaycastResult> hitresults = new List<RaycastResult>();

        // Takes all the data from the hitresults.
        EventSystem.current.RaycastAll(pointereventdata, hitresults);

        // returns all the GameObjects, that got hit
        return hitresults.Select(hit => hit.gameObject).ToArray();
    }
	
	void Update () 
	{
        // If there is something dragged...
        if (m_currentDragged != null)
        {
            // If the MouseButton was released...
            if (Input.GetMouseButtonUp(0))
            {
                // Than drop it!
                Drop();
                return;
            }
            else
            {
                // Calculate the Offset.
                Vector2 Offset = (Vector2)Input.mousePosition - _lastMousePosition;

                // Sets the last Mouse-Position to the current Mouse-Position.
                _lastMousePosition = Input.mousePosition;

                // Gets the Recttransform of the current Dragged.
                RectTransform rect = m_currentDragged.GetComponent<RectTransform>();

                // Translates the Object by X,Y
                rect.position += (Vector3)Offset;
            }
        }
        else
        {
            // If the Mouse was pressed...
            if (Input.GetMouseButtonDown(0))
            {
                // Gets all the GameObjects that got hit again.
                GameObject[] hits = RaycastAtMousePosition();

                // If there are no Objects hit...
                if (hits.Length == 0)
                    return;

                // Gets the Object on the Top.
                GameObject UpperObject = hits[0];

                // If the UpperObject has Idragable on it...
                if (UpperObject.GetComponent<IDragable>() != null)
                {
                    // Save the Original Parent
                    m_originalParent = UpperObject.transform.parent;

                    // Save the Original Position
                    m_originalPosition = UpperObject.transform.position;

                    // Sets the UpperObjects Parent to the transform.
                    UpperObject.transform.parent = transform;

                    // Sets the current Dragged to the Object on the Top.
                    m_currentDragged = UpperObject;

                    // Sets the LastMousePosition to the current Mouse Pos.
                    _lastMousePosition = Input.mousePosition;
                }
            }
        }
	}
}
