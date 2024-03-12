using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Weapon))]
public class WeaponEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Weapon weapon = (Weapon)target;

        DrawDefaultInspector(); // Draw default inspector fields

        EditorGUILayout.Space(); // Add some spacing

        // Draw fields for Damage struct
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.LabelField("Damage Settings", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;

        weapon.damage.Amount = EditorGUILayout.FloatField("Amount", weapon.damage.Amount);
        weapon.damage.Knock = EditorGUILayout.FloatField("Knock", weapon.damage.Knock);
        weapon.damage.DamageType = (DamageTypes)EditorGUILayout.EnumPopup("Damage Type", weapon.damage.DamageType);

        EditorGUI.indentLevel--;
        EditorGUILayout.Space();

        if (EditorGUI.EndChangeCheck())
        {
            // If any changes were made, mark the object as dirty so changes are saved
            EditorUtility.SetDirty(target);
        }
    }
}