using UnityEngine;

[CreateAssetMenu(menuName ="ProceduralCity/Rule")]
public class Rule : ScriptableObject
{
    public string letter;
    [SerializeField] // Allow us to see its value in the editor without changing it directly
    private string[] results = null;
    [SerializeField]
    private bool randomResult = false;

    public string GetResult()
    {
        if(randomResult){
            int randomIndex = UnityEngine.Random.Range(0, results.Length);
            return results[randomIndex];
        }
        return results[0];
    }

}