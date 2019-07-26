Shader "Space/PlanetShader" {

	Properties {
		_MainTex("Diffuse Texture", 2D) = "White" {}
//		_BumpMap("Normal Texture", 2D) = "Bump" {}
//		_BumpDepth ("Bump Depth", Range(-2.0, 2.0)) = 1.0
		_Color ("Color tint", Color) = (1.0, 1.0, 1.0, 1.0)
		_SpecColor ("Specular Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_Shininess ("Shininess", Float) = 10
		_RimColor ("Rim Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_RimPower ("Rim Power", Range(0.1, 10.0)) = 3.0
	}

	SubShader{
		Pass{
			
			Tags { "LightMode" = "ForwardBase"}

			CGPROGRAM
			
			//pragmas
			#pragma vertex vert
			#pragma fragment frag
			
			//user defined variables
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
//			uniform sampler2D _BumpMap;
//			uniform float4 _BumpMap_ST;
			uniform float4 _Color;
			uniform float4 _SpecColor;
			uniform float4 _RimColor;
			uniform float _Shininess;
			uniform float _RimPower;
			uniform float _BumpDepth;
			
			//Unity defined variables
			uniform float4 _LightColor0;
			
			//base input
			struct vertexInput{
			
				float4 vertex 	: POSITION;
				float3 normal 	: NORMAL;
				float4 texcoord : TEXCOORD0;
//				float4 tangent 	: TANGENT;
			};
			
			struct vertexOutput{
			
				float4 pos 			: SV_POSITION;
				float4 tex			: TEXCOORD0;
				float4 posWorld 	: TEXCOORD1;
				float3 normalWorld 	: TEXCOORD2;
//				float3 tangentWorld 	: TEXCOORD3;
//				float3 binormalWorld 	: TEXCOORD4;
			};
			
			//Vertex function
			vertexOutput vert(vertexInput v){
			
				vertexOutput o;
				
				o.normalWorld = mul(float4(v.normal, 0.0), _World2Object).xyz;
//				o.tangentWorld = mul(v.tangent, _Object2World).xyz;
//				o.binormalWorld = cross(o.normalWorld,o.tangentWorld) * v.tangent.w;
				
				o.posWorld =  mul(_Object2World, v.vertex);
				
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				
				o.tex = v.texcoord;
				return o;
			}
			
			//Fragment Function
			float4 frag(vertexOutput i) : COLOR 
			{
				//Vectors
				float3 normalDirection = normalize(i.normalWorld);
//				float3 tangentWorld = normalize(i.tangentWorld);
//				float3 binormalWorld = normalize(i.binormalWorld);
				float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz); 
				
				float3 lightDirection;
				float atten;
				
				if(_WorldSpaceLightPos0.w == 0.0)
				{
					atten = 1.0;
					lightDirection = normalize(_WorldSpaceLightPos0.xyz);				
					
				}
				else
				{
					float3 fragmentToLightSource = _WorldSpaceLightPos0.xyz - i.posWorld.xyz;
					float distance = length(fragmentToLightSource);
					atten = 1/distance;
					lightDirection = normalize(fragmentToLightSource);
				}
								
				//Textures
				float4 tex = tex2D(_MainTex, i.tex.xy * _MainTex_ST.xy + _MainTex_ST.zw);
//				float4 texN = tex2D(_BumpMap, i.tex.xy * _BumpMap_ST.xy + _BumpMap_ST.zw);
				
				//unpackNormal function
//				float3 localCoords = float3(2.0 * texN.ag - float2(1.0,1.0), 0.0);
//				localCoords.z = _BumpDepth;
				
				//normal transpose matrix
//				float3x3 local2WorldTranspose = float3x3(
//					tangentWorld,
//					binormalWorld,
//					normalWorld
//				);	
				
				//calculate normal direction
//				float3 normalDirection = normalize(mul(localCoords, local2WorldTranspose));
				
				//Lighting
				float3 diffuseReflection = atten * _LightColor0.xyz * _Color.xyz * saturate(dot(normalDirection, lightDirection));
				float3 specularReflection = atten * _LightColor0.xyz * _SpecColor.rgb * max( 0.0, dot(normalDirection, lightDirection)) * pow ( max(0.0, dot( reflect( -lightDirection, normalDirection), viewDirection)), _Shininess);
				
				//Rim
				float rim = 1 - saturate(dot(normalize(viewDirection), normalDirection));
				float3 rimLighting = atten * _LightColor0.xyz * _RimColor * saturate(dot(normalDirection, lightDirection)) * pow(rim, _RimPower);
				
				float3 lightFinal = rimLighting + diffuseReflection + specularReflection;
							
				return float4(tex.xyz * lightFinal * _Color.rgb , 1.0);
			}
			ENDCG
		}
	}
// Fallback "Diffuse"
}