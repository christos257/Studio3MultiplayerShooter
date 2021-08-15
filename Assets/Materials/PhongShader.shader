Shader "Custom/Phong"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _AmbientColor("Ambient Color", color) = (1, 1, 1, 1)
        _DiffuseColor("Diffuse Color", color) = (1, 1, 1, 1)
        _SpecularColor("Specular Color", color) = (1, 1, 1, 1)
        _Gloss("Gloss", Range(1.0, 1024.0)) = 1
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 100

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float3 normal: NORMAL;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float3 normal: NORMAL;
                    float4 vertex : SV_POSITION;
                    float3 viewVector : TEXCOORD1;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                fixed4 _AmbientColor;
                fixed4 _DiffuseColor;
                fixed4 _SpecularColor;
                float _Gloss;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    o.normal = UnityObjectToWorldNormal(v.normal);
                    o.viewVector = WorldSpaceViewDir(v.vertex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {

                    // sample the texture
                    fixed4 col = tex2D(_MainTex, i.uv);

                //-----------------------------------AMBIENT------------------------------
                float4 ambientLight = _AmbientColor;
                //-----------------------------------AMBIENT------------------------------
                float3 normalizedNormal = normalize(i.normal);
                float3 normalizedViewVector = normalize(i.viewVector);

                //-----------------------------------DIFFUSE------------------------------
                float LdotN = max(dot(_WorldSpaceLightPos0, normalizedNormal), 0);
                /*(if (LdotN > 0)
                {
                    LdotN = 1;
                }
                else
                {
                    LdotN = 0;
                }*/
                float4 diffuseLight = float4(_DiffuseColor.xyz * LdotN, 1);
                //-----------------------------------DIFFUSE------------------------------

                //-----------------------------------SPECULAR------------------------------
                float3 halfVector = normalize(normalizedViewVector + _WorldSpaceLightPos0);
                float HdotN = dot(halfVector, normalizedNormal);
                float4 specularLight = pow(_SpecularColor * HdotN, _Gloss);
                //-----------------------------------SPECULAR------------------------------

                float4 finalColor = diffuseLight + ambientLight + specularLight;
                return col * finalColor;
            }
            ENDCG
        }
        }
}