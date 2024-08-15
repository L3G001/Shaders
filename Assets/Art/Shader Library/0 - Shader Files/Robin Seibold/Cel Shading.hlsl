#ifndef LIGHTING_CEL_SHADED_INCLUDED
#define LIGHTING_CEL_SHADED_INCLUDED

#ifndef SHADERGRAPH_PREVIEW
struct SurfaceVariables
{
    float3 normal;
    float3 view;
    float smoothness;
    float shininess;
    float rimThreshold;
    float shadowAttenuation;
    float distanceAttenuation;
    float diff;
    float spec;
    float specularOffset;
    float r;
    float rimOffset;
};
#endif

float3 CalculateCelShading(Light l, SurfaceVariables s)
{
    float shadowAttenuationSmoothStepped = smoothstep(0.0f, s.shadowAttenuation, l.shadowAttenuation);
    float distanceAttenuationSmoothStepped = smoothstep(0.0f, s.distanceAttenuation, l.distanceAttenuation);
    float attenuation = shadowAttenuationSmoothStepped * distanceAttenuationSmoothStepped;

    float diffuse = saturate(dot(s.normal, l.direction));
    diffuse *= attenuation;

    float3 h = SafeNormalize(l.direction + s.view);
    float specular = saturate(dot(s.normal, h));
    specular = pow(specular, s.shininess);
    specular *= diffuse * s.smoothness;

    float rim = 1 - dot(s.view, s.normal);
    rim *= pow(diffuse, s.rimThreshold);

    diffuse = smoothstep(0.0f, s.diff, diffuse);
    specular = s.smoothness * smoothstep((1 - s.smoothness) * s.spec + s.specularOffset, s.spec + s.specularOffset, specular);

    rim = s.smoothness * smoothstep(s.r - 0.5f * s.rimOffset, s.r + 0.5f * s.rimOffset, rim);

    return l.color * (diffuse + max(specular, rim));
};

void LightingCelShaded_float(float Smoothness, float RimThreshold, float3 Position, float3 Normal, float3 View, float EdgeDiffuse, float EdgeSpecular, float EdgeSpecularOffset, float EdgeDistanceAttenuation, float EdgeShadowAttenuation, float EdgeRim, float EdgeRimOffset, out float3 Color)
{
#if defined(SHADERGRAPH_PREVIEW)
#else
    SurfaceVariables s;
    s.normal = normalize(Normal);
    s.view = SafeNormalize(View);
    s.smoothness = Smoothness;
    s.shininess = exp2(10 * Smoothness + 1);
    s.rimThreshold = RimThreshold;
    s.diff = EdgeDiffuse;
    s.spec = EdgeSpecular;
    s.specularOffset = EdgeSpecularOffset;
    s.distanceAttenuation = EdgeDistanceAttenuation;
    s.shadowAttenuation = EdgeShadowAttenuation;
    s.r = EdgeRim;
    s.rimOffset = EdgeRimOffset;

#if SHADOWS_SCREEN
    float4 clipPos = TransformWorldToHClip(Position);
    float4 shadowCoord = ComputeScreenPos(clipPos);
#else
    float4 shadowCoord = TransformWorldToShadowCoord(Position);
#endif
    
    Light light = GetMainLight(shadowCoord);
    Color = CalculateCelShading(light,s);

    int pixelLightCount = GetAdditionalLightsCount();
    for (int i = 0; i < pixelLightCount; i++)
	{
		light = GetAdditionalLight(i, Position, 1);
		Color += CalculateCelShading(light,s);
	}
#endif
};

#endif