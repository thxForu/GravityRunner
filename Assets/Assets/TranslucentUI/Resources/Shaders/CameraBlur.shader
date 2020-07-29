Shader "Custom/CameraBlur"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		__GreyScale("GreyScale", Float) = 0
		__Brightness("Brightness", Float) = 0
	}

	CGINCLUDE
	#include "UnityCG.cginc"

	uniform sampler2D _MainTex;
	float4 _MainTex_TexelSize;
	uniform float _Radius;
	float4 _MainTex_ST;
	uniform half __GreyScale;
	uniform half __Brightness;

	struct vertexInput
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
	};

	struct vertexOutput
	{
		float4 vertex : SV_POSITION;
		float2 texcoord : TEXCOORD0;
	};

	struct outputSmall
	{
		float4 vertex : SV_POSITION;
		float2 texcoord : TEXCOORD0;
		float4 blurTexcoord : TEXCOORD1;
	};
	struct outputMedium
	{
		float4 vertex : SV_POSITION;
		float2 texcoord : TEXCOORD0;
		float4 blurTexcoord[2] : TEXCOORD1;
	};
	struct outputBig
	{
		float4 vertex : SV_POSITION;
		float2 texcoord : TEXCOORD0;
		float4 blurTexcoord[3] : TEXCOORD1;
	};

	vertexOutput vert (vertexInput i)
	{
		vertexOutput o;
		o.vertex = UnityObjectToClipPos(i.vertex);
		o.texcoord = i.uv;
		return o;
	}

	fixed4 frag (vertexOutput i) : SV_Target
	{
		fixed3 color = tex2D(_MainTex, i.texcoord);
		return fixed4(color, 1.0);
	}

	outputSmall vertSmallHorizontal (vertexInput i)
	{
		outputSmall o;
		o.vertex = UnityObjectToClipPos(i.vertex);
		float2 offset1 = float2(_MainTex_TexelSize.x * _Radius * 1.33333333, 0.0); 
		float2 uv = i.uv;
		o.texcoord = UnityStereoScreenSpaceUVAdjust(uv, _MainTex_ST);
		o.blurTexcoord.xy = UnityStereoScreenSpaceUVAdjust(uv + offset1, _MainTex_ST);
		o.blurTexcoord.zw = UnityStereoScreenSpaceUVAdjust(uv - offset1, _MainTex_ST);
		return o;
	}

	outputSmall vertSmallVertical (vertexInput i)
	{
		outputSmall o;
		o.vertex = UnityObjectToClipPos(i.vertex);
		float2 offset1 = float2(0.0, _MainTex_TexelSize.y * _Radius * 1.33333333); 
		float2 uv = i.uv;
		o.texcoord = UnityStereoScreenSpaceUVAdjust(uv, _MainTex_ST);
		o.blurTexcoord.xy = UnityStereoScreenSpaceUVAdjust(uv + offset1, _MainTex_ST);
		o.blurTexcoord.zw = UnityStereoScreenSpaceUVAdjust(uv - offset1, _MainTex_ST);
		return o;
	}

	fixed4 fragSmall (outputSmall i) : SV_Target
	{
		float3 sum = tex2D(_MainTex, i.texcoord).xyz * 0.29411764;
		sum += tex2D(_MainTex, i.blurTexcoord.xy).xyz * 0.35294117;
		sum += tex2D(_MainTex, i.blurTexcoord.zw).xyz * 0.35294117;
		sum.rgb = saturate(lerp(sum.rgb, Luminance(sum.rgb), __GreyScale));
		sum.rgb = saturate(sum.rgb + __Brightness);
		return fixed4(sum, 1.0);
	}

	outputMedium vertMediumHorizontal (vertexInput i)
	{
		outputMedium o;
		o.vertex = UnityObjectToClipPos(i.vertex);
		float2 offset1 = float2(_MainTex_TexelSize.x * _Radius * 1.38461538, 0.0); 
		float2 offset2 = float2(_MainTex_TexelSize.x * _Radius * 3.23076923, 0.0);

		float2 uv = i.uv;

		o.texcoord = UnityStereoScreenSpaceUVAdjust(uv, _MainTex_ST);
		o.blurTexcoord[0].xy = UnityStereoScreenSpaceUVAdjust(uv + offset1, _MainTex_ST);
		o.blurTexcoord[0].zw = UnityStereoScreenSpaceUVAdjust(uv - offset1, _MainTex_ST);
		o.blurTexcoord[1].xy = UnityStereoScreenSpaceUVAdjust(uv + offset2, _MainTex_ST);
		o.blurTexcoord[1].zw = UnityStereoScreenSpaceUVAdjust(uv - offset2, _MainTex_ST);

		return o;
	}

	outputMedium vertMediumVertical (vertexInput i)
	{
		outputMedium o;
		o.vertex = UnityObjectToClipPos(i.vertex);
		float2 offset1 = float2(0.0, _MainTex_TexelSize.y * _Radius * 1.38461538); 
		float2 offset2 = float2(0.0, _MainTex_TexelSize.y * _Radius * 3.23076923);

		float2 uv = i.uv;
		o.texcoord = UnityStereoScreenSpaceUVAdjust(uv, _MainTex_ST);
		o.blurTexcoord[0].xy = UnityStereoScreenSpaceUVAdjust(uv + offset1, _MainTex_ST);
		o.blurTexcoord[0].zw = UnityStereoScreenSpaceUVAdjust(uv - offset1, _MainTex_ST);
		o.blurTexcoord[1].xy = UnityStereoScreenSpaceUVAdjust(uv + offset2, _MainTex_ST);
		o.blurTexcoord[1].zw = UnityStereoScreenSpaceUVAdjust(uv - offset2, _MainTex_ST);

		return o;
	}

	fixed4 fragMedium (outputMedium i) : SV_Target
	{
		float3 sum = tex2D(_MainTex, i.texcoord).xyz * 0.22702702;
		sum += tex2D(_MainTex, i.blurTexcoord[0].xy).xyz * 0.31621621;
		sum += tex2D(_MainTex, i.blurTexcoord[0].zw).xyz * 0.31621621;
		sum += tex2D(_MainTex, i.blurTexcoord[1].xy).xyz * 0.07027027;
		sum += tex2D(_MainTex, i.blurTexcoord[1].zw).xyz * 0.07027027;
		sum.rgb = saturate(lerp(sum.rgb, Luminance(sum.rgb), __GreyScale));
		sum.rgb = saturate(sum.rgb + __Brightness);
		return fixed4(sum, 1.0);
	}

	outputBig vertBigHorizontal (vertexInput i)
	{
		outputBig o;
		o.vertex = UnityObjectToClipPos(i.vertex);
		float2 offset1 = float2(_MainTex_TexelSize.x * _Radius * 1.41176470, 0.0); 
		float2 offset2 = float2(_MainTex_TexelSize.x * _Radius * 3.29411764, 0.0);
		float2 offset3 = float2(_MainTex_TexelSize.x * _Radius * 5.17647058, 0.0);

		float2 uv = i.uv;

		o.texcoord = UnityStereoScreenSpaceUVAdjust(uv, _MainTex_ST);
		o.blurTexcoord[0].xy = UnityStereoScreenSpaceUVAdjust(uv + offset1, _MainTex_ST);
		o.blurTexcoord[0].zw = UnityStereoScreenSpaceUVAdjust(uv - offset1, _MainTex_ST);
		o.blurTexcoord[1].xy = UnityStereoScreenSpaceUVAdjust(uv + offset2, _MainTex_ST);
		o.blurTexcoord[1].zw = UnityStereoScreenSpaceUVAdjust(uv - offset2, _MainTex_ST);
		o.blurTexcoord[2].xy = UnityStereoScreenSpaceUVAdjust(uv + offset3, _MainTex_ST);
		o.blurTexcoord[2].zw = UnityStereoScreenSpaceUVAdjust(uv - offset3, _MainTex_ST);

		return o;
	}

	outputBig vertBigVertical (vertexInput i)
	{
		outputBig o;
		o.vertex = UnityObjectToClipPos(i.vertex);
		float2 offset1 = float2(0.0, _MainTex_TexelSize.y * _Radius * 1.41176470); 
		float2 offset2 = float2(0.0, _MainTex_TexelSize.y * _Radius * 3.29411764);
		float2 offset3 = float2(0.0, _MainTex_TexelSize.y * _Radius * 5.17647058);
		float2 uv = i.uv;
		o.texcoord = uv;
		o.texcoord = UnityStereoScreenSpaceUVAdjust(uv, _MainTex_ST);
		o.blurTexcoord[0].xy = UnityStereoScreenSpaceUVAdjust(uv + offset1, _MainTex_ST);
		o.blurTexcoord[0].zw = UnityStereoScreenSpaceUVAdjust(uv - offset1, _MainTex_ST);
		o.blurTexcoord[1].xy = UnityStereoScreenSpaceUVAdjust(uv + offset2, _MainTex_ST);
		o.blurTexcoord[1].zw = UnityStereoScreenSpaceUVAdjust(uv - offset2, _MainTex_ST);
		o.blurTexcoord[2].xy = UnityStereoScreenSpaceUVAdjust(uv + offset3, _MainTex_ST);
		o.blurTexcoord[2].zw = UnityStereoScreenSpaceUVAdjust(uv - offset3, _MainTex_ST);
		return o;
	}

	fixed4 fragBig (outputBig i) : SV_Target
	{
		float3 sum = tex2D(_MainTex, i.texcoord).xyz * 0.19648255;
		sum += tex2D(_MainTex, i.blurTexcoord[0].xy).xyz * 0.29690696;
		sum += tex2D(_MainTex, i.blurTexcoord[0].zw).xyz * 0.29690696;
		sum += tex2D(_MainTex, i.blurTexcoord[1].xy).xyz * 0.09447039;
		sum += tex2D(_MainTex, i.blurTexcoord[1].zw).xyz * 0.09447039;
		sum += tex2D(_MainTex, i.blurTexcoord[2].xy).xyz * 0.01038136;
		sum += tex2D(_MainTex, i.blurTexcoord[2].zw).xyz * 0.01038136;
		sum.rgb = saturate(lerp(sum.rgb, Luminance(sum.rgb), __GreyScale));
		sum.rgb = saturate(sum.rgb + __Brightness);
		return fixed4(sum, 1.0);
	}

	ENDCG


	SubShader
	{

		Cull Off ZWrite Off ZTest Always Blend Off
		//
		//	dummy pass
		//
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}

		//
		//	Small kernal gaussian blur
		//
		Pass
		{
			CGPROGRAM
			#pragma vertex vertSmallHorizontal
			#pragma fragment fragSmall
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex vertSmallVertical
			#pragma fragment fragSmall
			ENDCG
		}

		//
		//	Medium kernal gaussian blur
		//
		Pass
		{
			CGPROGRAM
			#pragma vertex vertMediumHorizontal
			#pragma fragment fragMedium
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex vertMediumVertical
			#pragma fragment fragMedium
			ENDCG
		}

		//
		//	Big kernal gaussian blur
		//
		Pass
		{
			CGPROGRAM
			#pragma vertex vertBigHorizontal
			#pragma fragment fragBig
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex vertBigVertical
			#pragma fragment fragBig
			ENDCG
		}
	
	}
}
