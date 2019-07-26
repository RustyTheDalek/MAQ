Shader "Custom/TGShader-Local" {
	Properties {
		_MinCol ("Min", Color) = (0,0,0,0)
		_MaxCol ("Max", Color) = (0,0,0,0)
		_HMax ("Height Ratio", float) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert vertex:vert

		float4 _MinCol;
		float4 _MaxCol;
		float _HMax;

		struct Input {
			float3 localPos;
		};
		
		void vert (inout appdata_full v, out Input o) {
		
			UNITY_INITIALIZE_OUTPUT(Input,o);
			o.localPos = v.vertex.xyz;
		 }

		void surf (Input IN, inout SurfaceOutput o) {

			//Colour via Height

			float3 temp = IN.localPos * o.Normal;
			
			float t = clamp(IN.localPos.y/_HMax, 0, 1);

			fixed4 col = lerp(_MinCol, _MaxCol, t);
				
			o.Albedo = col;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
