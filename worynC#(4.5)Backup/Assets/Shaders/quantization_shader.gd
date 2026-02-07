@tool
extends CompositorEffect
class_name QuantizationShader

var rd : RenderingDevice
var shader : RID
var pipeline : RID

@export var color_depth : float = 256.0



func _init() -> void:
	RenderingServer.call_on_render_thread(initialize_computer_shader)

func _notification(what: int) -> void:
	if what == NOTIFICATION_PREDELETE and shader.is_valid(): 
		RenderingServer.free_rid(shader)

func _render_callback(_callback_type: int, render_data: RenderData) -> void:
	if not rd: return
	
	var scene_buffers : RenderSceneBuffersRD = render_data.get_render_scene_buffers()
	var scene_data  :RenderSceneData = render_data.get_render_scene_data()
	if not scene_buffers or not scene_data: return
	
	var size : Vector2i = scene_buffers.get_internal_size()
	if size.x == 0 or size.y == 0 : return
	
	@warning_ignore("integer_division")
	var x_groups : int = size.x / 8 + 1
	@warning_ignore("integer_division")
	var y_groups : int = size.y / 8 + 1
	
	var push_constants : PackedFloat32Array = PackedFloat32Array()
	push_constants.append(size.x)
	push_constants.append(size.y)
	push_constants.append(color_depth)
	push_constants.append(0.0)
	
	
	for view in scene_buffers.get_view_count():
		var screen_tex : RID = scene_buffers.get_color_layer(view)
		
		var uniform : RDUniform = RDUniform.new()
		uniform.uniform_type = RenderingDevice.UNIFORM_TYPE_IMAGE
		uniform.binding = 0
		uniform.add_id(screen_tex)
		
		var image_uniform_set : RID = UniformSetCacheRD.get_cache(shader, 0, [uniform])
		
		var compute_list : int = rd.compute_list_begin()
		rd.compute_list_bind_compute_pipeline(compute_list, pipeline)
		rd.compute_list_bind_uniform_set(compute_list, image_uniform_set, 0)
		rd.compute_list_set_push_constant(compute_list, push_constants.to_byte_array(), push_constants.size()*4)
		rd.compute_list_dispatch(compute_list, x_groups, y_groups, 1)
		rd.compute_list_end()

func initialize_computer_shader() -> void:
	rd = RenderingServer.get_rendering_device()
	if not rd : return
	
	var glsl_file : RDShaderFile = load("res://Assets/Shaders/quantization_compute.glsl")
	shader = rd.shader_create_from_spirv(glsl_file.get_spirv())
	pipeline = rd.compute_pipeline_create(shader)
