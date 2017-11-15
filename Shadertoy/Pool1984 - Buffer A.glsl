// *** Drawing functions

// Draws a line through p1 and p2
float drawLine(in vec2 uv, in vec2 p1, in vec2 p2, in float width)
{
    vec2 v = p2 - p1;
    vec2 w = uv - p1;

    float c1 = dot(w, v);
    if (c1 <= 0.0)
        return max(distance(uv, p1) - width, 0.0);
    float c2 = dot(v, v);
    if (c2 <= c1)
        return max(distance(uv, p2) - width, 0.0);

    float b = c1 / c2;
    vec2 u = p1 + b * v;
    return max(distance(uv, u) - width, 0.0);
}

// Draws a circular arc through p1, p2 and p3
// -- still has some artifacts
float drawArc(in vec2 uv, in vec2 p1, in vec2 p2, in vec2 p3, in float width)
{
    vec2 a = (p1 + p2) * 0.5;
    vec2 dv1 = vec2(p1.y - p2.y, -p1.x + p2.x);

    vec2 b = (p2 + p3) * 0.5;
    vec2 dv2 = vec2(p2.y - p3.y, -p2.x + p3.x);

    float d = (dv1.y * dv2.x - dv1.x * dv2.y);
    float t1 = ((a.x - b.x) * dv2.y + (b.y - a.y) * dv2.x) / d;
    vec2 cntr = vec2(a.x + t1 * dv1.x, a.y + t1 * dv1.y);

    if (dot(uv - p1, p2 - p1) >= 0.0 && dot(uv - p3, p2 - p3) >= 0.0)
    {
        return max(abs(length(uv - cntr) - length(p1 - cntr)) - width, 0.0);
    }
    else
    {
        return max(min(distance(uv, p1), distance(uv, p2)) - width, 0.0);
    }
}

// *** Window part ***

// Palmtree is a bitmap of 32x48
int tree[] = int[] (
    0x00003000, 0x00003C00, 0x0000FC00, 0x0000FC00, 0x01C1FC00, 0x0FC3FC00, 0x0FF1F830, 0x3FFBF96C,
    0x7FFFFF7F, 0x7FFDFFFF, 0x3FFFF7FF, 0x2FFFFFFE, 0x03FFFFFE, 0x04FFFFE8, 0x07FFFFF0, 0x1FFFFFF0,
    0x3FFFFFFC, 0x3FFFFFFC, 0x1FBFFFFC, 0x1F8FFFFE, 0x3F1FFFFE, 0x011FFFFA, 0x000FFA78, 0x000FFCF8,
    0x000FF840, 0x000FF000, 0x000FF800, 0x000FFC00, 0x000FFC00, 0x000FF800, 0x000FF800, 0x0007F000,
    0x0007F000, 0x0007E000, 0x0003E000, 0x0003E000, 0x0003E000, 0x0003E000, 0x0003E000, 0x0003E000,
    0x0001C000, 0x0001C000, 0x0003E000, 0x0001E000, 0x0001C000, 0x0001E000, 0x0001E000, 0x0001E000
);

// Player is a bitmap of 48x60 broken into 3 32xN bitmaps
int player1[] = int[] (
    0x00000000, 0x00001E00, 0x0000FF80, 0x0001FFC0, 0x0007FFC0, 0x000FFFE0, 0x001FFFE0, 0x001FFFF0,
    0x001FFFF0, 0x003FFFF0, 0x003FFFF0, 0x003FFFF0, 0x003FFFF0, 0x001FFFF8, 0x001FFFF8, 0x001FFFFC,
    0x000FFFFC, 0x000FFFF8, 0x0007FFF8, 0x0007FFF8, 0x0003FFF0, 0x0001FFF0, 0x0001FFF0, 0x0001FFF0,
    0x0001FFF0, 0x0000FFE0, 0x0000FF00, 0x0000FF00, 0x0000FF00, 0x0001FF00, 0x0001FF00, 0x0003FF80,
    0x000FFF80, 0x001FFFC0, 0x003FFFC0, 0x007FFFC0, 0x007FFFE0, 0x01FFFFE0, 0x03FFFFF0, 0x03FFFFF0,
    0x07FFFFF0, 0x0FFFFFF8, 0x1FFFFFF8, 0x1FFFFFF8, 0x1FFFFFFC, 0x3FFFFFFC, 0x3FFFFFFE, 0x3FFFFFFE,
    0x3FFFFFFF, 0x3FFFFFFF
);
int player2[] = int[](
    0x07FFFFFF, 0x07FEFFFF, 0x07FCFFFF, 0x07FCFFFF, 0x0FF8FFFF, 0x0FF8FFFF, 0x0FF8FFFF, 0x0FF8FFFF,
    0x1FF0FFFF, 0x1FF0FFFF
);
int player3[] = int[](
    0x7C000000, 0x7E000000, 0x7F000000, 0x7F800000, 0x7FC38000, 0x7FF7C000, 0x7FFFC000, 0x7FFFC000,
    0x7FFFC000, 0x7FFFC000
);

