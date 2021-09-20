using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class MeshSprite : MonoBehaviour
{
    //first int shader/base mat, zweiter int texture, dritter int how often used
    static Dictionary<(int material, int texture), (Material material, int uses)> MaterialPool = new Dictionary<(int, int), (Material, int)>();
    public Material BaseMaterial;
    private Texture2D CurrentTexture;
    public Sprite Sprite;

    public float Height = -1;

    private MaterialPropertyBlock _Block;
    MaterialPropertyBlock Block 
    {
        get
        {
            if(_Block == null)
                _Block = new MaterialPropertyBlock();
            return _Block;
        }
    }
    
    private MeshRenderer _Mesh;
    MeshRenderer Mesh
    {
        get
        {
            if(_Mesh == null)
                _Mesh = GetComponent<MeshRenderer>();
            return _Mesh;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateMPB();
    }

    private void OnValidate() 
    {
        UpdateMPB();    
    }

    public void UpdateMPB()
    {
        if(Sprite)
        {

            var material = GetMaterial(Sprite.texture);
            Mesh.sharedMaterial = material;
            Vector2 tiling = Sprite.rect.size * Sprite.texture.texelSize;
            Vector2 offset = Sprite.rect.position * Sprite.texture.texelSize;
            Block.SetVector("_MainTex_ST", new Vector4(tiling.x, tiling.y, offset.x, offset.y));
        }
        Mesh.SetPropertyBlock(Block);
        if(Height > 0)
        {
            float aspect = Sprite.rect.width/Sprite.rect.height;
            transform.localScale = new Vector3(aspect*Height, Height, 1);
        }
    }
    
    Material GetMaterial(Texture2D texture)
    {
        if(CurrentTexture != null && CurrentTexture != texture)
        {
            ReleaseMaterial(texture);
        }
        var key = (BaseMaterial.GetInstanceID(), texture.GetInstanceID());
        if(!MaterialPool.TryGetValue(key, out var materialPair) || materialPair.material == null)
        {
            materialPair.material = new Material(BaseMaterial);
            materialPair.uses = 0;
        }
        materialPair.uses++;
        MaterialPool[key] = materialPair;
        materialPair.material.SetTexture("_MainTex", texture);

        CurrentTexture = texture;

        return materialPair.material;
    }

    void ReleaseMaterial(Texture2D texture)
    {
        var key = (BaseMaterial.GetInstanceID(), texture.GetInstanceID());
        if(MaterialPool.TryGetValue(key, out var materialPair))
        {
            materialPair.uses--;
            if(materialPair.uses <= 0)
            {
                DestroyImmediate(materialPair.material);
                MaterialPool.Remove(key);
                return;
            }
            MaterialPool[key] = materialPair;
        }
    }
}
