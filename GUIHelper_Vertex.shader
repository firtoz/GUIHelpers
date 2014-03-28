Shader "GUI Helpers/GUI (Vertex)" {
Properties {
	_Color ("Tint (A = Opacity)", Color) = (1,1,1,1)
}

SubShader {
	Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
	LOD 100
	
	Cull Off
	ZWrite Off
	ZTest Always
	Blend SrcAlpha OneMinusSrcAlpha 
	
	Pass {  
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
                float4 color : COLOR;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
			};

			float4 _Color;
			
			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                o.color = v.color;
				return o;
			}
			
			fixed4 frag (v2f i) : COLOR
			{
				fixed4 col;
				return _Color * i.color;
			}
		ENDCG
	}
}

}