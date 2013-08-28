// Fog of War shader

Shader "Custom/FogOfWarShader"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 150

		CGPROGRAM
		#pragma surface surf Lambert noforwardadd

		sampler2D _MainTex;
		sampler2D _VisibilityTex;

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o)
		{
			fixed4 mainColor = tex2D(_MainTex, IN.uv_MainTex);
			float2 uv_VisibilityTex;
			// uv_VisibilityTex[0] = IN.uv_MainTex[0] / (1024f / 192f);
			// uv_VisibilityTex[1] = IN.uv_MainTex[1] / (1024f / 221f);
			uv_VisibilityTex[0] = 1 - IN.uv_MainTex[0];
			uv_VisibilityTex[1] = IN.uv_MainTex[1];
			fixed4 fogColor = tex2D(_VisibilityTex, uv_VisibilityTex);
			o.Albedo = mainColor * fogColor;
		}
		ENDCG
	}
}