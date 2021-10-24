Shader "Custom/Hull_Outline"{
	//show values to edit in inspector
	Properties{

        _OutlineColor ("Outline Color", Color) = (0, 0, 0, 1)
        _OutlineThickness ("Outline Thickness", Range(0, .1)) = 0.03

		_Color ("Tint", Color) = (0, 0, 0, 1)
		_MainTex ("Texture", 2D) = "white" {}
	}

	SubShader{
		//the material is completely non-transparent and is rendered at the same time as the other opaque geometry
		Tags{ "RenderType"="Opaque" "Queue"="Geometry" }

        Pass{
			CGPROGRAM

			//include useful shader functions
			#include "UnityCG.cginc"

			//define vertex and fragment shader functions
			#pragma vertex vert
			#pragma fragment frag

			//texture and transforms of the texture
			sampler2D _MainTex;
			float4 _MainTex_ST;

			//tint of the texture
			fixed4 _Color;

			//the mesh data thats read by the vertex shader
			struct appdata{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			//the data thats passed from the vertex to the fragment shader and interpolated by the rasterizer
			struct v2f {
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			//the vertex shader function
			v2f vert(appdata v){
				v2f o;
				//convert the vertex positions from object space to clip space so they can be rendered correctly
				o.position = UnityObjectToClipPos(v.vertex);
				//apply the texture transforms to the UV coordinates and pass them to the v2f struct
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			//the fragment shader function
			fixed3 frag(v2f i) : SV_TARGET{
			    //read the texture color at the uv coordinate
				fixed3 col = tex2D(_MainTex, i.uv);
				//multiply the texture color and tint color
				col *= _Color;
				//return the final color to be drawn on screen
				return col;
			}
			
			ENDCG
		}


		Pass{
            Cull Front
			CGPROGRAM

			//include useful shader functions
			#include "UnityCG.cginc"

			//define vertex and fragment shader functions
			#pragma vertex vert
			#pragma fragment frag

			//texture and transforms of the texture
			sampler2D _MainTex;
			float4 _MainTex_ST;

			//tint of the texture
			fixed4 _Color;

            //Color of the outline
            fixed4 _OutlineColor;

            //thickness of the outline
            float _OutlineThickness;

			//the mesh data thats read by the vertex shader
			struct appdata{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			//the data thats passed from the vertex to the fragment shader and interpolated by the rasterizer
			struct v2f {
				float4 position : SV_POSITION;
			};

			//the vertex shader function
			v2f vert(appdata v){
				v2f o;
                //calculate the pos of the expanded object
                float3 normal = normalize(v.normal);
                float3 outlineOffset = normal * _OutlineThickness;
                float3 position = v.vertex + outlineOffset;

				//convert the vertex positions from object space to clip space so they can be rendered correctly
				o.position = UnityObjectToClipPos(position);
				return o;
			}

			//the fragment shader function
			fixed4 frag(v2f i) : SV_TARGET{
				return _OutlineColor;
			}
			
			ENDCG
		}
	}
	Fallback "VertexLit"
}