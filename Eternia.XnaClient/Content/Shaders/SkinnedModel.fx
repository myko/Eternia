//-----------------------------------------------------------------------------
// SkinnedModel.fx
//
// Microsoft Game Technology Group
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------


// Maximum number of bone matrices we can render using shader 2.0 in a single pass.
// If you change this, update SkinnedModelProcessor.cs to match.
#define MaxBones 59


// Input parameters.
float4x4 World;
float4x4 View;
float4x4 Projection;

float4x4 Bones[MaxBones];

float3 Light1Direction = normalize(float3(0.5, -1, -0.2));
float3 Light1Color = float3(1, 0.9, 0.7);

float3 Light2Direction = normalize(float3(-0.3, 1, 0.1));
float3 Light2Color = float3(1, 0.9, 0.7);

float3 AmbientColor = 0.3;
float3 DiffuseColor = 1;

float Outline = 0;

texture Texture;

sampler Sampler = sampler_state
{
    Texture = (Texture);

    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = Linear;
};


// Vertex shader input structure.
struct VS_INPUT
{
    float4 Position : POSITION0;
    float3 Normal : NORMAL0;
    float2 TexCoord : TEXCOORD0;
    float4 BoneIndices : BLENDINDICES0;
	float4 BoneWeights : BLENDWEIGHT0;
};


// Vertex shader output structure.
struct VS_OUTPUT
{
    float4 Position : POSITION0;
    float3 Lighting : COLOR0;
    float2 TexCoord : TEXCOORD0;
};


// Vertex shader program.
VS_OUTPUT VertexShaderFunction(VS_INPUT input)
{
    VS_OUTPUT output;
    
    // Blend between the weighted bone matrices.
    float4x4 skinTransform = 0;
    
    skinTransform += Bones[input.BoneIndices.x] * input.BoneWeights.x;
    skinTransform += Bones[input.BoneIndices.y] * input.BoneWeights.y;
    skinTransform += Bones[input.BoneIndices.z] * input.BoneWeights.z;
    skinTransform += Bones[input.BoneIndices.w] * input.BoneWeights.w;
    
    // Skin the vertex normal, then compute lighting.
    float3 normal = normalize(mul(mul(input.Normal, skinTransform), World));
    // Skin the vertex position.
    float4 position = mul(input.Position, skinTransform);
    position.xyz += normal * Outline;
    
    output.Position = mul(mul(mul(position, World), View), Projection);
    
    float3 light1 = max(dot(normal, -Light1Direction), 0) * Light1Color;
    float3 light2 = max(dot(normal, -Light2Direction), 0) * Light2Color;

    output.Lighting = light1 + light2;
    output.TexCoord = input.TexCoord;
    
    return output;
}


// Pixel shader input structure.
struct PS_INPUT
{
    float4 Lighting : COLOR0;
    float2 TexCoord : TEXCOORD0;
};


// Pixel shader program.
float4 PixelShaderFunction(PS_INPUT input) : COLOR0
{
    float4 color = tex2D(Sampler, input.TexCoord);

    color.rgb *= DiffuseColor * input.Lighting;
    
    return float4(AmbientColor, 0) + color;
}


technique SkinnedModelTechnique
{
    pass SkinnedModelPass
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
