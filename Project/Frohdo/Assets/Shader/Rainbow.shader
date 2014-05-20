Shader "Custom/Rainbow" {
	Properties
{
_Anim("Time", Float) = 0
_ResX("_ResX", Float) = 128
_ResY("_ResY", Float) = 128
_WaveSpeed("_WaveSpeed", Float) = 50
}
SubShader
{
Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
Pass
{
CGPROGRAM
#pragma target 3.0
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
 
uniform float _ResX;
uniform float _ResY;
uniform float _Anim;
uniform float _WaveSpeed;
 
struct v2f {
float4 vertex : POSITION;
float4 color : COLOR0;
float4 fragPos : COLOR1;
};
 
v2f vert (appdata_base v)
{
v2f o;

float sinOff=v.vertex.x+v.vertex.y+v.vertex.z;
float t=_Time*_WaveSpeed;
if(t < 0.0) t *= -1.0;
 
v.vertex.x+=sin(t*1.45+sinOff)*0.5;
v.vertex.y=sin(t*3.12+sinOff)*0.7;
v.vertex.z-=sin(t*2.2+sinOff)*0.2;
 
o.vertex = mul( UNITY_MATRIX_MVP, v.vertex );
o.fragPos = o.vertex;

//o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
//o.fragPos = o.pos;
//o.color = float4 (1.0, 1.0, 1.0, 1);
return o;
}
 
half4 frag (v2f i) : COLOR {
float4 outColor = i.color;
float x = i.fragPos.x;
float y = i.fragPos.z;
float mov0 = x+y+cos(sin(_Time[1]/10)*2.)*100.+sin(x/100.)*1000.;
float mov1 = y / _ResY / 0.2 + _Time[1];
float mov2 = x / _ResX / 0.2;
float c1 = abs(sin(mov1+_Time[1])/2.+mov2/2.-mov1-mov2+_Time[1]);
float c2 = abs(sin(c1+sin(mov0/1000.+_Time[1])+sin(y/40.+_Time[1])+sin((x+y)/100.)*3.));
float c3 = abs(sin(c2+cos(mov1+mov2+c2)+cos(mov2)+sin(x/1000.)));
outColor.rgba = float4(c1,c2,c3,0.4);
return outColor;
}
ENDCG
}
}
FallBack "VertexLit"
}
