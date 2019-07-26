Shader "Custom/TGShader-Global" {
	Properties {
		_MinCol ("Min", Color) = (0,0,0,0)
		_MaxCol ("Max", Color) = (0,0,0,0)
		_HMax ("Height Rati", float) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		float4 _MinCol;
		float4 _MaxCol;
		float _HMax;

		struct Input {
			float3 worldPos;
		};
		void surf (Input IN, inout SurfaceOutput o) {

			
			float t = clamp(IN.worldPos.y/_HMax, 0, 1);

			//Colour via Height
			fixed4 col = lerp(_MinCol, _MaxCol, t);
				
			o.Albedo = col;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
