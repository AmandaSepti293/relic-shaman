using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class cameraManager : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera[] allvirtualCamera;
    private CinemachineVirtualCamera currentCamera;
    private CinemachineFramingTransposer farmingTransposer;
    [Header("Y Damping Settings : ")]
    [SerializeField] private float panAmount = 0.1f;
    [SerializeField] private float panTime = 0.2f;
    public float playerFallSpeedTreshold = -10;
    public bool isLerpingYDamp;
    private float normalYDamp;
    public bool hasLerpingYDamping;
    
    public static cameraManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        for(int i = 0; i < allvirtualCamera.Length; i++)
        {
            if(allvirtualCamera[i].enabled)
            {
                currentCamera = allvirtualCamera[i];
                farmingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            }
        }

        normalYDamp = farmingTransposer.m_YDamping;
    }

    private void Start()
    {
        for(int i = 0; i < allvirtualCamera.Length; i++)
        {
            allvirtualCamera[i].Follow = PlayerController.Instance.transform;
        }    
    }

    public void SwapCamera(CinemachineVirtualCamera _newcam)
    {
        currentCamera.enabled = false;
        currentCamera = _newcam;
        currentCamera.enabled = true;
    }

    public IEnumerator LerpYDamping(bool _isPlayerFalling)
    {
        isLerpingYDamp = true;

        float _startYDamp = farmingTransposer.m_YDamping;
        float _endYDamp = 0;

        if(_isPlayerFalling)
        {
            _endYDamp = panAmount;
            hasLerpingYDamping = true;

        }else
        {
            _endYDamp = normalYDamp;
        }

        float _timer = 0;
        while (_timer < panAmount)
        {
            _timer += Time.deltaTime;
            float _lerpedPanAmount = Mathf.Lerp(_startYDamp, _endYDamp, (_timer / panTime));
            farmingTransposer.m_YDamping = _lerpedPanAmount;
            yield return null;
        }
        isLerpingYDamp = false;
    }
}
