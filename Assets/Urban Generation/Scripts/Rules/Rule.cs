using UnityEngine;

[CreateAssetMenu(menuName ="ProceduralCity/Rule")]
public class Rule : ScriptableObject
{
    public string letter;
    [SerializeField] // Allow us to see its value in the editor without changing it directly
    private string[] results = null;

    public string GetResult()
    {
        return results[0];
    }

}