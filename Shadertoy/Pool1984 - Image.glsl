const float PI = acos(0.0) * 2.0;
const float MINDIST = 1E-5;
const float MAXDIST = 1E12;
const float TEXTUREANGLE = 0.55;
const vec3 AMBIENT = vec3(0.01);
const float REFL = 0.4;
const float MAXITER = 3.0;

const float SAMPLEX = 4.0;
const float SAMPLEY = 4.0;
const float SAMPLES = SAMPLEX * SAMPLEY;

struct Intersection{
	float dist; // Distance to object
	vec3 pos; // Intersection position
	vec3 nrm; // Surface normal
	vec3 tnrm; // Transformed normal
	bool hit; // Object is hit
	int obj; // Object number: 0: 1-Ball, 1: 9-Ball, 2: 8-Ball, 3: 4-Ball, 4: White ball, 5: Felt
	vec2 txtOffset; // Texture offset for number texture (Buf B)
};

struct Ray{
	vec3 ro;
	vec3 rd;
};

struct Camera{
	vec3 from;
	vec3 at;
	vec3 up;
	vec2 aper;

	vec3 look;
	vec3 hor;
	vec3 ver;
};

vec3[] lightPos = vec3[](vec3(-46.041, 10.783, 28.471), vec3(-11.662, 13.942, 34.967), vec3(27.862, 12.144, 27.498));
float[] lightRad1 = float[](2.985, 4.437, 3.713);
float[] lightRad2 = float[](4.895, 5.677, 4.775);
vec3[] entityColor = vec3[](vec3(1.537, 1.43, 0.185), vec3(1.447, 1.547, 1.024), vec3(0.0, 0.0, 0.0), vec3(0.96, 0.395, 0.921), vec3(1.447, 1.547, 1.024), vec3(0.0, 0.05, 0.0));

float hash12n(vec2 p) {
	p = fract(p * vec2(5123.3987, 5151.4321));
	p += dot(p.yx, p.xy + vec2(21.5351, 14.3137));
	return fract(p.x * p.y * 95.4323);
}


mat3 RotateAxisXY(vec3 axis, float angle)
{
    float sn = sin(angle);
    float cs = cos(angle);
    float tc = 1.0 - cs;
    return mat3(
		vec3(tc * axis.x * axis.x + cs, tc * axis.x * axis.y, sn * axis.y),
        vec3(tc * axis.x * axis.y, tc * axis.y * axis.y + cs, -sn * axis.x),
        vec3(-sn * axis.y, sn * axis.x, cs)
    );
}

vec3 GetBall1Pos (in float t)
{
    return vec3(-0.278, 3.745, 1.0);
}

vec3 GetBall1TextureTransformation (in float t, in vec3 v)
{
    return v * mat3(vec3(0.019, -0.412, 0.911), vec3(-0.353, 0.85, 0.392), vec3(-0.936, -0.329, -0.129));
}

vec3 GetBall9Pos (in float t)
{
    return t < 0.33 ?
        vec3(0.024, 0.731, 1.0) + t * vec3(0.19, 0.082, 0.0) / 0.33 : 
        vec3(0.214, 0.813, 1.0) + (t - 0.33) * vec3(0.177, 0.37, 0.0) / 0.67;
}

vec3 GetBall9TextureTransformation (in float t, in vec3 v)
{
    return t < 0.33 ?
        v * RotateAxisXY(vec3(0.395, -0.919, 0.0), 0.207 * t / 0.33) * mat3(vec3(0.028, 0.103, 0.978), vec3(0.138, -0.994, 0.131), vec3(0.957, 0.055, 0.276)) : 
        v * RotateAxisXY(vec3(0.902, -0.433, 0.0), 0.41 * (t - 0.33) / 0.67) * mat3(vec3(0.034, 0.184, 0.978), vec3(0.134, -0.981, 0.189), vec3(0.975, 0.07, 0.089));
}

vec3 GetBall8Pos (in float t)
{
    return t < 0.33 ?
        vec3(2.047, -0.078, 1.0) : 
        t < 0.735 ?
            vec3(2.047, -0.078, 1.0) + (t - 0.33) * vec3(0.238, -0.087, 0.0) / 0.405 : 
            vec3(2.285, -0.166, 1.0);
}

