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
    }

    // 패스 달성 패킷
    public void PassRewardRequest(int step)
    {
        ServerRole.Instance.PassRewardProcess(step);
    }

    public void PassRewardResponse(PassPoint passpoint)
    {
        if (passpoint.errorcode == eErrorCode.TransactionError)
        {
            Debug.Log($"Error :: {passpoint.errorcode.ToString()}");
        }

        // 받은 패스포인트 처리할것.

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

    }

    // 미션 달성 패킷
    public void MissionClearRequest(int id)
    {
        ServerRole.Instance.MissionClearProcess(id);
    }

    public void MissionClearResponse(bool isClear)
    {
        // 받은 미션처리여부 처리할것.
    }

    public void ErrorResponse(eErrorCode code)
    {
        if (code == eErrorCode.TransactionError)
        {
            Debug.Log($"Error :: {code.ToString()}");
        }
    }
}
