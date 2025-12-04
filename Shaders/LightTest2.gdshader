shader_type canvas_item;
render_mode unshaded;

uniform int step_posterize : hint_range(0, 100, 1) = 20;
uniform int step_pixelated : hint_range(0, 1000, 1) = 720;

uniform bool useBayer2 = false;

uniform vec4 rim_light_color : source_color = vec4(0.0, 0.0,0.10, 1.0);
uniform vec4 darkness_color_1 : source_color = vec4(0.0, 0.0, 0.0, 1.0);
uniform vec4 darkness_color_2 : source_color = vec4(0.0, 0.0, 0.10, 1.0);

uniform float threshold_1 :hint_range(0.0, 0.1, 0.0001) = 0.125;
uniform float threshold_2 :hint_range(0.0, 1.0, 0.0001) = 1.0;
uniform float darkness_scale :hint_range(0.0, 10.0, 0.0001) = 1.0;
uniform float darkness_adjust_plus :hint_range(0.0, 10.0, 0.0001) = 1.0;

uniform float dither_scale : hint_range(0.0, 1.0, 0.0001) = 1.0;


uniform float levels : hint_range(1, 32, 1.0) = 4.0; // color quantization
uniform sampler2D screen_texture : hint_screen_texture, repeat_disable, filter_nearest;

// 4x4 bayer matrix for ordered dithering
float bayer(vec2 p, float scale) {
    int x = int(mod(p.x * scale, 4.0));
    int y = int(mod(p.y * scale, 4.0));
    int index = x + y * 4;

    float m[16] = float[16](
        0.0,  8.0,  2.0, 10.0,
        12.0, 4.0, 14.0, 6.0,
        3.0, 11.0, 1.0, 9.0,
        15.0, 7.0, 13.0, 5.0
    );

	//returns 0 - 1 based on the index
	//this is the threshold for the pixel
    return m[index] / 16.0;
}

float bayer2(vec2 p, float scale) 
{
    int x = int(mod(p.x * scale, 2.0));
    int y = int(mod(p.y * scale, 2.0));
    int index = x + y * 2;

    float m[4] = float[4](
        0.0,  2.0,
        3.0, 1.0
    );

    return m[index] / 4.0;
}

void fragment() {
	vec4 color = texture(screen_texture, SCREEN_UV);
	float bayer_threshold = bayer(FRAGCOORD.xy, dither_scale);
	if(useBayer2){
		bayer_threshold = bayer2(FRAGCOORD.xy, dither_scale);
	}
   	float quantized_color_a = round(color.a * levels) / levels;
	
	//light rim 
	if(quantized_color_a > 0.5)
	{
		//the inside rim is 0.5-1.0
		//to get the normalized value let's stretch it out to 0-1
		float normalized_rim = (quantized_color_a * 2.0) - 1.0;
		COLOR.rgb = rim_light_color.rgb;
		if(normalized_rim > bayer_threshold)
		{
			COLOR.a = 0.0;
		}
		else
		{
			COLOR.a = 1.0;	
		}
	}
	else //Darkness color
	{
		//the darkeness alpha is is 0.0-0.5
		//to get the normalized value let's stretch it out to 0-1
		float normalized_darkness = (quantized_color_a * 2.0);
		if(normalized_darkness > (bayer_threshold))
		{
			COLOR.rgb = darkness_color_2.rgb;
		}
		else
		{
			COLOR.rgb = darkness_color_1.rgb;
		}
		COLOR.a = 1.0;
	}
}

//void light() {
//	// Called for every pixel for every light affecting the material.
//	// Uncomment to replace the default light processing function with this one.
//}
