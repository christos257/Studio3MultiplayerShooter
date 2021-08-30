using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputSOController : MonoBehaviour
{
    public FloatSO mhSO;
    public FloatSO mvSO;
    public BoolSO moveBO;
    public BoolSO spinBO;


   
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
        moveBO.value = true;
    }
    public void RightUp()
    {
        moveBO.value = false;
        mhSO.value = 0f;
    }
    public void UpDown() 
    {
        mvSO.value = 1f;
        moveBO.value = true;
    }
    public void UpUp()
    {
        moveBO.value = false;
        mvSO.value = 0f;
    }
    public void DownDown() 
    {
        mvSO.value = -1f;
        moveBO.value = true;
    }
    public void DownUp()
    {
        moveBO.value = false;
        mvSO.value = 0f;
    }
    public void LeftDown() 
    {
        mhSO.value = -1f;
        moveBO.value = true;
    }
    public void LeftUp()
    {
        moveBO.value = false;
        mhSO.value = 0f;
    }  
    public void SpinDown() 
    {

        spinBO.value = true;
    }
    public void SpinUp()
    {
        spinBO.value = false;

    }
}
