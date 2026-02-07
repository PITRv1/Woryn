#[compute]
#version 450

layout(local_size_x = 8, local_size_y = 8, local_size_z = 1) in;

layout(rgba16, binding = 0, set = 0)uniform image2D screen_tex;


layout(push_constant, std430)uniform Params {
    vec2 screen_size;
    float down_scaling;
}p;

void main(){
    ivec2 pixel = ivec2(gl_GlobalInvocationID.xy);
    if (pixel.x >= int(p.screen_size.x) || pixel.y >= int(p.screen_size.y)) return;

    int block_size = int(p.down_scaling);
    ivec2 block_coord = (pixel / block_size) * block_size;

    vec4 color = imageLoad(screen_tex, block_coord);
    imageStore(screen_tex, pixel, color);
}