float player(in vec2 uv)
{
    float stick =
        step(118.0 + (116.5 - 118.0) * (uv.y - 25.4) / (237.1 - 25.4), uv.x) *
        step(uv.x, 120.5 + (117.2 - 120.5) * (uv.y - 25.4) / (237.1 - 25.4)) *
        step(uv.y, 237.1);
    ivec2 iv = ivec2(vec2((uv.x - 74.6)*1.05, (204.7 - uv.y)*0.36));
    if (iv.x >= 4 && iv.x < 35 && iv.y >= 1 && iv.y < 50)
    {
        return (player1[iv.y - 1] & (1 << (35 - iv.x))) > 0 ? 1.0 : stick;
    }
    if (iv.y >= 50 && iv.y < 60)
    {
        if (iv.x >= 0 && iv.x < 31)
        {
            return (player2[iv.y - 50] & (1 << (31 - iv.x))) > 0 ? 1.0 : stick;
        }
        if (iv.x >= 31 && iv.x < 48)
        {
            return (player3[iv.y - 50] & (1 << (61 - iv.x))) > 0 ? 1.0 : stick;
        }
    }
    return stick;
}

float palmtrees(in vec2 uv)
{
    ivec2 iv = ivec2(vec2((uv.x - 207.0)*1.27, (205.0 - uv.y)*0.32));
    if (iv.x >= 0 && iv.x < 32 && iv.y >= 0 && iv.y < 48)
    {
        return (tree[iv.y] & (1 << (31 - iv.x))) > 0 ? 1.0 : 0.0;
    }
    iv = ivec2(vec2((258.6 - uv.x)*1.28, (216.9 - uv.y)*0.30));
    if (iv.x >= 0 && iv.x < 32 && iv.y >= 0 && iv.y < 48)
    {
        return (tree[iv.y] & (1 << (31 - iv.x))) > 0 ? 1.0 : 0.0;
    }
    return 0.0;
}

#define xin(a,b) step(a, uv.x) * step(uv.x, b)
#define xout(a,b) (step(uv.x, a) + step(b, uv.x))
#define yin(a,b) step(a, uv.y) * step(uv.y, b)
#define yout(a,b) (step(uv.y, a) + step(b, uv.y))

float windowPattern(in vec2 uv)
{
    return
        xin(0.0, 39.0) *( yin(0.0, 65.0) + yin(73.0, 237.0) ) +
        xin(63.0, 153.0) *( yin(39.0, 179.0) + yin(196.0, 255.0) ) *
        (max( xout(81.0, 85.0) * xout(136.0, 140.0), step(uv.y, 179.0) ) ) +

        xin(158.0, 255.0) *( yin(39.0, 212.0) + yin(226.0, 237.0) + yin(244.0, 255.0)) *
        (max( xout(174.0, 178.0) * xout(229.0, 233.0), step(uv.y, 212.0) ) );
}

vec3 windowPart(in vec2 uv)
{
    float r = player(uv);
    float s = palmtrees(uv);
    float p = windowPattern(uv);
    float q = windowPattern(uv * vec2(1.0, 1.04) - vec2(-4.0, 16.0));
    return
        vec3(0.34, 0.7, 0.9) * p * q * (1.0 - s - r) +
        vec3(1.0) * (p * (1.0 - q)) * (1.0 - r);
}

// *** Sunlight part

