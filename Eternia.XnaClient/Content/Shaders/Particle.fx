float4x4 World;
float4x4 View;
float4x4 Projection;
float Alpha;
texture Texture;
float4 Diffuse = 1;

sampler Sampler = sampler_state
{
    Texture = (Texture);

    //MinFilter = Linear;
    //MagFilter = Linear;
    //MipFilter = Linear;
};

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float4 Color: COLOR0;
	float2 TexCoords: TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float4 Color: COLOR0;
	float2 TexCoords: TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
    output.Color = input.Color;
	output.TexCoords = input.TexCoords;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    float4 color = tex2D(Sampler, input.TexCoords);
    //color = input.Color; // float4(color.rgb, color.a * Alpha) * Diffuse * input.Color;
	color = color * input.Color;
	clip(color.a == 0 ? -1 : 1);
    return color;
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
