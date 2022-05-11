using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UpDownManager : MonoBehaviour
{
    class GroupInfo
    {
        public List<UpDownComponent> components;
        public bool isUp;

        public GroupInfo(List<UpDownComponent> components, bool isUp)
        {
            this.components = components;
            this.isUp = isUp;
        }

        public void ToggleIsUp()
        {
            isUp = !isUp;
        }
    }

    static Dictionary<int, GroupInfo> groups;
    [SerializeField] int moveTime;
    [SerializeField] ACameraInstruction demoScene;
    [SerializeField] CameraUtils camUtils;
    [SerializeField] List<int> groupsThatStartDown;
    static List<int> groupsThatStartDownSt;
    bool showedDemoScene; // Demo scene should only happen once
    bool transitioning = false;

    private void Awake()
    {
        groups = new Dictionary<int, GroupInfo>();
        groupsThatStartDownSt = groupsThatStartDown;
    }

    public static void Register(UpDownComponent comp)
    {
        if (!groups.ContainsKey(comp.groupID))
        {
            groups.Add(comp.groupID, new GroupInfo(new List<UpDownComponent>(), GroupStartsUp(comp.groupID)));
        }
        groups[comp.groupID].components.Add(comp);
    }

    public void Toggle(int groupID)
    {
        if (transitioning)
        {
            Debug.LogError("Cannot accomodate up-down toggle. Already in transition mode.");
            return;
        }
        if (!showedDemoScene)
        {
            camUtils.HandleInstructions(demoScene);
            showedDemoScene = true;
        }
        StartCoroutine("Move", groups[groupID]);
    }

    IEnumerator Move(GroupInfo g)
    {
        transitioning = true;
        float timer = 0;
        while (timer < moveTime)
        {
            foreach(UpDownComponent c in g.components)
            {
                c.transform.position =
                    g.isUp ?
                    Vector3.Lerp(c.up.position, c.down.position, timer / moveTime) :
                    Vector3.Lerp(c.down.position, c.up.position, timer / moveTime);
            }
            timer += IndependentTime.deltaTime;
            yield return null;
        }
        g.ToggleIsUp();
        transitioning = false;
    }

    public static bool GroupStartsUp(int groupID)
    {
        return !groupsThatStartDownSt.Contains(groupID);
    }
}
