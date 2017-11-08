const float PI = acos(0.0) * 2.0;


float hash12n(vec2 p) {
	p = fract(p * vec2(5123.3987, 5151.4321));
	p += dot(p.yx, p.xy + vec2(21.5351, 14.3137));
	return fract(p.x * p.y * 95.4323);
}

// Fabric texture
void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
    vec2 uv = vec2(
        fragCoord.x - (fragCoord.y * 34.0 / 160.0),
        fragCoord.y - (fragCoord.x * 64.0 / 147.0)
    );
    fragColor.rgb = mix(
        vec3(0.2, 0.5, 0.2),
        vec3(0.0, 0.3, 0.0),
        sin(uv.x * PI + hash12n(uv.yx * 0.04) * 0.7) * cos(uv.y * PI * 1.3) * 0.5 + 0.5 + hash12n(uv * 0.01) * 0.4
    );
}