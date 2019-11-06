using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class Pixelate : MonoBehaviour
{
    [SerializeField] private Material _imageEffectMaterial;
    [SerializeField] private int downsample = 4;
    [SerializeField] private CameraEvent _camEvent;

    private RenderTexture _renderTexture;

    private void Start()
    {
        CommandBuffer commandBuffer = new CommandBuffer {name = "Pixelate"};

        //int downsample = CameraManager.CurrentCamera.pixelWidth / _targetWidth;



        int pixelWidth = CameraManager.CurrentCamera.pixelWidth;
        int pixelHeight = CameraFollow.MainCamera.pixelHeight;

        _renderTexture = new RenderTexture(pixelWidth / downsample, pixelHeight / downsample, 0)
        {
            filterMode = FilterMode.Point
        };
        RenderTargetIdentifier rtID = new RenderTargetIdentifier(_renderTexture);

        RenderTexture screenCopy = new RenderTexture(pixelWidth, pixelHeight, 0) {filterMode = FilterMode.Point};

        commandBuffer.SetRenderTarget(rtID);
        commandBuffer.ClearRenderTarget(true, true, new Color(0, 0, 0, 0), 1f);

        commandBuffer.Blit(BuiltinRenderTextureType.CameraTarget, screenCopy);
        commandBuffer.Blit(screenCopy, rtID, _imageEffectMaterial);

        CameraManager.CurrentCamera.AddCommandBuffer(_camEvent, commandBuffer);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(_renderTexture, destination);
    }
}