vec3 GetBall8TextureTransformation (in float t, in vec3 v)
{
    return t < 0.33 ?
        v * mat3(vec3(-0.29, -0.038, 0.956), vec3(0.003, -0.999, -0.038), vec3(0.957, -0.008, 0.29)) : 
        t < 0.735 ?
            v * RotateAxisXY(vec3(-0.344, -0.939, 0.0), 0.253 * (t - 0.33) / 0.405) * mat3(vec3(-0.29, -0.038, 0.956), vec3(0.003, -0.999, -0.038), vec3(0.957, -0.008, 0.29)) : 
            v * mat3(vec3(-0.275, -0.123, 0.991), vec3(-0.008, -0.992, -0.124), vec3(0.932, -0.024, 0.055));
}

vec3 GetBall4Pos (in float t)
{
    return t < 0.735 ?
        vec3(3.891, -1.358, 1.0) : 
        vec3(3.891, -1.358, 1.0) + (t - 0.735) * vec3(0.135, -0.097, 0.0) / 0.265;
}

vec3 GetBall4TextureTransformation (in float t, in vec3 v)
{
    return t < 0.735 ?
        v * mat3(vec3(-0.708, 0.189, 0.681), vec3(0.127, -0.914, 0.385), vec3(0.695, 0.359, 0.623)) : 
        v * RotateAxisXY(vec3(-0.585, -0.811, 0.0), 0.166 * (t - 0.735) / 0.265) * mat3(vec3(-0.708, 0.189, 0.681), vec3(0.127, -0.914, 0.385), vec3(0.695, 0.359, 0.623));
}

vec3 GetBallwPos (in float t)
{
    return vec3(-2.645, -0.519, 1.0) + t * vec3(-0.028, 0.088, 0.0);
}

vec3 GetBallwTextureTransformation (in float t, in vec3 v)
{
    return v;
}

Camera GetCamera() {
	Camera camera;
    
    if (iMouse.w < 0.0)
    {
        camera.from = vec3(0.0, 0.0, 33.7);
        camera.at = vec3(0.0, 0.0, 0.0);
        camera.up = vec3(0.817, 0.576, 0.0);
        camera.aper = vec2(7.827, 6.646);
    }
    else
    {
        float dst = 10.0;
        camera.from = vec3(
            dst * cos(iMouse.x * PI * 2.0 / iResolution.x) * cos(iMouse.y * PI * 0.499 / iResolution.y),
            dst * sin(iMouse.x * PI * 2.0 / iResolution.x) * cos(iMouse.y * PI * 0.499 / iResolution.y),
            0.01 + dst * sin(iMouse.y * PI * 0.5 / iResolution.y));
        camera.at = vec3(0.0, 0.0, 0.5);
        camera.up = vec3(0.0, 0.0, 1.0);
        camera.aper = vec2(30.0);
    }

	camera.look = camera.at - camera.from;
	float dist = length(camera.look);
	float hsize = tan(camera.aper.x * PI / 180.0) * dist;
	float vsize = tan(camera.aper.y * PI / 180.0) * dist;
	if (hsize * iResolution.x / iResolution.y > vsize)
		hsize = vsize * iResolution.x / iResolution.y;
	else
		vsize = hsize * iResolution.y / iResolution.x;
	camera.hor = normalize(cross(camera.look, camera.up)) * hsize;
	camera.ver = normalize(cross(camera.hor, camera.look)) * vsize;

	return camera;
}

Ray GetCameraRay(in Camera camera, in vec2 fragCoord) {
	Ray ray;
	ray.ro = camera.from;
	ray.rd = normalize(camera.look +
			(fragCoord.x / iResolution.x * 2.0 - 1.0) * camera.hor +
			(fragCoord.y / iResolution.y * 2.0 - 1.0) * camera.ver);
	return ray;
}

Intersection GetBallIntersection(Ray ray, vec3 center, float radius, float minDist, float maxDist, int obj) {
	Intersection intsec;
	intsec.obj = obj;
	intsec.dist = MAXDIST;
	vec3 ct = ray.ro - center;
	float b = 2.0 * dot(ray.rd, ct);
	float c = dot(ct, ct) - radius * radius;
	float d = b * b - 4.0 * c;
	if (d >= 0.0) {
		intsec.dist = (-b - sqrt(d)) * 0.5;
		intsec.hit = intsec.dist > minDist && intsec.dist < maxDist;
		intsec.pos = ray.ro + intsec.dist * ray.rd;
		intsec.nrm = (intsec.pos - center) / radius;
	}
	return intsec;
}

