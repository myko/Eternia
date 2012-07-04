float4x4 World;
float4x4 View;
float4x4 Projection;

texture Texture0;
texture Texture1;
texture Texture2;
texture Texture3;

sampler Sampler0 = sampler_state
{
    Texture = (Texture0);

    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = Linear;
    
    AddressU = Wrap;
    AddressV = Wrap;
};

sampler Sampler1 = sampler_state
{
    Texture = (Texture1);

    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = Linear;
    
    AddressU = Wrap;
    AddressV = Wrap;
};

sampler Sampler2 = sampler_state
{
    Texture = (Texture2);

    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = Linear;
    
    AddressU = Wrap;
    AddressV = Wrap;
};

sampler Sampler3 = sampler_state
{
    Texture = (Texture3);

    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = Linear;
    
    AddressU = Wrap;
    AddressV = Wrap;
};

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float2 TexCoords: TEXCOORD0;
	float4 TexBlends: COLOR0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float2 TexCoords: TEXCOORD0;
	float4 TexBlends: COLOR0;
    float4 Color: COLOR1;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    float4 projectionPosition = mul(viewPosition, Projection);
    
    output.Position = projectionPosition;
	output.TexCoords = input.TexCoords;
	output.TexBlends = input.TexBlends;
	output.Color.rgba = float4(input.Position.zzz * 0.6 + 0.4, 1);

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 tex0color = tex2D(Sampler0, input.TexCoords) * input.TexBlends[0];
	float4 tex1color = tex2D(Sampler1, input.TexCoords) * input.TexBlends[1];
	float4 tex2color = tex2D(Sampler2, input.TexCoords) * input.TexBlends[2];
	float4 tex3color = tex2D(Sampler3, input.TexCoords) * input.TexBlends[3];
	
	float4 color = (tex0color + tex1color + tex2color + tex3color) * input.Color;
    
    return color;
}

float4 FullBrightPixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    return float4(1, 1, 1, 1);
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
    
    pass Pass2
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 FullBrightPixelShaderFunction();
    }
}
