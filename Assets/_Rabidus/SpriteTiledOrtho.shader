Shader "Custom/SpriteTiledOrtho"
{
    Properties
    {
        [PerRendererData]_MainTex ("Sprite Texture", 2D) = "white" {}
        _Color    ("Tint", Color) = (1,1,1,1)

        _Anchor   ("Anchor Offset (X,Y)", Vector) = (0,0,0,0)

        // Единый контрол размера тайла в мировых единицах.
        // Это высота одного повторяющегося тайла (по Y).
        // По X ширина считается через _Aspect.
        _Size     ("Tile Size World", Float) = 1

        // Соотношение сторон тайла (ширина : высота), по умолчанию 1:1
        _Aspect   ("Aspect (X:Y)", Vector) = (1,1,0,0)
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "CanUseSpriteAtlas"="True"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Lighting Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 worldPos : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;

            float4 _Anchor;    // xy: сдвиг тайла в мировом пространстве
            float4 _Aspect;    // xy: желаемый аспект (ширина : высота)
            float  _Size;      // высота тайла в world units

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // мировые координаты вершины спрайта
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldPos = worldPos.xy;

                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // соотношение сторон (ширина/высота)
                float aspectRatio = _Aspect.x / max(_Aspect.y, 1e-6);

                // физический размер тайла в мире:
                // ширина = _Size * aspectRatio
                // высота = _Size
                float2 tileWorldScale = float2(
                    _Size * aspectRatio,
                    _Size
                );

                // uv до frac — "раскрученный" по миру
                float2 baseUV = (i.worldPos + _Anchor.xy) / tileWorldScale;

                // Производные по экрану: как быстро текстура меняется внутри одного пикселя
                float2 dx = ddx(baseUV);
                float2 dy = ddy(baseUV);

                // Насколько сильно текстура ужимается в пикселе
                float pixelFootprint = max(length(dx), length(dy));

                // Базовые UV с зацикливанием
                float2 tiledUV = frac(baseUV);

                fixed4 col;

                // Если тайл очень частый (pixelFootprint > 1), делаем лёгкое усреднение 4 сэмплов,
                // что гасит рябь при отдалении/зуме.
                if (pixelFootprint > 1.0)
                {
                    float2 offX = dx * 0.5;
                    float2 offY = dy * 0.5;

                    fixed4 c00 = tex2D(_MainTex, frac(baseUV + offX + offY));
                    fixed4 c01 = tex2D(_MainTex, frac(baseUV + offX - offY));
                    fixed4 c10 = tex2D(_MainTex, frac(baseUV - offX + offY));
                    fixed4 c11 = tex2D(_MainTex, frac(baseUV - offX - offY));

                    col = (c00 + c01 + c10 + c11) * 0.25;
                }
                else
                {
                    // вблизи — обычное чтение, чтобы не мылить текстуру
                    col = tex2D(_MainTex, tiledUV);
                }

                return col * i.color;
            }

            ENDCG
        }
    }
}
