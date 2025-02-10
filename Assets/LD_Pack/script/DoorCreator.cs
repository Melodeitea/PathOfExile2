using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


[CustomEditor(typeof(UnlockableElement))]
public class DoorCreator : Editor
{
    GameObject Lever;
    int CreatorTracker=0;
    
    public Vector3 NextLeverPos(Vector3 basepos)
    {
        Vector3 newPos = Vector3.zero;
        newPos = basepos + Vector3.right *3+(Vector3.right*3*CreatorTracker)+Vector3.down;
        return newPos;
    }
    public override void OnInspectorGUI()
    {
        Lever = Resources.Load<GameObject>("Lever");

        DrawDefaultInspector();
        UnlockableElement door = (UnlockableElement)target;
        if (GUILayout.Button("New Lever"))
        {
            Debug.Log("CreatorTracker : " + CreatorTracker);
            CreatorTracker++;
            Debug.Log("CreatorTracker : "+ CreatorTracker);
            door.name = "Door : " + door.KeyCode;
            GameObject lever = Instantiate(Lever);
            lever.transform.position = NextLeverPos(door.transform.position); 
            lever.name = "Levier n° "+ CreatorTracker + " de la porte " + door.KeyCode;
            lever.GetComponent<Lever>().SetLever(door, CreatorTracker,door.KeyCode) ;
            EditorUtility.SetDirty(lever);
            door.AddLock(lever.GetComponent<Lever>());
            EditorUtility.SetDirty(door);
            
        }

        if (GUILayout.Button("Reset"))
        {
            List<Lever> Levers = door.ResetLevers();

            foreach (Lever obj in Levers)
            {
                if(obj != null)
                {
                    DestroyImmediate(obj.gameObject);
                }
                
            }
            CreatorTracker = 0;
            EditorUtility.SetDirty(door);

        }
    }

}
