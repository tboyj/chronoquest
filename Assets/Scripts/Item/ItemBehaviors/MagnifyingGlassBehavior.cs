
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(menuName = "Item/MagnifyingGlass")]
public class MagnifyingGlass : ItemUseEffect
{
    public override void Apply(GameObject target)
    {
        Debug.Log("We hearest" + target.name);
        if (target.GetComponent<BaseUse>() != null)
        {
            Debug.Log("I got ze papers.");
            if (target.GetComponent<BaseUse>() is Paper)
            {
                target.GetComponent<Paper>().Use(); // GO!
            }
            
        }
    }
    public override void ApplyAlone()
    {
        Debug.Log("We loneliest");
    }
}
