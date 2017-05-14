float4x4 World;
float4x4 View;
float4x4 Projection;

texture Texture0;
texture Texture1;
texture Texture2;

sampler2D textureSampler0 = sampler_state {
	Texture = (Texture0);
	MinFilter = Point;
	MagFilter = Point;
	AddressU = WRAP;
	AddressV = WRAP;
};
sampler2D textureSampler1 = sampler_state {
	Texture = (Texture1);
	MinFilter = Point;
	MagFilter = Point;
	AddressU = WRAP;
	AddressV = WRAP;
};
sampler2D textureSampler2 = sampler_state {
	Texture = (Texture2);
	MinFilter = Point;
	MagFilter = Point;
	AddressU = WRAP;
	AddressV = WRAP;
};

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float4 Normal : NORMAL0;
	float2 TextureCoordinate : TEXCOORD0;
	float TextureIndex : PSIZE0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float4 Color : COLOR0;
	float3 Normal : TEXCOORD0;
	float2 TextureCoordinate : TEXCOORD1;
	float TextureIndex : PSIZE0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);

	output.Normal = input.Normal;

	output.TextureCoordinate = input.TextureCoordinate;
	output.TextureIndex = input.TextureIndex;
	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	if (input.TextureIndex == 0)
	{
		float4 textureColor = tex2D(textureSampler0, input.TextureCoordinate);
		return textureColor;
	}
	if (input.TextureIndex > 0.5 && input.TextureIndex < 1.5 || input.TextureIndex == 1)
	{
		float4 textureColor = tex2D(textureSampler1, input.TextureCoordinate);
		return textureColor;
	}
	if (input.TextureIndex > 1.5 && input.TextureIndex < 2.5 || input.TextureIndex == 2)
	{
		float4 textureColor = tex2D(textureSampler2, input.TextureCoordinate);
		return textureColor;
	}

	float4 textureColor = tex2D(textureSampler2, input.TextureCoordinate);
	return textureColor;
}

technique Textured
{
	pass Pass1
	{
		PixelShader = compile ps_4_0_level_9_1 PixelShaderFunction();
		VertexShader = compile vs_4_0_level_9_1 VertexShaderFunction();
	}
}