using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePanelGroup : MonoBehaviour
{
    [SerializeField] float alternationTime; // Time before each alternation
    List<StatePanel> panels = new List<StatePanel>();

    private void Start()
    {
        StartCoroutine(AlternationPattern());
    }

    /// <summary>
    /// At the start of the same, initializes the given state panel as part of
    /// this group. Assumes no repeated panels.
    /// </summary>
    public void RegisterPanel(StatePanel panel)
    {
        panels.Add(panel);
    }

    /// <summary>
    /// Executes the alternation of walkability for all panels in the group.
    /// </summary>
    IEnumerator AlternationPattern()
    {
        while (true)
        {
            yield return new WaitForSeconds(alternationTime);
            foreach(StatePanel panel in panels)
            {
                panel.AlternateState();
            }
        }
    }
}