Intersection GetPlaneIntersection(Ray ray, vec3 center, vec3 normal, float minDist, float maxDist, int obj) {
	Intersection intsec;
	intsec.obj = obj;
	intsec.dist = MAXDIST;
	vec3 ct = ray.ro - center;
	float k = dot(ray.rd, normal);
	if (k != 0.0) {
		intsec.dist = -dot(ct, normal) / k;
		intsec.hit = intsec.dist > minDist && intsec.dist < maxDist;
		intsec.pos = ray.ro + intsec.dist * ray.rd;
		intsec.nrm = normal;
	}
	return intsec;
}

float GetBallShadow(Ray ray, vec3 center, float radius, float minDist, float maxDist) {
	vec3 ct = ray.ro - center;
	float b = 2.0 * dot(ray.rd, ct);
	float c = dot(ct, ct) - radius * radius;
	float d = b * b - 4.0 * c;
	if (d >= 0.0) {
		float dist = (-b - sqrt(d)) * 0.5;
		return 1.0 - step(minDist, dist) * step(dist, maxDist);
	}
	return 1.0;
}

Intersection GetIntsec(Ray ray, float t) {
	Intersection intsec1, intsec2;
   
    intsec1 = GetBallIntersection(ray, GetBall1Pos(t), 1.0, MINDIST, MAXDIST, 0);
	if (intsec1.hit) {
		intsec1.tnrm = GetBall1TextureTransformation(t, intsec1.nrm);
		intsec1.txtOffset = vec2(0.0, 0.0);
	}
	intsec2 = GetBallIntersection(ray, GetBall9Pos(t), 1.0, MINDIST, MAXDIST, 1);
	if (intsec2.hit && intsec2.dist < intsec1.dist) {
		intsec1 = intsec2;
		intsec1.tnrm = GetBall9TextureTransformation(t, intsec1.nrm);
		intsec1.txtOffset = vec2(0.5, 0.0);
	}
	intsec2 = GetBallIntersection(ray, GetBall8Pos(t), 1.0, MINDIST, MAXDIST, 2);
	if (intsec2.hit && intsec2.dist < intsec1.dist) {
		intsec1 = intsec2;
		intsec1.tnrm = GetBall8TextureTransformation(t, intsec1.nrm);
		intsec1.txtOffset = vec2(0.0, 0.5);
	}
	intsec2 = GetBallIntersection(ray, GetBall4Pos(t), 1.0, MINDIST, MAXDIST, 3);
	if (intsec2.hit && intsec2.dist < intsec1.dist) {
		intsec1 = intsec2;
		intsec1.tnrm = GetBall4TextureTransformation(t, intsec1.nrm);
		intsec1.txtOffset = vec2(0.5, 0.5);
	}
	intsec2 = GetBallIntersection(ray, GetBallwPos(t), 1.0, MINDIST, MAXDIST, 4);
	if (intsec2.hit && intsec2.dist < intsec1.dist) {
		intsec1 = intsec2;
	}
	intsec2 = GetPlaneIntersection(ray, vec3(0.0), vec3(0.0, 0.0, 1.0), MINDIST, MAXDIST, 5);
	if (intsec2.hit && intsec2.dist < intsec1.dist) {
		return intsec2;
	}
	return intsec1;
}

float GetBallShadow(Ray ray, float t, float maxDist) {
	if (GetBallShadow(ray, GetBall1Pos(t), 1.0, MINDIST, maxDist) < 0.5)
		return 0.0;
	if (GetBallShadow(ray, GetBall9Pos(t), 1.0, MINDIST, maxDist) < 0.5)
		return 0.0;
	if (GetBallShadow(ray, GetBall8Pos(t), 1.0, MINDIST, maxDist) < 0.5)
		return 0.0;
	if (GetBallShadow(ray, GetBall4Pos(t), 1.0, MINDIST, maxDist) < 0.5)
		return 0.0;
	if (GetBallShadow(ray, GetBallwPos(t), 1.0, MINDIST, maxDist) < 0.5)
		return 0.0;
	return 1.0;
}

