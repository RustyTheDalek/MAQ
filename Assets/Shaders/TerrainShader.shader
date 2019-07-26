Shader "Custom/TerrainShader" {

	SubShader{
	
		Pass{
		
			Tags { "LightMode" = "ForwardBase"}
			
			CGPROGRAM
			
			//pragmas
			#pragma vertex vert
			#pragma fragment frag
			
			//Unity defined variables
			uniform float4 _LightColor0;
			uniform float4x4 _LightMatrix0;
			uniform sampler2D _LightTexture0; 

			//base input
			
			struct vertexInput{
			
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				
			};
			
			struct vertexOutput{
			
				float4 pos 			: SV_POSITION;
				float4 posWorld 	: TEXCOORD0;
			 	float4 posLight 	: TEXCOORD1;
				float3 normalDir	: TEXCOORD2;
				float4 col 			: COLOR;
			};
			
			//Vertex Function
			vertexOutput vert(vertexInput v){
			
				vertexOutput o;
				
				o.posWorld = mul(_Object2World, v.vertex);
				o.posLight = mul(_LightMatrix0, o.posWorld);
				o.normalDir = normalize(mul(float4(v.normal, 0.0), _World2Object).xyz);

				//Colour via Height
				float g = (((v.vertex.y*0.47)/10)+0.53);
				float b = (((v.vertex.y*-0.42)/10)+0.58);
				
				//Other Values
//				 float g = (((input.posWorld.y*0.78)/10)+0.22);
//				 float b = (((input.posWorld.y*-1)/10)+1);
				
				o.col = float4(0,g,b, 1.0);
												
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
								
				return o;
			}
			
			float4 frag(vertexOutput i) : COLOR {
			
				//Vectors
				float3 normalDirection = normalize(i.normalDir);
				
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
				
				//Colouring via height
//				float g = (((i.posWorld.y*0.47)/10)+0.53);
//				float b = (((i.posWorld.y*-0.42)/10)+0.58);
				
//				i.col = float4(0, g, b, 1.0);
				//Lighting
				
				float3 diffuseReflection = UNITY_LIGHTMODEL_AMBIENT + atten * i.col * _LightColor0.xyz * saturate(dot(normalDirection, lightDirection));

				i.col = float4(diffuseReflection, 1.0);
				
				return i.col;
			}
			ENDCG
		}
		
		Pass{
		
			Tags { "LightMode" = "ForwardAdd"}
			
			Blend One One
			
			CGPROGRAM
			
			//pragmas
			#pragma vertex vert
			#pragma fragment frag
			
			//Unity defined variables
			uniform float4 _LightColor0;
			uniform float4x4 _LightMatrix0;
			uniform sampler2D _LightTexture0; 

			//base input
			
			struct vertexInput{
			
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				
			};
			
			struct vertexOutput{
			
				float4 pos 			: SV_POSITION;
				float4 posWorld 	: TEXCOORD0;
			 	float4 posLight 	: TEXCOORD1;
				float3 normalDir	: TEXCOORD2;
				float4 col 			: COLOR;
			};
			
			//Vertex Function
			vertexOutput vert(vertexInput v){
			
				vertexOutput o;
				
				o.posWorld = mul(_Object2World, v.vertex);
				o.posLight = mul(_LightMatrix0, o.posWorld);
				o.normalDir = normalize(mul(float4(v.normal, 0.0), _World2Object).xyz);

				//Colour via Height
				float g = (((v.vertex.y*0.47)/10)+0.53);
				float b = (((v.vertex.y*-0.42)/10)+0.58);
				
				//Other Values
//				 float g = (((input.posWorld.y*0.78)/10)+0.22);
//				 float b = (((input.posWorld.y*-1)/10)+1);
				
				o.col = float4(0,g,b, 1.0);
												
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
								
				return o;
			}
			
			float4 frag(vertexOutput i) : COLOR {
			
				//Vectors
				float3 normalDirection = normalize(i.normalDir);
				
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
				
				//Colouring via height
//				float g = (((i.posWorld.y*0.47)/10)+0.53);
//				float b = (((i.posWorld.y*-0.42)/10)+0.58);
				
//				i.col = float4(0, g, b, 1.0);
				//Lighting
				
				float cookieAttenuation = 1.0;
	            if (0.0 == _WorldSpaceLightPos0.w) // directional light?
	            {
	               cookieAttenuation = tex2D(_LightTexture0, 
	                  i.posLight.xy).a;
	            }
	            else if (1.0 != _LightMatrix0[3][3]) 
	               // spotlight (i.e. not a point light)?
	            {
	               cookieAttenuation = tex2D(_LightTexture0, 
	                  i.posLight.xy / i.posLight.w 
	                  + float2(0.5, 0.5)).a;
	            }
				
				
				float3 diffuseReflection = atten * i.col * _LightColor0.xyz * saturate(dot(normalDirection, lightDirection));

				i.col = float4(cookieAttenuation * diffuseReflection, 1.0);
				
				return i.col;
			}
			ENDCG
		}
}
// Fallback "Diffuse"
}
// fixed g = (((IN.worldPos.y*0.29)/20)+0.71);
// fixed b = (((IN.worldPos.y*-0.35)/20)+0.35);