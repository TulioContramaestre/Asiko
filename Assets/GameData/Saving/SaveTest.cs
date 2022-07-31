using UnityEngine;

public class SaveTest : MonoBehaviour
{
    public DataValues dv;

    private void Update()
    {
        // Save data
        if (Input.GetKeyDown(KeyCode.K))
        {
            SaveManager.Save(dv);
        }

        // Load data
        if (Input.GetKeyDown(KeyCode.L))
        {
            dv = SaveManager.Load();
        }
    }
}
