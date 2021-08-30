using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputSOController : MonoBehaviour
{
    public FloatSO mhSO;
    public FloatSO mvSO;


   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RightDown() 
    {
        mhSO.value = 1f;
    
    }
    public void RightUp()
    {

        mhSO.value = 0f;
    }
    public void UpDown() 
    {
        mvSO.value = 1f;
    
    }
    public void UpUp()
    {

        mvSO.value = 0f;
    }
    public void DownDown() 
    {
        mvSO.value = -1f;
    
    }
    public void DownUp()
    {

        mvSO.value = 0f;
    }
    public void LeftDown() 
    {
        mhSO.value = -1f;
    
    }
    public void LeftUp()
    {

        mhSO.value = 0f;
    }
}
