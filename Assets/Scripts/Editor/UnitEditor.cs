using UnityEditor;

[CustomEditor(typeof(Unit))]
public class UnitEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Unit unitObject = (Unit)target;
        unitObject.SetNewPosition(
            EditorGUILayout.IntField("Move X", unitObject.X),
            EditorGUILayout.IntField("Move Y", unitObject.Y)
            );

    }
}