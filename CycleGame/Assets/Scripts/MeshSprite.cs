using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class MeshSprite : MonoBehaviour
{

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
            Block.SetTexture("_MainTex", Sprite.texture);
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
}