vec3 sunlightPart(in vec2 uv)
{
    float x1[] = float[] (0.0, 43.3, 106.5, 167.6);
    float y1[] = float[] (0.0, 29.9, 48.5, 90.4);
    float x2[] = float[] (26.8, 98.7, 156.7, 239.2);
    float y2[] = float[] (20.1, 48.5, 72.2, 237.1);

    vec4 d1 = vec4(0.0);
    float d2;

    // A chair (?)
    const vec2 dv = vec2(45.0, 86.9);
    d2 =
        xin(75.0, 142.3) * step(uv.y, 262.6) *
        max(
            step(1.0, length((uv - vec2(113.0, 263.3)) / dv)) *
            step(length((uv - vec2(113.0, 222.5)) / dv), 1.0) +
            step(1.0, length((uv - vec2(113.0, 200.8)) / dv)) *
            step(length((uv - vec2(113.0, 169.7)) / dv), 1.0) +
            step(1.0, length((uv - vec2(113.0, 148.0)) / dv)) *
            step(length((uv - vec2(113.0, 116.9)) / dv), 1.0),

            xout(82.9, 134.4) * yin(0.0, 180.0)
        );
    d1 = d2 > d1.w ? vec4(1.0, 1.0, 0.5, 1.0) * d2 : d1;

    // Sunlight projected on the wall
    vec2 tuv = vec2(uv.x - (255.0 - uv.y) * 0.06, 255.0 - uv.y - uv.x);
    for (int y = 0; y < 4; y++)
    {
        if (tuv.y >= y1[y] && tuv.y <= y2[y] && uv.y > 0.0)
        for (int x = 0; x < 4; x++)
        {
            if (tuv.x >= x1[x] && tuv.x <= x2[x + y/3])
            {
                {
                    d2 = x != 2 || y ==2  ? 0.3 + (y > 2 || y == 1 || x == 2 ? 0.5 : 0.0) : 0.0;
                    d1 = d2 > d1.w ? vec4(1.0, 1.0, 0.0, 1.0) * d2 : d1;
                }
            }
        }
    }
    return d1.rgb;
}

// *** Light spot part
vec3 lightspotPart(in vec2 uv)
{
    float r = length((uv - vec2(127.0, 230.2)) / vec2(71.7, 24.4));
    float sy = (1.0 - pow(abs(uv.x - 127.0)/127.0, 2.0)) * 188.0;
    float s = smoothstep (sy, sy - 20.0, uv.y) * pow(uv.y / 188.0, 2.0);
    float t =
       min(
        drawLine(uv, vec2(197.3, 82.7), vec2(203.5, 177.8), 2.0),
        drawLine(uv, vec2(192.3, 82.7), vec2(27.0, 99.0), 4.0)
      );
    return (max(smoothstep (1.0, 0.5, r), s)
        * (1.0 - smoothstep (6.0, 0.5, t)))
        * vec3(1.0);
}

