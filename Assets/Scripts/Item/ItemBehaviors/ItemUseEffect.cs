using System;
using UnityEngine;


    [CreateAssetMenu(menuName = "Item/UseEffect")]
    public class ItemUseEffect : ScriptableObject
{    
            
        public virtual void Apply(GameObject target)
        {
            
        }

        public virtual void ApplyAlone()
        {
            throw new NotImplementedException();
        }
    }
