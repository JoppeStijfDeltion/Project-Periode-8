Shader "Custom/Interactable" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		[Toggle]_bool ("Use a Mettalic/Smoothness map", float) = 1
		_MeSm ("Metallic(R), Smoothness(A)", 2D) = "white" {}
		_Normal("Normal Map", 2D) = "bump" {}
		_Emission ("Emmision", 2D) = "black"
		[Toggle]_interact ("Interact", float) = 0
		_Effect("Effect", 2D) = "black" {}
		_EffectColor ("Effect Color", Color) = (1,1,1,1)
		_EffectSlider("Effect Slider", Range(0,10)) = 5
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		#pragma multi_compile MS

		sampler2D _MainTex;
		sampler2D _MeSm;
		sampler2D _Normal;
		sampler2D _Emission;
		sampler2D _Effect;

		struct Input {
			float2 uv_MainTex;
			float2 uv_Effect;
			float4 screenPos;
		};

		fixed4 _Color;
		fixed4 _EffectColor;
		float _bool;
		float _interact;
		float _EffectSlider;

		void surf (Input IN, inout SurfaceOutputStandard o) {

			fixed2 scrolledUV = IN.uv_Effect;

			fixed xScrolllValue = _EffectSlider * _Time;
			fixed yScrolllValue = _EffectSlider * _Time;

			scrolledUV += fixed2(xScrolllValue, yScrolllValue);

			float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
			screenUV += fixed2(xScrolllValue, yScrolllValue);

			fixed4 effect = (saturate(tex2D (_Effect, scrolledUV)) * _EffectColor) * 0.1;//clamp(sin(_Time.w * 1), 0.2, 0.4);
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb + (effect.rgb * _interact);
			o.Normal = UnpackNormal(tex2D(_Normal, IN.uv_MainTex));
			fixed4 ms = tex2D (_MeSm, IN.uv_MainTex);
			if(_bool == 1){
				o.Metallic = ms.r;
				o.Smoothness = ms.a;
			}
			else {
				o.Metallic = 0;
				o.Smoothness = 0;
			}
			if(_interact == 1){
				o.Emission = tex2D (_Emission, IN.uv_MainTex).rgb + effect.rgb;
			}
			else {
				o.Emission = tex2D (_Emission, IN.uv_MainTex).rgb;
			}
			
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
