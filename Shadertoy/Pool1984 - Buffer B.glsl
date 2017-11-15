// Thanks to IQ, see http://iquilezles.org/www/articles/smin/smin.htm
float smin( float a, float b, float k )
{
    float res = exp( -k*a ) + exp( -k*b );
    return -log( res )/k;
}

float one(vec2 uv)
{
    float d = length((uv - vec2(131.0, 186.1)) / vec2(15.4, 9.5)) - 1.0;
    d = min(d, length((uv - vec2(131.0, 65.1)) / vec2(15.4, 9.5)) - 1.0);
    d = min(d, max(abs(uv.x - 131.0) / 15.4, abs(uv.y - 125.5) / 60.6) - 1.0);
    return smoothstep(0.03, 0.0, d);
} 

float nine(vec2 uv)
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

float eight(vec2 uv)
{
    float d = length((uv - vec2(129.3, 163.5)) / vec2(49.1, 39.2)) - 1.0;
    d = smin(d, length((uv - vec2(129.3, 97.7)) / vec2(52.8, 41.8)) - 1.0, 16.0);

    d = max(d, 1.0 - length((uv - vec2(129.3, 161.9)) / vec2(17.7, 21.5)));
    d = max(d, 1.0 - length((uv - vec2(129.3, 97.3)) / vec2(19.5, 25.2)));
    
    return smoothstep(0.03, 0.0, d);
}  

float four(vec2 uv)
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
  
void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
    if (iFrame > 2) discard;
    vec2 uv = fragCoord * 512.0 / iResolution.xy;
    
    fragColor.rgb = vec3(
        one(uv - vec2(0.0, 0.0)) +
        nine(uv - vec2(256.0, 0.0)) +
        eight(uv - vec2(0.0, 256.0)) +
        four(uv - vec2(256.0, 256.0))
    );
}