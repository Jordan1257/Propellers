using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;

[CustomEditor(typeof(Propeller))]
public class PropellerEditor : Editor
{
    Propeller propeller;

    float currentRPM => propeller.CurrentRPM;

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new();
        
        propeller = (Propeller)target;

        SerializedProperty sp_fuel = serializedObject.FindProperty("fuel");
        PropertyField fuelField = new(sp_fuel);
        root.Add(fuelField);

        SerializedProperty sp_power = serializedObject.FindProperty("power");
        PropertyField powerField = new(sp_power);
        root.Add(powerField);

        SerializedProperty sp_weight = serializedObject.FindProperty("weight");
        Label weightLabel = new();
        // weightLabel.BindProperty(sp_weight);
        weightLabel.text = $"Weight: {sp_weight.floatValue}";
        root.Add(weightLabel);

        if(Application.isPlaying)
        {
            IMGUIContainer imguiContainer = new();
            imguiContainer.onGUIHandler = () =>
            {
                EditorGUILayout.LabelField("Current RPM: " + currentRPM);
            };
            root.Add(imguiContainer);
        }

        SerializedProperty sp_isRunning = serializedObject.FindProperty("isRunning");
        Button startStopButton = new();
        startStopButton.clicked += ()=>
        {            
            if(sp_weight.floatValue < 0.0001f)
            {
                ((Propeller)target).CalculateWeight();
            }
            sp_isRunning.boolValue = !sp_isRunning.boolValue;
            startStopButton.text = sp_isRunning.boolValue ? "Stop" : "Start";
            sp_isRunning.serializedObject.ApplyModifiedProperties();
        };
        startStopButton.text = sp_isRunning.boolValue ? "Stop" : "Start";
        root.Add(startStopButton);
        
        return root;
    }
}
