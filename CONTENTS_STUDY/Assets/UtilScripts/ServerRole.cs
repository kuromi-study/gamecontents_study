using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Singleton;

public class ServerRole : MonoSingleton<ServerRole>
{
    PassPoint _passPoint;

    public void LoginProcess()
    {
        // 패스 포인트 및 달성도를 설정합니다.
        InitPass();
        PacketManager.Instance.LoginResponse(_passPoint);
    }

    public void PassRewardProcess(int step)
    {
        // 패스포인트 보상을 처리합니다.
        if(_passPoint == null)
        {
            PacketManager.Instance.ErrorResponse(eErrorCode.ValueError);
            return;
        }

        SetPassStep(step);
        PacketManager.Instance.PassRewardResponse(_passPoint);
    }

    public void GetPassPointProcess(int point)
    {
        // 패스포인트 획득을 처리합니다.
        if(_passPoint == null)
        {
            PacketManager.Instance.ErrorResponse(eErrorCode.ValueError);
            return;
        }

        SetPassPoint(point);
        PacketManager.Instance.PassPointResponse(_passPoint);
    }

    public void MissionClearProcess(int id)
    {
        // 미션 클리어 여부를 설정합니다.
        bool isClear = false;

        var forErrorCode = Random.Range(0, 10);
        switch(forErrorCode)
        {
            case 5:
                PacketManager.Instance.ErrorResponse(eErrorCode.ValueError);
                return;
            case 9:
                PacketManager.Instance.ErrorResponse(eErrorCode.ValueError);
                return;
            case 10:
                isClear = false;
                break;
            default:
                isClear = true;
                break;

        }

        PacketManager.Instance.MissionClearResponse(isClear);
    }

    #region PassSet
    void InitPass()
    {
        if(_passPoint == null)
        {
            _passPoint = new PassPoint();
        }

        // 임의로 1/10확률로 Fail발생
        var forErrorCode = Random.Range(0, 10);
        forErrorCode = forErrorCode / 10;
        if(forErrorCode == 1)
        {
            _passPoint.errorcode = eErrorCode.TransactionError;
            return;
        }

        var randomPoint = Random.Range(0, 500);
        var randomStep = Random.Range(0, randomPoint / 100);

        _passPoint.Point = randomPoint;
        _passPoint.Step = randomStep;
        _passPoint.errorcode = eErrorCode.Success;
    }

    void SetPassStep(int step)
    {
        // 임의로 1/10확률로 Fail발생
        var forErrorCode = Random.Range(0, 10);
        forErrorCode = forErrorCode / 10;
        if (forErrorCode == 1)
        {
            _passPoint.errorcode = eErrorCode.TransactionError;
            return;
        }

        if(_passPoint.Step > step)
        {
            // 이미 클리어한 스텝입니다.
            _passPoint.errorcode = eErrorCode.ValueError;
            return;
        }

        _passPoint.Step = step;
        _passPoint.errorcode = eErrorCode.Success;
    }

    void SetPassPoint(int point)
    {
        // 임의로 1/10확률로 Fail발생
        var forErrorCode = Random.Range(0, 10);
        forErrorCode = forErrorCode / 10;
        if (forErrorCode == 1)
        {
            _passPoint.errorcode = eErrorCode.TransactionError;
            return;
        }

        _passPoint.Point += point;
        _passPoint.errorcode = eErrorCode.Success;
    }
    #endregion
}

#region PacketClass

public class PassPoint
{
    public int Point { get => _point; set => _point = value; }
    public int Step { get => _step; set => _step = value; }
    public eErrorCode errorcode;

    int _point;
    int _step;
}

public enum eErrorCode
{
    Success = 0,
    TransactionError,
    ValueError,
}
#endregion