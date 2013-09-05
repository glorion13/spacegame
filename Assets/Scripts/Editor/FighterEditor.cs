using UnityEditor;

[CustomEditor(typeof(Fighter))]
public class FighterEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Fighter unitObject = (Fighter)target;
        unitObject.SetNewPosition(
            EditorGUILayout.IntField("Move X", unitObject.X),
            EditorGUILayout.IntField("Move Y", unitObject.Y)
            );

    }
}