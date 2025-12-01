shader_type canvas_item;

uniform sampler2D source_tex : hint_default_black;
uniform float amount = 1.0;

uniform float levels : hint_range(2, 32) = 4.0; // color quantization

// 4x4 bayer matrix for ordered dithering
float bayer(vec2 p) {
    int x = int(mod(p.x, 4.0));
    int y = int(mod(p.y, 4.0));
    int index = x + y * 4;

    float m[16] = float[16](
        0.0,  8.0,  2.0, 10.0,
        12.0, 4.0, 14.0, 6.0,
        3.0, 11.0, 1.0, 9.0,
        15.0, 7.0, 13.0, 5.0
    );

    return m[index] / 16.0;
}

void fragment() {
    vec2 uv = UV;
    vec4 col = texture(source_tex, uv);

    float d = bayer(FRAGCOORD.xy);

    // Posterize color
    vec3 quantized = floor(col.rgb * levels + d) / levels;

    COLOR = vec4(quantized, col.a);
}