// *** Neon sign 1 (probably displaying 'Bud light')
vec3 bud[] = vec3[](
    vec3(10.0, 104.2, 155.3), vec3(11.0, 104.3, 109.3), vec3(12.0, 136.6, 93.3),  vec3(13.0, 143.9, 97.8),  vec3(12.0, 148.4, 104.2),
    vec3(13.0, 149.5, 112.0), vec3(11.0, 149.5, 153.6), vec3(12.0, 145.7, 157.4), vec3(13.0, 141.9, 153.3), vec3(11.0, 141.4, 110.0),
    vec3(12.0, 125.7, 101.7), vec3(13.0, 117.6, 113.4), vec3(11.0, 117.5, 150.9), vec3(12.0, 115.3, 155.5), vec3(13.0, 110.4, 157.0),
    vec3(10.0, 186.9, 158.2), vec3(12.0, 194.8, 158.1), vec3(13.0, 205.2, 155.7), vec3(12.0, 213.4, 150.4), vec3(13.0, 218.6, 139.9),
    vec3(12.0, 220.1, 122.5), vec3(13.0, 215.3, 105.0), vec3(12.0, 206.9, 96.5),  vec3(13.0, 195.5, 93.2),  vec3(11.0, 174.6, 93.2),
    vec3(12.0, 171.2, 95.4),  vec3(13.0, 170.4, 99.3),  vec3(11.0, 170.4, 152.9), vec3(12.0, 175.5, 158.9), vec3(13.0, 181.2, 153.8),
    vec3(11.0, 181.5, 106.6), vec3(12.0, 183.1, 103.0), vec3(13.0, 186.5, 101.1), vec3(12.0, 201.5, 103.2), vec3(13.0, 207.6, 111.7),
    vec3(12.0, 208.6, 127.6), vec3(13.0, 203.2, 144.8), vec3(12.0, 198.5, 149.3), vec3(13.0, 193.3, 151.2), vec3(11.0, 186.7, 151.2),
    vec3(10.0, 40.1, 159.7),  vec3(11.0, 40.1, 100.3),  vec3(12.0, 41.0, 96.4),   vec3(13.0, 44.1, 94.0),   vec3(12.0, 46.2, 93.9),
    vec3(13.0, 48.2, 94.0),   vec3(12.0, 51.0, 95.8),   vec3(13.0, 51.9, 99.0),   vec3(11.0, 52.2, 147.2),  vec3(12.0, 53.4, 149.8),
    vec3(13.0, 55.9, 151.2),  vec3(12.0, 62.3, 151.5),  vec3(13.0, 68.6, 150.9),  vec3(12.0, 72.6, 148.7),  vec3(13.0, 75.0, 143.7),
    vec3(12.0, 74.2, 136.6),  vec3(13.0, 69.2, 131.5),  vec3(12.0, 65.0, 130.0),  vec3(13.0, 60.6, 129.7),  vec3(12.0, 58.1, 126.9),
    vec3(13.0, 60.8, 123.8),  vec3(12.0, 65.8, 123.6),  vec3(13.0, 69.9, 122.8),  vec3(12.0, 75.0, 116.5),  vec3(13.0, 74.3, 106.9),
    vec3(12.0, 71.9, 103.2),  vec3(13.0, 67.4, 101.2),  vec3(11.0, 60.5, 101.2),  vec3(12.0, 57.4, 97.9),   vec3(13.0, 60.7, 93.6),
    vec3(11.0, 74.0, 93.6),   vec3(12.0, 78.1, 94.5),   vec3(13.0, 82.1, 97.2),   vec3(12.0, 87.9, 109.3),  vec3(13.0, 84.1, 122.2),
    vec3(12.0, 82.0, 127.5),  vec3(13.0, 83.9, 132.8),  vec3(12.0, 87.2, 140.8),  vec3(13.0, 86.3, 150.0),  vec3(12.0, 80.2, 155.5),
    vec3(13.0, 70.7, 157.7),  vec3(11.0, 47.6, 157.7),  vec3(20.0, 52.3, 27.6),   vec3(21.0, 29.2, 27.8),   vec3(22.0, 26.6, 29.4),
    vec3(23.0, 25.5, 32.2),   vec3(21.0, 25.5, 70.3),   vec3(20.0, 66.4, 27.6),   vec3(21.0, 66.4, 72.7),   vec3(20.0, 115.5, 49.3),
    vec3(21.0, 128.2, 49.3),  vec3(22.0, 130.6, 48.2),  vec3(23.0, 131.6, 45.7),  vec3(21.0, 131.6, 36.4),  vec3(22.0, 128.1, 31.3),
    vec3(23.0, 122.3, 29.1),  vec3(22.0, 111.2, 28.0),  vec3(23.0, 100.4, 30.6),  vec3(22.0, 89.5, 53.4),   vec3(23.0, 113.8, 72.9),
    vec3(22.0, 121.0, 71.2),  vec3(23.0, 127.1, 66.8),  vec3(20.0, 153.6, 30.1),  vec3(21.0, 153.5, 71.6),  vec3(20.0, 190.3, 31.7),
    vec3(21.0, 190.1, 72.3),  vec3(20.0, 158.7, 51.3),  vec3(21.0, 184.0, 51.3),  vec3(20.0, 220.1, 31.9),  vec3(21.0, 220.1, 68.8),
    vec3(20.0, 203.0, 74.4),  vec3(21.0, 236.3, 74.2),  vec3(30.0, 16.8, 177.1),  vec3(32.0, 5.5, 172.9),   vec3(33.0, 0.2, 162.1),
    vec3(31.0, 0.0, 16.2),    vec3(32.0, 4.7, 4.9),     vec3(33.0, 16.0, 0.0),    vec3(31.0, 239.3, 0.0),   vec3(32.0, 250.6, 5.3),
    vec3(33.0, 255.0, 17.0),  vec3(31.0, 254.6, 160.3), vec3(30.0, 153.6, 30.1),  vec3(31.0, 153.5, 71.6),  vec3(30.0, 190.3, 31.7),
    vec3(31.0, 190.1, 72.3),  vec3(30.0, 158.7, 51.3),  vec3(31.0, 184.0, 51.3),  vec3(30.0, 220.1, 31.9),  vec3(31.0, 220.1, 68.8),
    vec3(30.0, 203.0, 74.4),  vec3(31.0, 236.3, 74.2),  vec3(30.0, 16.8, 177.1),  vec3(32.0, 5.5, 172.9),   vec3(33.0, 0.2, 162.1),
    vec3(31.0, 0.0, 16.2),    vec3(32.0, 4.7, 4.9),     vec3(33.0, 16.0, 0.0),    vec3(31.0, 239.3, 0.0),   vec3(32.0, 250.6, 5.3),
    vec3(33.0, 255.0, 17.0),  vec3(31.0, 254.6, 160.3), vec3(32.0, 249.9, 172.7), vec3(33.0, 237.3, 177.2), vec3(31.0, 16.8, 177.1)
);

