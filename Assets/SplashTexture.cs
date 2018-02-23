using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class SplashTexture : MonoBehaviour
{
    [SerializeField] private Texture2D _brushTexture;
    [SerializeField] private Color _brushColor = Color.black;
    private Material _material;
    private Texture2D _texture;

    private RaycastHit _hit;

    private void Awake()
    {
        _material = GetComponent<MeshRenderer>().material;

        _texture = new Texture2D(512, 512, TextureFormat.RGB24, false);
        Color[] colors = _texture.GetPixels();
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.black;
        }
        _texture.SetPixels(colors);
        _texture.Apply();

        _material.SetTexture("_MainTex", _texture);
        _material.SetTexture("_DispTex", _texture);
    }

    private void Update()
    {
        if (!Input.GetMouseButton(0))
            return;
        
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _hit))
        {
            UpdateSplashTexture();
        }
    }

    private void UpdateSplashTexture()
    {
        Vector2 uv = _hit.textureCoord;
        uv.x *= _texture.width;
        uv.y *= _texture.height;

        int centerX = (int)uv.x;
        int centerY = (int)uv.y;

        int minX = centerX - _brushTexture.width / 2;
        int maxX = centerX + _brushTexture.width / 2;
        int minY = centerY - _brushTexture.height / 2;
        int maxY = centerY + _brushTexture.height / 2;

        minX = Mathf.Clamp(minX, 0, _texture.width);
        maxX = Mathf.Clamp(maxX, 0, _texture.width);
        minY = Mathf.Clamp(minY, 0, _texture.height);
        maxY = Mathf.Clamp(maxY, 0, _texture.height);

        Color brushPixel = Color.white;

        Color[] textureColors = _texture.GetPixels();

        for (int x = minX; x < maxX; x++)
        {
            for (int y = minY; y < maxY; y++)
            {
                brushPixel = _brushTexture.GetPixel(x - centerX + _brushTexture.width / 2, y - centerY + _brushTexture.height / 2);

                if (brushPixel.a == 0)
                    continue;

                textureColors[y * _texture.width + x] += _brushColor;
            }
        }

        _texture.SetPixels(textureColors);

        _texture.Apply();
    }
}