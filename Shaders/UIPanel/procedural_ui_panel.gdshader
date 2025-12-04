shader_type canvas_item;

uniform vec2 aspect = vec2(1.0, 1.0);
uniform float border_size: hint_range(0.0, 0.5) = 0.1;
uniform float border_fade_in: hint_range(0.0, 10.0) = 0.0;
uniform float border_fade_out: hint_range(0.0, 10.0) = 0.0;
uniform float corner_radius: hint_range(0.0, 8.0) = 0.0;


uniform float inner_gradient_offset : hint_range(-1.5, 1.5) = 0.0;
uniform float inner_gradient_size : hint_range(0.5, 2) = 0.5;
uniform float inner_gradient_angle : hint_range(0.0, 360.0) = 0.0;

uniform vec4 inner_gradient_first_color : source_color = vec4(1.0);
uniform vec4 inner_gradient_second_color : source_color = vec4(0.0, 0.0, 0.0, 1.0);

uniform float outer_gradient_offset : hint_range(-1.5, 1.5) = 0.0;
uniform float outer_gradient_size : hint_range(0.5, 2) = 0.5;
uniform float outer_gradient_angle : hint_range(0.0, 360.0) = 0.0;

uniform vec4 outer_gradient_first_color : source_color = vec4(1.0);
uniform vec4 outer_gradient_second_color : source_color = vec4(0.0, 0.0, 0.0, 1.0);

float rectangle(vec2 position) {
	vec2 component_wise_edge_distance = abs(position) - (vec2(0.5, 0.5) * aspect + corner_radius * (aspect - 1.0));
	float outside_distance = length(max(component_wise_edge_distance, 0));
    float inside_distance = min(max(component_wise_edge_distance.x, component_wise_edge_distance.y), 0);
    return outside_distance + inside_distance;
}



void fragment() {
	COLOR.a = 0.0;
	float border_scale = 1.0 + 2.0 * corner_radius;
	float dist = rectangle((UV - 0.5) * aspect * border_scale);
	float border_width = (border_size + border_fade_in + border_fade_out) * border_scale;

	float inner_pivot = inner_gradient_offset + 0.5;
	vec2 inner_uv = UV - inner_pivot;
	float inner_rotated = inner_uv.x * cos(radians(inner_gradient_angle)) - inner_uv.y * sin(radians(inner_gradient_angle));
	float inner_pos = smoothstep((1.0 - inner_gradient_size) + inner_gradient_offset, inner_gradient_size + 0.0001 + inner_gradient_offset, inner_rotated + inner_pivot);

	vec4 inner_gradient_color = mix(
		inner_gradient_first_color,
		inner_gradient_second_color,
		inner_pos);

	float outer_pivot = outer_gradient_offset + 0.5;
	vec2 outer_uv = UV - outer_pivot;
	float outer_rotated = outer_uv.x * cos(radians(outer_gradient_angle)) - outer_uv.y * sin(radians(outer_gradient_angle));
	float outer_pos = smoothstep((1.0 - outer_gradient_size) + outer_gradient_offset, outer_gradient_size + 0.0001 + outer_gradient_offset, outer_rotated + outer_pivot);

	vec4 outer_gradient_color = mix(
		outer_gradient_first_color,
		outer_gradient_second_color,
		outer_pos);



	if (dist >= corner_radius - border_width && dist <= corner_radius) {


		COLOR.a = 0.0;

		float rel_d = (dist - corner_radius + border_width) / border_width;
		float alpha = min((rel_d / (border_fade_in * border_scale / border_width)), (1.0 - (rel_d)) / (border_fade_out * border_scale / border_width));
		if (dist < corner_radius - (border_size + border_fade_out) * border_scale) {
			COLOR = mix(inner_gradient_color, outer_gradient_color, alpha).rgba;
			COLOR.a = mix(inner_gradient_color.w, outer_gradient_color.w, alpha);
		} else {
			COLOR = outer_gradient_color.rgba;
			COLOR.a = mix(0.0, outer_gradient_color.w, alpha);
		}
	} else if (dist < corner_radius - border_width ) {
		COLOR = inner_gradient_color.rgba;
		COLOR.a = inner_gradient_color.w;
	}
}