// Returns 0..4, 0..1 (0..1, 0..1 for each side - no bottom & top sides)
vec2 DirToCube(vec3 rd) {
    float plane = mix(step(0.0, rd.x), step(rd.y, 0.0) + 0.5, step(abs(rd.x), abs(rd.y))) * 2.0;
    vec2 v;
    switch (int(plane))
    {
        case 0:
            v = vec2(-rd.y / rd.x, -rd.z / rd.x);
            break;
        case 1:
            v = vec2(rd.x / rd.y, rd.z / rd.y);
            break;
        case 2:
            v = vec2(-rd.y / rd.x, rd.z / rd.x);
            break;
        case 3:
            v = vec2(rd.x / rd.y, -rd.z / rd.y);
            break;
    }
    return v * 0.5 + vec2(0.5 + plane, 0.5);
}

vec3 GetCubemapYellow(in vec2 uv) {
	float x1[] = float[](1014.9, 762.6, 637.0, 409.6, 329.4, 261.2);
	float y1[] = float[](123.8, 133.7, 111.7, 123.5, 122.2, 130.4);
	float x2[] = float[](776.4, 666.0, 571.0, 466.4, 397.9, 321.8);
	float y2[] = float[](186.5, 161.9, 175.0, 183.4, 184.2, 167.5);

	for (int part = 0; part < 6; part++) {
		float x = (uv.x - x1[part]) * 255.0 / (x2[part] - x1[part]);
		if (x >= -10.0 && x <= 265.0) {
			float y = (uv.y - y1[part]) * 255.0 / (y2[part] - y1[part]);
			if (y >= -10.0 && y <= 265.0) {
				return texture(iChannel0, (vec2(x, y) + vec2(22.0)) / vec2(900.0, 600.0) + vec2(float(part % 3) / 3.0, float(part / 3) / 2.0),
					1.0).rgb;
			}
		}
	}
	return vec3(0.0);
}

vec3 GetCubemapPurple(in vec2 uv) {
	float x1[] = float[](1015.3, 763.6, 622.5, 396.5, 316.5, 246.7);
	float y1[] = float[](121.4, 131.5, 113.8, 124.6, 124.1, 132.4);
	float x2[] = float[](776.8, 667.0, 556.4, 453.3, 384.9, 307.2);
	float y2[] = float[](184.1, 159.7, 177.1, 184.4, 186.1, 169.4);
	for (int part = 0; part < 6; part++) {
		float x = (uv.x - x1[part]) * 255.0 / (x2[part] - x1[part]);
		if (x >= -10.0 && x <= 265.0) {
			float y = (uv.y - y1[part]) * 255.0 / (y2[part] - y1[part]);
			if (y >= -10.0 && y <= 265.0) {
				return texture(iChannel0, (vec2(x, y) + vec2(22.0)) / vec2(900.0, 600.0) + vec2(float(part % 3) / 3.0, float(part / 3) / 2.0),
					1.0).rgb;
			}
		}
	}
	return vec3(0.0);
}

vec3 GetFabricColor(vec2 uv) {
    return texture(iChannel2, mod(uv * 0.08, 1.0)).rgb;
}

