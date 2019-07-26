 Shader "Custom/SkyShader" {
 
	Properties{
	
		_Colour ("Colour", Color) = (0,0,1,1)
	}
	
	SubShader {
       
       Tags { "RenderType" = "Opaque" }
       
       Cull Front
       
       CGPROGRAM
       
       #pragma surface surf Lambert vertex:vert
       
       void vert(inout appdata_full v)
       {
           v.normal.xyz = v.normal * -1;
       }
       
		struct Input {
           float4 color : COLOR;
       	};
       	
       void surf (Input IN, inout SurfaceOutput o) {
           
           o.Albedo = 0.4;
       }
       
       ENDCG
       
     }
     
     Fallback "Diffuse"
     
   }