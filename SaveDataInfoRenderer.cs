using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataInfoRenderer : MonoBehaviour
{

    public NewSaveDataTest newSaveDataModel;

    public void Start()
    {

        
        Invoke("RenderSaveDataInfo",3f);
    }

    public List<SaveFileRenderer> saveFileRenderers;

    public  void RenderSaveDataInfo() {



        //for(int i = 0; i < saveFileRenderers.Count; i++) {

            //SaveDataManager.instance.currentSaveFile

            saveFileRenderers[0].RenderFileData(newSaveDataModel);

        //}


    }
}


