using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Singleton;

public class PacketManager : MonoSingleton<PacketManager>
{
    // 로그인 패킷
    public void LoginRequest()
    {
        ServerRole.Instance.LoginProcess();
    }

    public void LoginResponse(PassPoint passpoint)
    {
        if(passpoint.errorcode == eErrorCode.TransactionError)
        {
            Debug.Log($"Error :: {passpoint.errorcode.ToString()}");
        }

        // 받은 패스포인트 처리할것.

        // A -- Myinfo에 받아온 패스정보 저장
        A_PassInfo.Instance.SetPassInfo(passpoint);
    }

    // 패스 달성 패킷
    public void PassRewardRequest(int step, bool isPremium)
    {
        ServerRole.Instance.PassRewardProcess(step, isPremium);
    }

    public void PassRewardResponse(PassPoint passpoint)
    {
        if (passpoint.errorcode == eErrorCode.TransactionError)
        {
            Debug.Log($"Error :: {passpoint.errorcode.ToString()}");
        }

        // 받은 패스포인트 처리할것.


        // A -- Myinfo에 받아온 패스정보 저장 및 브로드캐스팅처리
        A_PassInfo.Instance.SetPassInfo(passpoint);
        A_PassInfo.Instance.BroadCastEvent(A_PassInfo.Instance.PASS_EVENT_NAME);
    }

    // 패스 포인트 획득 패킷
    public void PassPointRequest(int point)
    {
        ServerRole.Instance.GetPassPointProcess(point);
    }

    public void PassPointResponse(PassPoint passpoint)
    {
        if (passpoint.errorcode == eErrorCode.TransactionError)
        {
            Debug.Log($"Error :: {passpoint.errorcode.ToString()}");
        }

        // 받은 패스포인트 처리할것.


        // A -- Myinfo에 받아온 패스정보 저장 및 브로드캐스팅처리
        A_PassInfo.Instance.SetPassInfo(passpoint);
        A_PassInfo.Instance.BroadCastEvent(A_PassInfo.Instance.PASS_EVENT_NAME);
    }

    // 미션 달성 패킷
    public void MissionClearRequest(int id)
    {
        ServerRole.Instance.MissionClearProcess(id);
    }

    public void MissionClearResponse(bool isClear)
    {
        if(isClear == true)
        {
            // 받은 미션처리여부 처리할것.
            A_PassInfo.Instance.BroadCastEvent(A_PassInfo.Instance.PASS_EVENT_NAME);
        }
    }

    public void ErrorResponse(eErrorCode code)
    {
        if (code == eErrorCode.TransactionError)
        {
            Debug.Log($"Error :: {code.ToString()}");
        }
    }

    public void BuyPremiumRequest()
    {
        ServerRole.Instance.PurchasePremium();
    }

    public void BuyPremiumResponse(PassPoint passpoint)
    {
        // A -- Myinfo에 받아온 패스정보 저장 및 브로드캐스팅처리
        A_PassInfo.Instance.SetPassInfo(passpoint);
        A_PassInfo.Instance.BroadCastEvent(A_PassInfo.Instance.PASS_EVENT_NAME);
    }

    public void BuyLevelRequest()
    {
        ServerRole.Instance.PurchaseLevel();
    }

    public void BuyLevelResponse(PassPoint passpoint)
    {
        // A -- Myinfo에 받아온 패스정보 저장 및 브로드캐스팅처리
        A_PassInfo.Instance.SetPassInfo(passpoint);
        A_PassInfo.Instance.BroadCastEvent(A_PassInfo.Instance.PASS_EVENT_NAME);
    }
}
