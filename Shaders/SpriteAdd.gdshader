shader_type canvas_item;
render_mode blend_add;

uniform float alpha_scale : hint_range(0.0, 1.0, 0.01) = 0.5;

void fragment() {
	// Called for every pixel the material is visible on.
	COLOR.a = COLOR.a * alpha_scale;
}