using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace Pool1984.Exporters
{
    class ShaderToyExporter : Exporter
    {
        public override string GetFileDialogFilter()
        {
            return "Shader files|*.glsl";
        }

        public override string GetDefaultLocation()
        {
            return Path.GetFullPath(@"..\..\Shadertoy\Pool1984 - Image.glsl");
        }

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
                                    st :
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
            string template;
            using (StreamReader sr = new StreamReader(new FileStream(@"Shadertoy\Pool1984 - Image Template.glsl", FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                template = sr.ReadToEnd();
            }

            var sb = new StringBuilder();
            sb.AppendLine("const float PI = acos(0.0) * 2.0;");
            sb.AppendLine($"const float MINDIST = 0.001;");
            sb.AppendLine($"const float MAXDIST = 1000.0;");
            sb.AppendLine($"const float TEXTUREANGLE = {DoubleToString(Ball.TextureAngle)};");
            sb.AppendLine($"const vec3 AMBIENT = {ColToString(model.AmbientColor)};");
            sb.AppendLine($"const float REFL = {DoubleToString(model.Reflection)};");
            sb.AppendLine($"const float MAXITER = {DoubleToString(model.IterationDepth)};");
            sb.AppendLine();
            sb.AppendLine($"const float SAMPLEX = {DoubleToString(model.NrSamplesX)};");
            sb.AppendLine($"const float SAMPLEY = {DoubleToString(model.NrSamplesY)};");
            sb.AppendLine("const float SAMPLES = SAMPLEX * SAMPLEY;");
            sb.AppendLine();
            sb.AppendLine("const vec3[] LIGHT_POS = vec3[] (" + String.Join(", ", model.Lights.Select(lt => VecToString(lt.Center))) + ");");
            sb.AppendLine("const float[] LIGHT_RAD1 = float[] (" + String.Join(", ", model.Lights.Select(lt => DoubleToString(lt.Radius1))) + ");");
            sb.AppendLine("const float[] LIGHT_RAD2 = float[] (" + String.Join(", ", model.Lights.Select(lt => DoubleToString(lt.Radius2))) + ");");
            sb.AppendLine("const vec3[] COLOR = vec3[] (" + String.Join(", ", model.Primitives.Select(ball => ColToString(ball.DiffuseColor))) + ");");
            sb.AppendLine();
            string constPart = sb.ToString();

            sb = new StringBuilder();
            foreach (var ball in model.Primitives.OfType<Ball>())
            {
                string name1 = ball.Name.Replace(" ", "");
                string name2 = name1.ToLower();

                sb.AppendLine($"vec3 Get{name1}Pos (in float t)");
                sb.AppendLine("{");
                sb.AppendLine($"    return {ExpressionToString(ball.GetCenterExpr, 1)};");
                sb.AppendLine("}");
                sb.AppendLine("");
                sb.AppendLine($"vec3 Get{name1}TextureTransformation (in float t, in vec3 v)");
                sb.AppendLine("{");
                sb.AppendLine($"    return {ExpressionToString(ball.GetWorldToTextureTransformationExpr, 1)};");
                sb.AppendLine("}");
                sb.AppendLine("");
            }
            string positionsPart = sb.ToString();

            sb = new StringBuilder();
            sb.AppendLine($"        camera.from = {VecToString(model.Camera.From)};");
            sb.AppendLine($"        camera.at = {VecToString(model.Camera.At)};");
            sb.AppendLine($"        camera.up = {VecToString(model.Camera.Up)};");
            sb.AppendLine($"        camera.aper = {VecToString(new Vector2(model.Camera.ApertureH, model.Camera.ApertureV))};");
            string cameraPart = sb.ToString();

            string result = template
                .Replace("/*<-- CONST -->*/\r\n", constPart)
                .Replace("/*<-- POSITIONS -->*/\r\n", positionsPart)
                .Replace("/*<-- CAMERA -->*/\r\n", cameraPart);

            byte[] data = Encoding.UTF8.GetBytes(result);
            outputStream.Write(data, 0, data.Length);
            outputStream.Close();
        }
    }
}

