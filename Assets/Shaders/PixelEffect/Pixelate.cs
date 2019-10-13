using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class Pixelate : MonoBehaviour
{
    [SerializeField] private Material _imageEffectMaterial;
    [SerializeField] private int _targetWidth = 450;
    [SerializeField] private CameraEvent _camEvent;

    private RenderTexture _renderTexture;

    private void Start()
    {
        CommandBuffer commandBuffer = new CommandBuffer();
        commandBuffer.name = "Pixelate";

        int downsample =(int)(CameraFollow.MainCamera.pixelWidth / _targetWidth);

        //check if modulus of downsample rate is a multiple of two, if not, add 1
        if (downsample % 2 != 0)
        {
            downsample++;
        }

        if (downsample <= 2)
        {
            downsample = 4;
        }
        
        _renderTexture = new RenderTexture((int)(CameraFollow.MainCamera.pixelWidth / downsample), (int)(CameraFollow.MainCamera.pixelHeight / downsample), 0);
        _renderTexture.filterMode = FilterMode.Point;
        RenderTargetIdentifier rtID = new RenderTargetIdentifier(_renderTexture);

        RenderTexture screenCopy = screenCopy = new RenderTexture(CameraFollow.MainCamera.pixelWidth, CameraFollow.MainCamera.pixelHeight, 0);
        screenCopy.filterMode = FilterMode.Point;
        
        commandBuffer.SetRenderTarget(rtID);
        commandBuffer.ClearRenderTarget(true, true, new Color(0, 0, 0, 0), 1f);

        commandBuffer.Blit(BuiltinRenderTextureType.CameraTarget, screenCopy);
        commandBuffer.Blit(screenCopy, rtID, _imageEffectMaterial);

        CameraFollow.MainCamera.AddCommandBuffer(_camEvent, commandBuffer);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(_renderTexture, destination);
    }
}
