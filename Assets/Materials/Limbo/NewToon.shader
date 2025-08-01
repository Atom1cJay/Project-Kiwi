Shader "Roystan/Toon/NewToon"
{
	Properties
	{
		_Color("Color", Color) = (0.5, 0.65, 1, 1)
		_MainTex("Main Texture", 2D) = "white" {}
		[HDR] _AmbientColor("Ambient Color", Color) = (0.4,0.4,0.4,1)
		[HDR] _SpecularColor("Specular Color", Color) = (0.9,0.9,0.9,1)
		_Glossiness("Glossiness", Float) = 32
		[HDR] _RimColor("Rim Color", Color) = (1,1,1,1)
		_RimAmount("Rim Amount", Range(0, 1)) = 0.716
		_RimThreshold("Rim Threshold", Range(0, 1)) = 0.1
	}
	SubShader
	{
		Pass
		{
			Tags
			{
				"RenderPipeline" = "UniversalPipeline"
				"LightMode" = "UniversalForward"
				"PassFlags" = "OnlyDirectional"
			}

			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			//#pragma multi_compile
			//#pragma multi_compile_fwdbase
			//#pragma multi_compile_local __ MAIN_LIGHT_CALCULATE_SHADOWS_ON
			//#pragma multi_compile_local __ _MAIN_LIGHT_SHADOWS_CASCADE_ON 
			//#pragma multi_compile_local __ _SHADOWS_SOFT_ON
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

			struct appdata
			{
				float4 vertex : POSITION;				
				float4 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 worldNormal : NORMAL;
				float3 viewDir : TEXCOORD1;
				float4 shadowCoord : TEXCOORD2;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.pos = TransformObjectToHClip(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldNormal = TransformObjectToWorldNormal(v.normal);
				o.viewDir = GetWorldSpaceViewDir(v.vertex);
				o.shadowCoord = TransformWorldToShadowCoord(o.pos);
				return o;
			}
			
			float4 _Color;
			float4 _AmbientColor;
			float _Glossiness;
			float4 _SpecularColor;
			float4 _RimColor;
			float _RimAmount;
			float _RimThreshold;

			float4 frag (v2f i) : SV_Target
			{
				float3 normal = normalize(i.worldNormal);
				float NdotL = dot(_MainLightPosition, normal);

				float shadow = MainLightRealtimeShadow(i.shadowCoord);
				Light mainLight = GetMainLight(i.shadowCoord);
				//half shadow = mainLight.shadowAttenuation;
				//float shadow = MainLightRealtimeShadow(i.shadowCoord); 
				float lightIntensity = smoothstep(0, 0.01, NdotL * shadow);

				float4 light = lightIntensity * _MainLightColor;

				float3 viewDir = normalize(i.viewDir);
				float3 halfVector = normalize(_MainLightPosition + viewDir);
				float NdotH = dot(normal, halfVector);
				float specularIntensity = pow(NdotH * lightIntensity, _Glossiness * _Glossiness);
				float specularIntensitySmooth = smoothstep(0.005, 0.01, specularIntensity);
				float4 specular = specularIntensitySmooth * _SpecularColor;

				float4 rimDot = 1 - dot(viewDir, normal);
				float rimIntensity = rimDot * pow(NdotL, _RimThreshold);
				rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimIntensity);
				float4 rim = rimIntensity * _RimColor;

				float4 sample = tex2D(_MainTex, i.uv);
				return _Color * sample * (_AmbientColor + light + specular + rim);
			}
			ENDHLSL
		}

		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}
}