vec3 Render(Ray ray, int nr, float t) {
	vec3 result = vec3(0.0);
    float refl = 1.0;

    for (float iter = 0.0; iter <= MAXITER; iter++)
    {
        Intersection intsec;
        Ray mray, sray; // Shadow & Mirror rays

        intsec = GetIntsec(ray, t);
        if (intsec.hit) {
            // Calculate mirror ray
            float a = -dot(intsec.nrm, ray.rd);
            mray.ro = intsec.pos;
            mray.rd = ray.rd + 2.0 * a * intsec.nrm;

            vec3 color = entityColor[intsec.obj];
            if (intsec.obj < 4) // Balls with texture
            {
                intsec.tnrm = normalize(intsec.tnrm);
                vec2 uv = vec2(
                        atan(intsec.tnrm.y, intsec.tnrm.x) / TEXTUREANGLE,
                        atan(intsec.tnrm.z, length(intsec.tnrm.xy)) / TEXTUREANGLE);
                float c1 = step(length(uv), 1.0);
                float c2 = step(float(intsec.obj), 1.0) * step(abs(intsec.tnrm.x), 0.5);
                color =
                    color * (1.0 - c1 - c2) +
                    c1 * mix(entityColor[1], entityColor[2], texture(iChannel1, uv * 0.25 + vec2(0.25) + intsec.txtOffset).r) +
                    c2 * entityColor[0]; // Band color is the same as of the yellow ball
            } 
            else if (intsec.obj == 5 && iter == 0.0) // Cloth
            {
                color = GetFabricColor(intsec.pos.xy);
            }
            for (int n = 0; n < lightPos.length(); n++) {
                vec3 ltVec1 = lightPos[n] - intsec.pos;
                float a = hash12n(ray.rd.xy) * PI * 2.0;
                float b = hash12n(ray.rd.yx) * PI * 2.0;

                vec3 ltVec3 = ltVec1 + vec3(cos(a) * cos(b), sin(a) * cos(b), sin(b)) * lightRad1[n];

                float lightDist = length(ltVec3);

                Ray sray;
                sray.ro = intsec.pos;
                sray.rd = ltVec3 / lightDist;

                float shadow = GetBallShadow(sray, t, lightDist + 0.01);

                float specInt = 0.0;
                if (intsec.obj < 5) // All balls
                {
                    float k =
                        dot(lightPos[n] - mray.ro, ltVec3) /
                        dot(mray.rd, ltVec3);
                    vec3 v = mray.ro + k * mray.rd - lightPos[n];
                    specInt = k > MINDIST && k < length(ltVec1) + lightRad2[n] ?
                        max(0.0, min(1.0, (length(v) - lightRad2[n]) / (lightRad1[n] - lightRad2[n]))) :
                        0.0;
                }
                float i = max(0.0, dot(ltVec3 / lightDist, intsec.nrm)) * 0.333 * shadow;
                float specularIntensity = specInt * shadow;

                result = result + refl * (i * color + specularIntensity + AMBIENT);
            }
        } 
        else 
        {
            float jitter = (1.0 - iter) * 0.01;
            float a = hash12n(ray.rd.xy) * jitter;
            float b = hash12n(ray.rd.yx) * jitter;
            vec3 hor = normalize(cross(ray.rd, vec3(ray.rd.y, 0.0, ray.rd.x)));
            vec3 ver = normalize(cross(hor, ray.rd));
            
            vec2 coord = DirToCube(ray.rd + a * hor + b * ver );
            result = result + refl * GetCubemapYellow(coord * 255.0);
        }
        
        // Initialize for next iteration
        refl = intsec.obj < 5 ? REFL : 0.0;
        if (refl < 0.1) break;
        ray = mray;
    }
	return result;
}

void mainImage(out vec4 fragColor, in vec2 fragCoord) {
    //fragColor.rgb = GetCubemapYellow(vec2(fragCoord.x / iResolution.x * 1024.0, fragCoord.y / iResolution.y * 255.0));
    //fragColor.rgb = texture(iChannel1, (fragCoord / iResolution.yy) * vec2(0.5, 0.5) + vec2(0.5, 0.0)).rgb;
    fragColor.rgb = texture(iChannel2, (fragCoord / iResolution.xy)).rgb;
    //return;
    
	Camera camera = GetCamera();
	vec3 color = vec3(0.0);
	for (float smpx = 0.0; smpx < SAMPLEX; smpx += 1.0) 
    {
		for (float smpy = 0.0; smpy < SAMPLEX; smpy += 1.0) 
        {  
            // time: 0..1 with jitter
            float time = mod(iTime * 3.0, 5.0) - 2.5 + ((smpx * SAMPLEY + smpy) + hash12n(fragCoord)) * 0.5 / SAMPLES;
            //float time = ((smpx * SAMPLEY + smpy) + hash12n(fragCoord)) * 1.0 / SAMPLES;
            
            // position jitter
            vec2 offs = vec2(
                (smpx + hash12n(fragCoord.yx)) / SAMPLEX,
                (smpy + hash12n(fragCoord.xy)) / SAMPLEY
            ) * 2.0;
            
            Ray ray = GetCameraRay(camera, fragCoord + offs);
            color = color + Render(ray, 1, time);
        }
	}
	fragColor.rgb = color / SAMPLES;
}
