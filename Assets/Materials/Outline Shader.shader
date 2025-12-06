// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Test2"
{
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
	}
	SubShader {
		 ZTest LEqual
    
		 Stencil
		 {
			   Ref 1
			   Comp notequal
		  }

		Tags { "RenderType"="Opaque" }
		LOD 200
		
		Pass {
			Tags { "LightMode" = "Always" }
			
			Fog { Mode Off }
			ZWrite On
			ZTest LEqual
			Cull Off
			Lighting Off
	
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				
				fixed4 _Color;
				
				struct appdata {
					float4 vertex : POSITION;
				};
				
				struct v2f {
					float4 vertex : POSITION;
				};
				
				v2f vert (appdata v) {
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					return o;
				}
				
				fixed4 frag (v2f i) : COLOR {
					return _Color;
				}
			ENDCG
	
		}
	} 
}