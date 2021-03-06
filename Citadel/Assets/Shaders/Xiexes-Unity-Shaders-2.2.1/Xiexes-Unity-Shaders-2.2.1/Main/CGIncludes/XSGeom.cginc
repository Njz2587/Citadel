#if defined(Geometry)
    [maxvertexcount(6)]
    void geom(triangle v2g IN[3], inout TriangleStream<g2f> tristream)
    {
        g2f o;

        //Outlines loop
        for (int i = 2; i >= 0; i--)
        {	
            float4 worldPos = (mul(unity_ObjectToWorld, IN[i].vertex));
            half outlineWidthMask = tex2Dlod(_OutlineMask, float4(IN[i].uv, 0, 0));
            float3 outlineWidth = outlineWidthMask * _OutlineWidth * .01;
            outlineWidth *= min(distance(worldPos, _WorldSpaceCameraPos) * 3, 1);
            float4 outlinePos = float4(IN[i].vertex + normalize(IN[i].normal) * outlineWidth, 1);
            
            
            o.pos = UnityObjectToClipPos(outlinePos);
            o.worldPos = worldPos;
            o.ntb[0] = IN[i].ntb[0];
            o.ntb[1] = IN[i].ntb[1];
            o.ntb[2] = IN[i].ntb[2];
            o.uv = IN[i].uv;
            o.uv1 = IN[i].uv1;
            o.color = float4(_OutlineColor.rgb, 1); // store if outline in alpha channel of vertex colors | 1 = is an outline
            o.screenPos = ComputeScreenPos(o.pos);
            o.objPos = normalize(outlinePos);

            #if defined (SHADOWS_SCREEN) || ( defined (SHADOWS_DEPTH) && defined (SPOT) ) || defined (SHADOWS_CUBE)
                o._ShadowCoord = IN[i]._ShadowCoord; //Can't use TRANSFER_SHADOW() macro here
            #endif
            UNITY_TRANSFER_FOG(o, o.pos);
            tristream.Append(o);
        }
        tristream.RestartStrip();
        
        //Main Mesh loop
        for (int j = 0; j < 3; j++)
        {
            o.pos = UnityObjectToClipPos(IN[j].vertex);
            o.worldPos = IN[j].worldPos;
            o.ntb[0] = IN[j].ntb[0];
            o.ntb[1] = IN[j].ntb[1];
            o.ntb[2] = IN[j].ntb[2];
            o.uv = IN[j].uv;
            o.uv1 = IN[j].uv1;
            o.color = float4(IN[j].color.rgb,0); // store if outline in alpha channel of vertex colors | 0 = not an outline
            o.screenPos = ComputeScreenPos(o.pos);
            o.objPos = normalize(IN[j].vertex);

            #if defined (SHADOWS_SCREEN) || ( defined (SHADOWS_DEPTH) && defined (SPOT) ) || defined (SHADOWS_CUBE)
                o._ShadowCoord = IN[j]._ShadowCoord; //Can't use TRANSFER_SHADOW() or UNITY_TRANSFER_SHADOW() macros here, could use custom versions of them
            #endif
            UNITY_TRANSFER_FOG(o, o.pos);
            tristream.Append(o);
        }
        tristream.RestartStrip();
    }
#endif