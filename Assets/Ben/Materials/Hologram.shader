Shader "benShades/Hologram"
{
    Properties
    {


        _OutlineColor("Outline Color", Color) = (1,0.5,0.5,1)
        _OutlinePower("Power", Range(0.5,10)) = 0.5

    }
        SubShader
    {
        Tags {"Queue" = "Transparent" }

        CGPROGRAM

        #pragma surface surf Lambert  alpha:fade


        struct Input
        {
            float3 viewDir;
        };

        float4 _OutlineColor;

        float _OutlinePower;


        void surf(Input IN, inout SurfaceOutput o) {
          half tempOL = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
          o.Emission = _OutlineColor.rgb * pow(tempOL, _OutlinePower) * 10;
          o.Alpha = pow(tempOL, _OutlinePower);
      }
        ENDCG
    }
        FallBack "Diffuse"
}
