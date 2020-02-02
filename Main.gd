extends Node2D

# Called when the node enters the scene tree for the first time.
func _ready():
	# set layers
	get_node("./RobotChest").part_layer = 1
	get_node("./RobotChestplateLeft").part_layer = 0
	get_node("./RobotChestplateRight").part_layer = 0
