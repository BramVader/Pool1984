// **** POOL1984 ****
// Created by Observer (Bram Vader)
// License Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
//
// This shader is the result of a project that attempts to recreate the "Pixar 1984 pool ball shot", mentioned 
// in the paper "Distributed Ray Tracing" by Robert L. Cook, Thomas Porter and Loren Carpenter (1984) which can
// be found here: https://graphics.pixar.com/library/DistributedRayTracing/.
//
// The principles described in this paper are used to recreate the picture, which can be found at the end of the
// paper and also on the web page as a thumbnail.  The thumbnail, which is an image of 778x669 pixels is the best 
// quality we have, as far as I know. Another paper "Stochastic Sampling in Computer Graphics" by Robert L. Cook 
// (1986) contains a close-up of "1984" which is used as well to better reproduce a part of the environment map.
//
// To calculate all positions, lights, textures and animations, a C#-program Pool1984.exe was made, which can be 
// found on https://github.com/BramVader/Pool1984.
// The most time-consuming part was creating Buffer A that contains the cubemap that reflects the environment, 
// mirrored in the balls.
//
// --> Buffer C: Cloth-texture

const float PI = acos(0.0) * 2.0;

float hash12n(vec2 p) {
    p = fract(p * vec2(5123.3987, 5151.4321));
    p += dot(p.yx, p.xy + vec2(21.5351, 14.3137));
    return fract(p.x * p.y * 95.4323);
}

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