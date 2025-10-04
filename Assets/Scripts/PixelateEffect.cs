using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class PixelateEffect : MonoBehaviour
{
    public Shader pixelateShader;
    [Range(64,1024)]
    public int pixelSize = 256;
    [Range(2,64)]
    public float colorLevels = 64f;

    private Material _material;

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (pixelateShader == null)
        {
            Graphics.Blit(src, dest);
            return;
        }

        if (_material == null)
            _material = new Material(pixelateShader);

        _material.SetFloat("_PixelSize", pixelSize);
        _material.SetFloat("_Levels", colorLevels); // <- explicitly pass the value
        Graphics.Blit(src, dest, _material);
    }
}
