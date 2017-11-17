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
// --> Buffer B: Number textures "1", "9", "8", "4"

// Thanks to IQ, see http://iquilezles.org/www/articles/smin/smin.htm
float smin(in float a, in float b, in float k )
{
    float res = exp( -k*a ) + exp( -k*b );
    return -log( res )/k;
}

float one(in vec2 uv)
{
    float d = length((uv - vec2(131.0, 186.1)) / vec2(15.4, 9.5)) - 1.0;
    d = min(d, length((uv - vec2(131.0, 65.1)) / vec2(15.4, 9.5)) - 1.0);
    d = min(d, max(abs(uv.x - 131.0) / 15.4, abs(uv.y - 125.5) / 60.6) - 1.0);
    return smoothstep(0.03, 0.0, d);
} 

float nine(in vec2 uv)
{
    float d1 = 1.0 - length((uv - vec2(128.5, 162.9)) / vec2(20.2, 24.2));
    float d2 = 1.0 - length((uv - vec2(128.8, 118.9)) / vec2(27.4, 39.2));
    float d3 = 1.0 - max(abs(uv.x - 127.4) / 68.2, abs(uv.y - 131.4) / 37.6);
    
    float d = max(d1, max(d2, -d3));
    d = max(d, length((uv - vec2(153.8, 131.5)) / vec2(28.3, 69.2)) - 1.0);
    d = min(d, 
            min(length((uv - vec2(91.7, 92.9)) / vec2(15.7, 8.9)) - 1.0, 
                max(
                    length((uv - vec2(126.5, 100.0)) / vec2(51.3, 44.0)) - 1.0, 
                    max(d2, d3)
                   )
            )
        );
    d = min(d, max(length((uv - vec2(129.4, 159.7)) / vec2(49.0, 42.9)) - 1.0, d1));
    d = min(d, max(length((uv - vec2(127.5, 127.5)) / vec2(127.5, 127.5)) - 1.0, 1.0 - length((uv - vec2(127.5, 127.5)) / vec2(107.5, 107.5))));
    
    return smoothstep(0.001, -0.00, d);
}    

float eight(in vec2 uv)
{
    float d = length((uv - vec2(129.3, 163.5)) / vec2(49.1, 39.2)) - 1.0;
    d = smin(d, length((uv - vec2(129.3, 97.7)) / vec2(52.8, 41.8)) - 1.0, 16.0);

    d = max(d, 1.0 - length((uv - vec2(129.3, 161.9)) / vec2(17.7, 21.5)));
    d = max(d, 1.0 - length((uv - vec2(129.3, 97.3)) / vec2(19.5, 25.2)));
    
    return smoothstep(0.03, 0.0, d);
}  

float four(in vec2 uv)
{
    float d = length((uv - vec2(150.5, 191.9)) / vec2(18.8, 9.5)) - 1.0;
    d = min(d, length((uv - vec2(94.3, 102.2)) / vec2(14.9, 8.4)) - 1.0);
    d = min(d, length((uv - vec2(168.0, 104.1)) / vec2(14.9, 6.9)) - 1.0);
    d = min(d, length((uv - vec2(153.9, 65.9)) / vec2(15.4, 9.5)) - 1.0);
    d = min(d, max(abs(uv.x - 153.9) / 15.4, abs(uv.y - 128.9) / 62.9) - 1.0);
    d = min(d, max(abs(uv.x - 131.8) / 37.5, abs(uv.y - 102.2) / 8.4) - 1.0);

    d = min(d, max(abs(uv.x - (120.8 + 52.4 * (uv.y - 150.0) / 89.7)) / 14.5, abs(uv.y - 149.6) / 44.8) - 1.0);
    
    return smoothstep(0.03, 0.0, d);
}    
  
void mainImage(out vec4 fragColor, in vec2 fragCoord )
{
    vec2 uv = fragCoord * 512.0 / iResolution.xy;
    
    fragColor.rgb = vec3(
        one(uv - vec2(0.0, 0.0)) +
        nine(uv - vec2(256.0, 0.0)) +
        eight(uv - vec2(0.0, 256.0)) +
        four(uv - vec2(256.0, 256.0))
    );
}