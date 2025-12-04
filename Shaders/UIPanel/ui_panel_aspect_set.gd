
@tool
extends Control

var last_size := Vector2(0, 0)
var calculated_aspect := Vector2()
func _process(delta):
	if last_size != size:
		if size.x > size.y:
			calculated_aspect = Vector2(size.x/size.y, 1)
		else:
			calculated_aspect = Vector2(1, size.y/size.x)
		material.set("shader_parameter/aspect", calculated_aspect)
		last_size = size
