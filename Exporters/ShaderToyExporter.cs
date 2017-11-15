using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Pool1984.Exporters
{
    class ShaderToyExporter : Exporter
    {
        public override string GetFileDialogFilter()
        {
            return "Shader files|*.glsl";
        }

        private string staticCode = @"
const float PI = acos(0.0) * 2.0;
const float MINDIST = 1E-5;
const float MAXDIST = 1E12;
const float TEXTUREANGLE = 0.55;

struct Intersection
{
	float dist;
	vec3 pos;
	vec3 nrm;
    vec3 tnrm;
    bool hit;
    int obj;
    vec2 txtOffset;
};

struct Ray
{
	vec3 ro;
	vec3 rd;
};

float hash12n(vec2 p)
{
	p  = fract(p * vec2(5123.3987, 5151.4321));
    p += dot(p.yx, p.xy + vec2(21.5351, 14.3137));
	return fract(p.x * p.y * 95.4323);
}
        ";

        private string DoubleToString(double d) => d.ToString("0.0##", CultureInfo.InvariantCulture);
        private string VecToString(Vector2 v) => $"vec2({DoubleToString(v.X)}, {DoubleToString(v.Y)})";
        private string VecToString(Vector3 v) => $"vec3({DoubleToString(v.X)}, {DoubleToString(v.Y)}, {DoubleToString(v.Z)})";
        private string ColToString(Color3 c) => $"vec3({DoubleToString(c.R)}, {DoubleToString(c.G)}, {DoubleToString(c.B)})";
        private string MatrixColumnToString(Matrix4 value, int v) => $"vec3({DoubleToString(value[0, v])}, {DoubleToString(value[1, v])}, {DoubleToString(value[2, v])})";
        private string Matrix4ToString(Matrix4 value) => $"mat3({MatrixColumnToString(value, 0)}, {MatrixColumnToString(value, 1)}, {MatrixColumnToString(value, 2)})"; 

        private string ExpressionToString(Expression expr, int indent = 0)
        {
            return ExpressionToString(expr, 0, indent).TrimEnd();
        }

        private string ExpressionToString(Expression expr, int level, int indent)
        {
            Func<int, string> getNewLine = indnt => "\r\n" + new string(' ', indnt * 4);
            Func<string, bool> splitLines = str => str.Contains('\r') || str.Length > 60;
            var bexpr = expr as BinaryExpression;
            string st;
            switch (expr.NodeType)
            {
                case ExpressionType.Parameter:
                    var pexpr = expr as ParameterExpression;
                    return pexpr.Name;
                case ExpressionType.Constant:
                    var cexpr1 = expr as ConstantExpression;
                    if (cexpr1.Value == null) return null;
                    if (cexpr1.Value.GetType() == typeof(double)) return DoubleToString((double)cexpr1.Value);
                    if (cexpr1.Value.GetType() == typeof(Vector2)) return VecToString((Vector2)cexpr1.Value);
                    if (cexpr1.Value.GetType() == typeof(Vector3)) return VecToString((Vector3)cexpr1.Value);
                    if (cexpr1.Value.GetType() == typeof(Matrix4)) return Matrix4ToString((Matrix4)cexpr1.Value);
                    return cexpr1.Value.ToString();
                case ExpressionType.New:
                    var nexpr = expr as NewExpression;
                    st = "vec2(" + ExpressionToString(nexpr.Arguments[0], 0, indent) + ", " + ExpressionToString(nexpr.Arguments[1], 0, indent) + ")";
                    if (nexpr.Type == typeof(Vector2)) return
                            nexpr.Arguments.Count() == 2 ?
                                splitLines(st) ?
                                    "vec2(" + getNewLine(indent + 1) +
                                        ExpressionToString(nexpr.Arguments[0], 0, indent + 1) + ", " + getNewLine(indent + 1) +
                                        ExpressionToString(nexpr.Arguments[1], 0, indent + 1) + getNewLine(indent) +
                                    ")" :
                                    st  :
                            "vec2(0.0)";
                    if (nexpr.Type == typeof(Vector3)) return
                            nexpr.Arguments.Count() == 3 ?
                                splitLines(st) ?
                                    "vec3(" + getNewLine(indent + 1) +
                                        ExpressionToString(nexpr.Arguments[0], 0, indent + 1) + ", " + getNewLine(indent + 1) +
                                        ExpressionToString(nexpr.Arguments[1], 0, indent + 1) + ", " + getNewLine(indent + 1) +
                                        ExpressionToString(nexpr.Arguments[2], 0, indent + 1) + getNewLine(indent) +
                                    ")" :
                                    st :
                            "vec3(0.0)";
                    throw new Exception("Unsupported type");
                case ExpressionType.Call:
                    var cexpr2 = expr as MethodCallExpression;
                    return cexpr2.Method.Name + "(" + String.Join(", ", cexpr2.Arguments.Select(it => ExpressionToString(it, 0, indent))) + ")";
                case ExpressionType.MemberAccess:
                    var mexpr = expr as MemberExpression;
                    return ExpressionToString(mexpr.Expression, 0, indent) + "." + mexpr.Member.Name.ToLower();
                case ExpressionType.AndAlso:
                    return ExpressionToString(bexpr.Left, 0, indent) + " && " + ExpressionToString(bexpr.Right, 0, indent);
                case ExpressionType.OrElse:
                    return ExpressionToString(bexpr.Left, 0, indent) + " || " + ExpressionToString(bexpr.Right, 0, indent);
                case ExpressionType.LessThan:
                    return ExpressionToString(bexpr.Left, 0, indent) + " < " + ExpressionToString(bexpr.Right, 0, indent);
                case ExpressionType.LessThanOrEqual:
                    return ExpressionToString(bexpr.Left, 0, indent) + " <= " + ExpressionToString(bexpr.Right, 0, indent);
                case ExpressionType.GreaterThan:
                    return ExpressionToString(bexpr.Left, 0, indent) + " > " + ExpressionToString(bexpr.Right, 0, indent);
                case ExpressionType.GreaterThanOrEqual:
                    return ExpressionToString(bexpr.Left, 0, indent) + " >= " + ExpressionToString(bexpr.Right, 0, indent);
                case ExpressionType.Add:
                    return 
                        level == 0 ?
                            ExpressionToString(bexpr.Left, 0, indent) + " + " + ExpressionToString(bexpr.Right, 0, indent) :
                            "(" + ExpressionToString(bexpr.Left, 0, indent) + " + " + ExpressionToString(bexpr.Right, 0, indent) + ")";
                case ExpressionType.Subtract:
                    return
                        level == 0 ?
                            ExpressionToString(bexpr.Left, 0, indent) + " - " + ExpressionToString(bexpr.Right, 0, indent) :
                            "(" + ExpressionToString(bexpr.Left, 0, indent) + " - " + ExpressionToString(bexpr.Right, 0, indent) + ")";
                case ExpressionType.Multiply:
                    return ExpressionToString(bexpr.Left, 1, indent) + " * " + ExpressionToString(bexpr.Right, 1, indent);
                case ExpressionType.Divide:
                    return ExpressionToString(bexpr.Left, 1, indent) + " / " + ExpressionToString(bexpr.Right, 1, indent);
                case ExpressionType.Conditional:
                    var cexpr = expr as ConditionalExpression;
                    st = ExpressionToString(cexpr.Test, 0, indent) + " ? " + ExpressionToString(cexpr.IfTrue, 0, indent) + " : " + ExpressionToString(cexpr.IfFalse, 0, indent);
                    return splitLines(st) ?
                        ExpressionToString(cexpr.Test, 0, indent) + " ?" + getNewLine(indent + 1) +
                        ExpressionToString(cexpr.IfTrue, 0, indent + 1) + " : " + getNewLine(indent + 1) +
                        ExpressionToString(cexpr.IfFalse, 0, indent + 1) + getNewLine(indent) :
                        st;
                case ExpressionType.Lambda:
                    var lexpr = expr as LambdaExpression;
                    return ExpressionToString(lexpr.Body, 0, indent);
                default:
                    throw new Exception($"Unsupported node type: {expr.NodeType}");
            }
        }

        public override void Export(Stream outputStream, Model model)
        {
            using (StreamWriter sw = new StreamWriter(outputStream))
            {
                sw.WriteLine(staticCode);

                sw.WriteLine("vec3[] lightPos = vec3[] (" + String.Join(", ", model.Lights.Select(lt => VecToString(lt.Center))) + ");");
                sw.WriteLine("float[] lightRad1 = float[] (" + String.Join(", ", model.Lights.Select(lt => DoubleToString(lt.Radius1))) + ");");
                sw.WriteLine("float[] lightRad2 = float[] (" + String.Join(", ", model.Lights.Select(lt => DoubleToString(lt.Radius2))) + ");");

                sw.WriteLine("vec3[] entityColor = vec3[] (" + String.Join(", ", model.Primitives.Select(ball => ColToString(ball.DiffuseColor))) + ");");
                sw.WriteLine();

                foreach (var ball in model.Primitives.OfType<Ball>())
                {
                    string name1 = ball.Name.Replace(" ", "");
                    string name2 = name1.ToLower();

                    sw.WriteLine($"vec3 Get{name1}Pos (in float t)");
                    sw.WriteLine("{");
                    sw.WriteLine($"    return {ExpressionToString(ball.GetCenterExpr, 1)};");
                    sw.WriteLine("}");
                    sw.WriteLine("");
                    sw.WriteLine($"vec3 Get{name1}TextureTransformation (in float t, in vec3 v)");
                    sw.WriteLine("{");
                    sw.WriteLine($"    return {ExpressionToString(ball.GetWorldToTextureTransformationExpr, 1)};");
                    sw.WriteLine("}");
                    sw.WriteLine("");
                }

                sw.WriteLine("Ray GetCamera(in vec2 fragCoord)");
                sw.WriteLine("{");
                sw.WriteLine($"    vec3 from = {VecToString(model.Camera.From)};");
                sw.WriteLine($"    vec3 at = {VecToString(model.Camera.At)};");
                sw.WriteLine($"    vec3 up = {VecToString(model.Camera.Up)};");
                sw.WriteLine($"    vec2 aper = {VecToString(new Vector2(model.Camera.ApertureH, model.Camera.ApertureV))};");
                sw.WriteLine(@"
    vec3 look = at - from;
    float dist = length(look);
    float hsize = tan(aper.x * PI / 180.0) * dist;
    float vsize = tan(aper.y * PI / 180.0) * dist;
    if (hsize * iResolution.x / iResolution.y > vsize)
        hsize = vsize * iResolution.x / iResolution.y;
    else
        vsize = hsize * iResolution.y / iResolution.x;
    vec3 hor = normalize(cross(look, up)) * hsize;
    vec3 ver = normalize(cross(hor, look)) * vsize;
    Ray ray;
    ray.ro = from;
    ray.rd = normalize(look + 
   		(fragCoord.x / iResolution.x * 2.0 - 1.0) * hor + 
        (fragCoord.y / iResolution.y * 2.0 - 1.0) * ver);
    return ray;
}");
                sw.WriteLine();
            }
        }
    }
}
