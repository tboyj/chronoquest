using UnityEngine;

public class StartBoneDust : MonoBehaviour

{
    public QuestManager questManager;
    [SerializeField]
    private BoneObjectCreator boneToaster;
    void Start()
    {
        boneToaster = transform.GetChild(0).GetComponent<BoneObjectCreator>();
        Debug.Log("YES");
        gameObject.SetActive(true);
    }
    public void Run()
    {
        foreach (Transform child in transform)
        {
            Debug.Log("Index " + child.GetComponentIndex() + ": " + child.gameObject.name);
        }
        gameObject.SetActive(true);
        
        
        Debug.Log("Hi!");
        questManager.GetComponent<PauseScript>().activateTimeFreeze();
        System.Random randomLimit = new System.Random();
        int max = randomLimit.Next(2, 4);
        for (int i = 0; i < max; i++)
        {
            boneToaster.GenerateNewObject();
        }
    }
}