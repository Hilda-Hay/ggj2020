extends KinematicBody2D

# Player movement speed
export var speed = 75

# Player dragging flag
var drag_enabled = false
var drag_offset = Vector2(0,0)

var part_layer = 0

var snap_point# = Vector2(0, 0)

#var collision_area

func _ready():
	snap_point = position

func _physics_process(delta):
	# Get player input
	var direction: Vector2
	direction.x = Input.get_action_strength("ui_right") - Input.get_action_strength("ui_left")
	direction.y = Input.get_action_strength("ui_down") - Input.get_action_strength("ui_up")
	
	# If input is digital, normalize it for diagonal movement
	if abs(direction.x) == 1 and abs(direction.y) == 1:
		direction = direction.normalized()
	
	# Calculate movement
	var movement = speed * direction * delta
	
	# If dragging is enabled, use mouse position to calculate movement
	if drag_enabled:
		var new_position = get_global_mouse_position()
		var snap_offset = new_position - snap_point + drag_offset
		if snap_offset.length() < 30:
			new_position = snap_point - drag_offset
		movement = new_position - position + drag_offset
		#if movement.length() > (speed * delta):
		#	movement = speed * delta * movement.normalized()
	
	# Apply movement
	move_and_collide(movement)

func _input_event(viewport, event, shape_idx):
	if event is InputEventMouseButton:
		if event.button_index == BUTTON_LEFT:
			#if get_node("./Area2D").get_overlapping_areas().size() == 0:
			if !check_covered():
				drag_enabled = event.pressed
				drag_offset = position - get_global_mouse_position()
				

func check_covered():
	var overlapping_parts = get_node("./Area2D").get_overlapping_areas()
	var covered = false;
	for item in overlapping_parts:
		if item.get_parent().part_layer < part_layer:
			covered = true
	return covered

func _input(event):
	if event is InputEventMouseButton:
		if event.button_index == BUTTON_LEFT and not event.pressed:
			drag_enabled = false

#func set_coverable(c):
#	coverable = c

func _process(delta):
	get_node("./SnapPointSprite").position = snap_point - position
