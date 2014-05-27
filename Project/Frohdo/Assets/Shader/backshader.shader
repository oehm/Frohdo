Shader "Custom/BackgroundMult"
    {
        Properties
        {
			_Color ("Main Color", Color) = (1,1,1,1)
			_MainTex ( "Main Tex", 2D ) = "white" {}
        }
 
        SubShader
        {
           Pass
           {
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
 
				#include "UnityCG.cginc"
 
 
				// uniforms
				uniform sampler2D _MainTex;
				//LOOK! The texture name + ST is needed to get the tiling/offset
				uniform float4 _MainTex_ST;
				uniform float4 _Color;
 
 
				struct vertexInput
				{
					float4 vertex : POSITION; 
					float4 texcoord : TEXCOORD0;
				};
 
				struct fragmentInput
				{
					float4 pos : SV_POSITION;
					float2 uv : TEXCOORD0;
				};

				fragmentInput vert( vertexInput i )
				{
					fragmentInput o;
					o.pos = mul( UNITY_MATRIX_MVP, i.vertex );
					o.uv = TRANSFORM_TEX( i.texcoord, _MainTex ); 
					return o;
				}
 
				float4 frag( fragmentInput i ) : COLOR
				{
					float4 _mainCol = tex2D(_MainTex, i.uv);

					float4 _newCol = 255-((255-_mainCol)*(255-_Color)/255);
					_newCol.a = 255;
					//_newCol.r = 255-((255-_mainCol.r)*(255-_Color.r)/255);
					//_newCol.g = 255-((255-_mainCol.g)*(255-_Color.g)/255);
					//_newCol.b = 255-((255-_mainCol.b)*(255-_Color.b)/255);
					
					
					return _newCol;
				}
			ENDCG
		} // end Pass
	} // end SubShader
 
FallBack "Diffuse"
}