#[compute]
#version 450

layout(local_size_x = 8, local_size_y = 8, local_size_z = 1) in;

layout(rgba16, binding = 0, set = 0) uniform image2D screen_tex;

layout(push_constant, std430) uniform Params {
    vec2 screen_size;
    float num_colors;
} p;

// Convert RGB to HSV
vec3 rgb_to_hsv(vec3 color) {
    float c_max = max(color.r, max(color.g, color.b));
    float c_min = min(color.r, min(color.g, color.b));
    float delta = c_max - c_min;

    float h = 0.0;
    if (delta > 0.0) {
        if (c_max == color.r) {
            h = mod((color.g - color.b) / delta, 6.0);
        } else if (c_max == color.g) {
            h = (color.b - color.r) / delta + 2.0;
        } else {
            h = (color.r - color.g) / delta + 4.0;
        }
        h /= 6.0;
    }

    float s = c_max == 0.0 ? 0.0 : delta / c_max;
    return vec3(h < 0.0 ? h + 1.0 : h, s, c_max);
}

// Convert HSV to RGB
vec3 hsv_to_rgb(vec3 hsv) {
    float h = hsv.x * 6.0;
    float c = hsv.z * hsv.y;
    float x = c * (1.0 - abs(mod(h, 2.0) - 1.0));
    vec3 rgb;

    if (0.0 <= h && h < 1.0) rgb = vec3(c, x, 0.0);
    else if (1.0 <= h && h < 2.0) rgb = vec3(x, c, 0.0);
    else if (2.0 <= h && h < 3.0) rgb = vec3(0.0, c, x);
    else if (3.0 <= h && h < 4.0) rgb = vec3(0.0, x, c);
    else if (4.0 <= h && h < 5.0) rgb = vec3(x, 0.0, c);
    else rgb = vec3(c, 0.0, x);

    float m = hsv.z - c;
    return rgb + vec3(m);
}

void main() {
    ivec2 pixel = ivec2(gl_GlobalInvocationID.xy);
    vec2 size = p.screen_size;
    
    if (pixel.x >= size.x || pixel.y >= size.y) return;

    // Load the pixel color
    vec4 color = imageLoad(screen_tex, pixel);

    // Convert RGB to HSV
    vec3 hsv = rgb_to_hsv(color.rgb);

    // Quantize HSV channels
    float levels = p.num_colors - 1.0; // Map values to 0..levels
    hsv.x = round(hsv.x * levels) / levels; // Hue (wraps around 1.0)
    hsv.y = round(hsv.y * levels) / levels; // Saturation
    hsv.z = round(hsv.z * levels) / levels; // Value (Brightness)

    // Convert back to RGB
    vec3 quantized_rgb = hsv_to_rgb(hsv);

    // Store the quantized color back to the texture
    imageStore(screen_tex, pixel, vec4(quantized_rgb, color.a));
}