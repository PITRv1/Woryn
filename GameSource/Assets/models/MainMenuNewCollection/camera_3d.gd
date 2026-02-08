extends Camera3D

@export var max_yaw_deg: float = 45.0     # Left / Right rotation
@export var max_pitch_deg: float = 30.0   # Up / Down rotation
@export var rotation_speed: float = 5.0   # Smoothing

var _base_rotation: Vector3

func _ready() -> void:
	Input.mouse_mode = Input.MOUSE_MODE_VISIBLE
	_base_rotation = rotation

func _process(delta: float) -> void:
	var viewport := get_viewport()
	var viewport_size: Vector2 = viewport.get_visible_rect().size
	var mouse_pos: Vector2 = viewport.get_mouse_position()

	var normalized := (mouse_pos / viewport_size) * 2.0 - Vector2.ONE

	normalized.x = clamp(normalized.x, -1.0, 1.0)
	normalized.y = clamp(normalized.y, -1.0, 1.0)

	var target_yaw := deg_to_rad(max_yaw_deg) * -normalized.x
	var target_pitch := deg_to_rad(max_pitch_deg) * -normalized.y

	var target_rotation := Vector3(
		target_pitch,
		target_yaw,
		0.0
	) + _base_rotation
	
	rotation = rotation.lerp(target_rotation, delta * rotation_speed)
