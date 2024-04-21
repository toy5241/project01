using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICharSelectPlayerSeat : MonoBehaviour
{
    private int m_SeatIndex;

    // playerNumber of who is sitting in this seat right now. 0-based; e.g. this is 0 for Player 1, 1 for Player 2, etc. Meaningless when m_State is Inactive (and in that case it is set to -1 for clarity)
    private int m_PlayerNumber;
    private bool m_IsDisabled;
    public void Initialize(int seatIndex)
    {
        m_SeatIndex = seatIndex;
        m_PlayerNumber = -1;
    }

    // Called directly by Button in UI
    public void OnClicked()
    {
        Debug.Log("クリックされている？？");
        ClientCharSelectState.Instance.OnPlayerClickedSeat(m_SeatIndex);
    }

}