vec3 neonSign1(vec2 uv)
{
    vec4 d1 = vec4(1E12);
    float d2;
    vec2 p1, p2, p3;
    int md;
    float width = 1.0;
    vec3 color;
    vec3 colors[] = vec3[] (vec3(1.0), vec3(0.04, 0.58, 0.96), vec3(0.96, 0.02, 0.0));
    for (int n = 1; n < bud.length(); n++)
    {
        p1 = bud[n - 1].yz;
        p2 = bud[n].yz;
        md = int(bud[n].x) % 10;
        color = colors[int(bud[n].x) / 10 - 1];
        if (md == 1) // LineTo
        {
            d2 = drawLine(uv, p1, p2, width);
        }
        if (md == 2) // ArcTo
        {
            p3 = bud[n + 1].yz;
            d2 = drawArc(uv, p1, p2, p3, width);
        }
        d1 = d2 < d1.w ? vec4(color, d2) : d1;
    }
    return d1.rgb * (smoothstep(1.2, 0.0, d1.w) + 0.45 / (d1.w * 0.2 + 1.0));
}

// *** Neon sign 2, unintelligible from a few pixels
int sign2a[] = int[] (
    0x00183000, 0x00387000, 0x00787000, 0x00F07380, 0x01F0FFC0, 0x07FFFF80, 0x17FFFF00, 0x7FFFFF00,
    0x7FFF8C00, 0x7FFF0000, 0x7FE70000, 0x7CC60000, 0x78070000, 0x70070000, 0x00068000, 0x0016C000,
    0x0013C000, 0x001FC000, 0x00078000, 0x00060000
);
int sign2b[] = int[] (
    0x001C0000, 0x0CFE0FFE, 0x1FFFFFF8, 0x3FFFFFF8, 0x3FDFFFFC, 0x7B9F17FC, 0x718F07E6, 0x000000C2,
    0x00000000, 0x58069EF6, 0x680000A0
);

vec3 neonSign2(in vec2 uv)
{
    ivec2 iv = ivec2(uv * vec2(32.0, -29.0) / 255.0) + ivec2(0, 28);
    if (iv.x >= 0 && iv.x < 31)
    {
        if (iv.y >= 0 && iv.y < 20)
        {
            return (sign2a[iv.y] & (1 << (31 - iv.x))) > 0 ? vec3(0.0, 0.6, 1.0) : vec3(0.0);
        }
        if (iv.y >= 20 && iv.y < 28)
        {
            return (sign2b[iv.y-20] & (1 << (31 - iv.x))) > 0 ? vec3(1.0, 0.0, 0.0) : vec3(0.0);
        }
    }
    return vec3(0.0);
}

// *** Neon sign 3, just an ellipse
vec3 neonSign3(in vec2 uv)
{
    return vec3(0.04, 0.58, 0.96) * smoothstep(0.1, 0.09, abs(0.9 - length(uv / 127.0 - vec2(1.0))));
}


void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
    if (iFrame > 2) discard;
    
    vec2 uv = fragCoord.xy * vec2(900.0, 600.0) / iResolution.xy - vec2(22.0, 33.0);
    fragColor.rgb =
        windowPart(uv) +
        sunlightPart(uv - vec2(300.0, 0.0)) +
        lightspotPart(uv - vec2(600.0, 0.0)) +
        neonSign1(uv - vec2(0.0, 300.0)) +
        neonSign2(uv - vec2(300.0, 300.0)) +
        neonSign3(uv - vec2(600.0, 300.0));
}