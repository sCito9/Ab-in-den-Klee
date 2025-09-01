using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    [SerializeField]private AudioSource source;
   
   
    public void OnAccelerate()
    {
        source.pitch = (source.pitch + 0.1f)  ;
        if(source.pitch >= 3)
        {
            source.pitch = 3;
        }
    }

    public void OnRelease()
    {
        
        source.pitch = 1;
        
        

    }
   

    public void OnBreak()
    {
        source.pitch = (source.pitch - 0.1f) ;
        if (source.pitch <= 1)
        {
            source.pitch = 1;
        }
    }


}
