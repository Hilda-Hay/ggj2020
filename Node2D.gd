extends Node2D

var sprite
var rotation_speed = 21
var screwing
var unscrewing
var attached
var screw_duration
var screw_timer
var base_scale
var scale_multiplier

# Called when the node enters the scene tree for the first time.
func _ready():
	sprite = get_node("./Sprite")
	screwing = false
	unscrewing = false
	attached = true
	screw_duration = 1.5
	screw_timer = 0
	base_scale = sprite.scale
	scale_multiplier = 1

func _process(delta):
	if unscrewing:
		sprite.rotation = sprite.rotation + rotation_speed * delta
		sprite.scale = base_scale * scale_multiplier
		scale_multiplier += delta * 0.3
		screw_timer += delta
		if screw_timer >= screw_duration:
			unscrewing = false
			sprite.rotation = 0
			scale_multiplier = 1
			sprite.set_scale(base_scale)
			attached = false
			sprite.visible = false
	if screwing:
		sprite.rotation = sprite.rotation - rotation_speed * delta
		sprite.scale = base_scale * scale_multiplier
		scale_multiplier -= delta * 0.3
		screw_timer += delta
		if screw_timer >= screw_duration:
			screwing = false
			sprite.rotation = 0
			scale_multiplier = 1
			sprite.set_scale(base_scale)

func _input_event(viewport, event, shape_idx):
	if event is InputEventMouseButton:
		if event.button_index == BUTTON_LEFT && event.pressed == true:
			if !screwing && !unscrewing:
				if attached:
					unscrewing = true
					screw_timer = 0
				else:
					attached = true
					sprite.visible = true
					screwing = true
					screw_timer = 0
					scale_multiplier = 1.45
