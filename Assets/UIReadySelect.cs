using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIReadySelect : MonoBehaviour
{
    private int m_SeatIndex;
    private bool readyFlg;
    public void Initialize(int seatIndex)
    {
        readyFlg = false;
    }

    // Called directly by Button in UI
    public void OnClicked()
    {
        Debug.Log("Ready�{�^���N���b�N");
        bool isReady = true;

        if(readyFlg)
        {
            isReady = false;
        }

        readyFlg = isReady;

        ClientCharSelectState.Instance.OnPlayerClickedReadyState(readyFlg);
    }

}
