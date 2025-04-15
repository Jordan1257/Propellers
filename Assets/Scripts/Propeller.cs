using UnityEngine;


[ExecuteInEditMode]
public class Propeller : MonoBehaviour
{
    protected float currentRPM;
    public float CurrentRPM => currentRPM;

    [SerializeField]
    float fuel, power, weight;

    [SerializeField]
    bool isRunning;

    const float BLADE_MASS = 0.1f;
    const float FRICTION = 0.05f;
    const float MAX_RPM_MULTIPLIER = 100f; // Maximum RPM per unit of power-to-weight ratio
    const float ACCELERATION_RATE = 0.5f; // How quickly the RPM changes

    void Start()
    {
        CalculateWeight();
    }
    
    public void CalculateWeight()
    {
        weight = 0;
        Renderer[] renderers = this.gameObject.GetComponentsInChildren<Renderer>();
        for(int i = 0; i < renderers.Length; i++)
        {
            weight += (BLADE_MASS * renderers[i].bounds.size.magnitude);
        }
    }

    float CalculateMaxRPM()
    {
        // Calculate power-to-weight ratio and multiply by our constant
        // Add a small minimum weight to prevent division by zero
        float powerToWeight = power / (weight + 0.1f);
        return powerToWeight * MAX_RPM_MULTIPLIER;
    }

    void Update()
    {
        if(fuel > 0 && isRunning)
        {
            float maxRPM = CalculateMaxRPM();
            
            // Gradually approach the target RPM
            float targetRPM = maxRPM;
            currentRPM = Mathf.Lerp(currentRPM, targetRPM, ACCELERATION_RATE * Time.deltaTime);
            
            // Apply rotation based on current RPM
            this.transform.Rotate(currentRPM * Vector3.up * Time.deltaTime);
            fuel -= power * ACCELERATION_RATE * Time.deltaTime;
        }
        else if(isRunning)
        {
            fuel = 0;
            isRunning = false;
        }
        else if(currentRPM > 0.01f)
        {
            // Gradually slow down when not running
            currentRPM = Mathf.Lerp(currentRPM, 0, FRICTION * Time.deltaTime);
            this.transform.Rotate(currentRPM * Vector3.up * Time.deltaTime);
        }
    }
}
