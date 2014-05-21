Shader "Sprites/DiffuseOverlay"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_OverlayTex ("Base (RGB)", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		_OverlayColor ("OverlayTint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
		#pragma surface surf Lambert alpha vertex:vert
		#pragma multi_compile DUMMY PIXELSNAP_ON

		sampler2D _MainTex;
		sampler2D _OverlayTex;
		fixed4 _Color;
		fixed4 _OverlayColor;

		struct Input
		{
			float2 uv_MainTex : Texcoord0;
			float2 uv_OverlayTex : Texcoord1;
			fixed4 color;
			fixed4 overlayColor;
		};
		
		void vert (inout appdata_full v, out Input o)
		{
			#if defined(PIXELSNAP_ON) && !defined(SHADER_API_FLASH)
			v.vertex = UnityPixelSnap (v.vertex);
			#endif
			v.normal = float3(0,0,-1);
			
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.color = _Color;
			o.overlayColor = _OverlayColor;
		}

		void surf (Input IN, inout SurfaceOutput o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * IN.overlayColor;
			fixed4 cOver = tex2D(_OverlayTex, IN.uv_OverlayTex) * IN.color;

			o.Albedo = cOver.a < 0.5 ? c.rgb : c.rgb * cOver.rgb;
			//o.Albedo = lerp(c.rgb, c.rgb * cOver.rgb, cOver.a);
			o.Alpha = c.a;
		}
		ENDCG
	}

Fallback "Transparent/VertexLit"
}
