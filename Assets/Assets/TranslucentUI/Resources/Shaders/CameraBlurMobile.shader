
Shader "Custom/CameraBlurMobile"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		__GreyScale("GreyScale", Float) = 0
		__Brightness("Brightness", Float) = 0
	}

	CGINCLUDE
		#include "UnityCG.cginc"

		sampler2D _MainTex;
		half4 _MainTex_TexelSize;
		half4 _MainTex_ST;
		uniform half __GreyScale;
		uniform half __Brightness;

		uniform fixed _Radius;

		struct v2f
		{
			half4 vertex : SV_POSITION;
			half2 uv1 : TEXCOORD1;
			half2 uv2 : TEXCOORD2;
			half2 uv3 : TEXCOORD3;
			half2 uv4 : TEXCOORD4;
		};

		v2f vert(appdata_img v)
		{
			v2f o;
			half2 delta = half2(0.5h, 0.5h) * _MainTex_TexelSize.xy * _Radius;
			half2 deltaInvertY = half2(delta.x, -delta.y);

			o.vertex = UnityObjectToClipPos(v.vertex);
			o.uv1 = UnityStereoScreenSpaceUVAdjust(v.texcoord - delta, _MainTex_ST);
			o.uv2 = UnityStereoScreenSpaceUVAdjust(v.texcoord + delta, _MainTex_ST);
			o.uv3 = UnityStereoScreenSpaceUVAdjust(v.texcoord + deltaInvertY, _MainTex_ST);
			o.uv4 = UnityStereoScreenSpaceUVAdjust(v.texcoord - deltaInvertY, _MainTex_ST);

			return o;
		}

		half4 frag(v2f i) : SV_Target
		{
			half4 output = tex2D(_MainTex, i.uv1);
			output += tex2D(_MainTex, i.uv2);
			output += tex2D(_MainTex, i.uv3);
			output += tex2D(_MainTex, i.uv4);
			output /= 4.0;
			output.rgb = saturate(lerp(output.rgb, Luminance(output.rgb), __GreyScale));
			output.rgb = saturate(output.rgb + __Brightness);
			return output;
		}
	ENDCG

	SubShader
	{
		Cull Off ZWrite Off ZTest Always Blend Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag		
			ENDCG
		}
	}

	FallBack Off
}
