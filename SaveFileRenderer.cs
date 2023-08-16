using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class SaveFileRenderer : MonoBehaviour
{

        public TextMeshPro textMesh;

        public List<SavedChallengeData> completedChallenge;

        public void RenderFileData (NewSaveDataTest pSaveFile)
        {
            textMesh.text = textMesh.text.Replace("<drinks>",pSaveFile.storedData.powerupDrinksConsumed.ToString());
            textMesh.text = textMesh.text.Replace("<score>", pSaveFile.storedData.lifeTimeScore.ToString());
            textMesh.text = textMesh.text.Replace("<kills> ", pSaveFile.storedData.lifeTimeKills.ToString());
          

            


            textMesh.text = textMesh.text.Replace("<completed>", pSaveFile.completedChallengesList.Count.ToString());


            textMesh.text = textMesh.text.Replace("<total>", pSaveFile.availableChallengesList.Count.ToString());


       
         }
}
