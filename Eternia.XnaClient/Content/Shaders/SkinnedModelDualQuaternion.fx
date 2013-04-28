//-----------------------------------------------------------------------------
// SkinnedModel.fx
//
// Microsoft Game Technology Group
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

//This code contains modified code from: http://isg.cs.tcd.ie/kavanl/dq/dqs.cg
//As such, it keeps the original code's license:
/* dqs.cg

  Dual quaternion skinning vertex shader (unoptimized version, no shading computations)

  Version 1.0.0, February 7th, 2007

  Copyright (C) 2006-2007 University of Dublin, Trinity College, All Rights 
  Reserved

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the author(s) be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Author: Ladislav Kavan, kavanl@cs.tcd.ie

*/


// Maximum number of bone matrices we can render using shader 2.0 in a single pass.
// If you change this, update SkinnedModelProcessor.cs to match.
// Note: This number could theoretically be increased to 59 * 2 because dual quaternions take up half of the constant registers that matrices do.
#define MaxBones 115


// Input parameters.
float4x4 World;
float4x4 View;
float4x4 Projection;

float4 Bones[MaxBones * 2];

float3 Light1Direction = normalize(float3(0.5, -1, -0.2));
float3 Light1Color = float3(1, 0.9, 0.7);

float3 Light2Direction = normalize(float3(-0.3, 1, 0.1));
float3 Light2Color = float3(1, 0.9, 0.7);

float3 AmbientColor = 0.3;
float3 DiffuseColor = 1;

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

float4x4 DQToMatrix(float4 Qn, float4 Qd)
{	
	Matrix M;
    
    float len2 = dot(Qn, Qn);
    float w = Qn.w, x = Qn.x, y = Qn.y, z = Qn.z;
    float t0 = Qd.w, t1 = Qd.x, t2 = Qd.y, t3 = Qd.z;
				
    M[0][0] = w*w + x*x - y*y - z*z;
    M[1][0] = 2 * x * y - 2 * w * z;
    M[2][0] = 2 * x * z + 2 * w * y;
    M[0][1] = 2 * x * y + 2 * w * z;
    M[1][1] = w * w + y * y - x * x - z * z;
    M[2][1] = 2 * y * z - 2 * w * x;
    M[0][2] = 2 * x * z - 2 * w * y;
    M[1][2] = 2 * y * z + 2 * w * x;
    M[2][2] = w * w + z * z - x * x - y * y;

    M[3][0] = -2 * t0 * x + 2 * w * t1 - 2 * t2 * z + 2 * y * t3;
    M[3][1] = -2 * t0 * y + 2 * t1 * z - 2 * x * t3 + 2 * w * t2;
    M[3][2] = -2 * t0 * z + 2 * x * t2 + 2 * w * t3 - 2 * t1 * y;

    M[0][3] = 0;
    M[1][3] = 0;
    M[2][3] = 0;
    M[3][3] = len2;

    M /= len2;

    return M;	
}

// Vertex shader program.
VS_OUTPUT VertexShaderFunction(VS_INPUT input)
{
    VS_OUTPUT output;
    
    float2x4 blendDQ;
    
    /*blendDQ[0] = Bones[input.BoneIndices.x * 2];
    blendDQ[1] = Bones[input.BoneIndices.x * 2 + 1];*/
    
    float yc = 1.0, zc = 1.0, wc = 1.0;
    
    if (dot(Bones[input.BoneIndices.x * 2], Bones[input.BoneIndices.y * 2]) < 0.0)
    {
		yc = -1.0;
    }
    if (dot(Bones[input.BoneIndices.x * 2], Bones[input.BoneIndices.z * 2]) < 0.0)
    {
		zc = -1.0;
	}
    if (dot(Bones[input.BoneIndices.x * 2], Bones[input.BoneIndices.w * 2]) < 0.0)
    {
		wc = -1.0;
	}
    
    blendDQ[0] = Bones[input.BoneIndices.x * 2] * input.BoneWeights.x;
    blendDQ[1] = Bones[input.BoneIndices.x * 2 + 1] * input.BoneWeights.x;
    
    blendDQ[0] += yc*Bones[input.BoneIndices.y * 2] * input.BoneWeights.y;
    blendDQ[1] += yc*Bones[input.BoneIndices.y * 2 + 1] * input.BoneWeights.y;
    
    blendDQ[0] += zc*Bones[input.BoneIndices.z * 2] * input.BoneWeights.z;
    blendDQ[1] += zc*Bones[input.BoneIndices.z * 2 + 1] * input.BoneWeights.z;
    
    blendDQ[0] += wc*Bones[input.BoneIndices.w * 2] * input.BoneWeights.w;
    blendDQ[1] += wc*Bones[input.BoneIndices.w * 2 + 1] * input.BoneWeights.w;
    
    // Blend between the weighted bone matrices.
    float4x4 skinTransform = DQToMatrix(blendDQ[0], blendDQ[1]);
    
    // Skin the vertex position.
    float4 position = mul(input.Position, skinTransform);
    
    output.Position = mul(mul(mul(position, World), View), Projection);

    // Skin the vertex normal, then compute lighting.
    float3 normal = normalize(mul(mul(input.Normal, skinTransform), World));
    
    float3 light1 = max(dot(normal, Light1Direction), 0) * Light1Color;
    float3 light2 = max(dot(normal, Light2Direction), 0) * Light2Color;

    output.Lighting = DiffuseColor * (light1 + light2) + AmbientColor;

    output.TexCoord = input.TexCoord;
    
    return output;
}


// Pixel shader input structure.
struct PS_INPUT
{
    float3 Lighting : COLOR0;
    float2 TexCoord : TEXCOORD0;
};


// Pixel shader program.
float4 PixelShaderFunction(PS_INPUT input) : COLOR0
{
    float4 color = tex2D(Sampler, input.TexCoord);

    color.rgb *= input.Lighting;
    
    return color;
}


technique SkinnedModelTechnique
{
    pass SkinnedModelPass
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
