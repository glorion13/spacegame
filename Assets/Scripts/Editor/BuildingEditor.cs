using UnityEditor;

[CustomEditor(typeof(Building))]
public class BuildingEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Building buildingObject = (Building) target;
        buildingObject.SetNewPosition(
            EditorGUILayout.IntField("Move X", buildingObject.X),
            EditorGUILayout.IntField("Move Y", buildingObject.Y)
            );

    }
}