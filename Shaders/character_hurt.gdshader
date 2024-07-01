shader_type spatial;

render_mode unshaded, cull_disabled;

uniform bool active = false;
uniform vec4 flash_colour: source_color = vec4(1.0, 1.0, 1.0, 1.0);
uniform sampler2D tex: source_color;

void fragment()
{
	vec4 colour = texture(tex, UV);
	ALPHA = colour.a;
	
		if (active == true) 
	{
		colour = flash_colour;
	}
	
	ALBEDO = vec3(colour.r, colour.g, colour.b);
}