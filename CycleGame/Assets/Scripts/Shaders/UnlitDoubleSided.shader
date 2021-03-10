Shader "Tutorial/006_Basic_Transparency"{
	Properties{
		_Color ("Tint", Color) = (0, 0, 0, 1)
		_MainTex ("Texture", 2D) = "white" {}
	}

	SubShader{
		Tags{ "RenderType"="Transparent" "Queue"="Transparent" "DisableBatching" = "True"}

		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite off
        	Cull off


		Pass{
			CGPROGRAM

			#include "UnityCG.cginc"

			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing

			UNITY_INSTANCING_BUFFER_START(Props)

			UNITY_DEFINE_INSTANCED_PROP(float4, _MainTex_ST)
			UNITY_DEFINE_INSTANCED_PROP(float4, _Color)

			UNITY_INSTANCING_BUFFER_END(Props)

			sampler2D _MainTex;

			struct appdata{
				UNITY_VERTEX_INPUT_INSTANCE_ID
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f{
				UNITY_VERTEX_INPUT_INSTANCE_ID
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f vert(appdata v){
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				o.position = UnityObjectToClipPos(v.vertex);
				float4 scaleTrans = UNITY_ACCESS_INSTANCED_PROP(Props, _MainTex_ST);
				o.uv = v.uv * scaleTrans.xy + scaleTrans.zw;
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				return o;
			}

			fixed4 frag(v2f i) : SV_TARGET{
				UNITY_SETUP_INSTANCE_ID(i);
				fixed4 col = tex2D(_MainTex, i.uv);
				col *=  UNITY_ACCESS_INSTANCED_PROP(Props, _Color);
				return col;
			}

			ENDCG
		}
	}
}