using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPicker : MonoBehaviour
{
    [SerializeField] PartyInfoBlock pInfoBlock;
    [SerializeField] CharaStatBlock chara;
    [SerializeField] QuestBoardObject questBlock;
    [SerializeField] Wallet wallet;
    
    public void PickCharacter()
    {
        pInfoBlock.members.Clear();
        pInfoBlock.inactives.Clear();
        pInfoBlock.AddMember(chara);
        questBlock.ResetAllQuests();
        wallet.Reset();
    }

}
