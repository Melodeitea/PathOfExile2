using UnityEngine;
using UnityEditor;
using UnityEngine.SocialPlatforms;

[CustomEditor(typeof(UnlockableElement))]
public class DoorCreator : Editor
{
    GameObject Lever;
    UnlockableElement self;
    public override void OnInspectorGUI()
    {
        Lever = Resources.Load<GameObject>("Lever");
        self = (UnlockableElement)target;

        DrawDefaultInspector();
        if (GUILayout.Button("Add Lever"))
        {
            GameObject lever = Instantiate(Lever);
            lever.transform.position = self.transform.position;
            self.AddLock(lever.GetComponent<Lever>());
        }
    }

}
