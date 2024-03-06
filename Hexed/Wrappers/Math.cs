using Hexed.Core;
using System.Drawing;
using System.Numerics;

namespace Hexed.Wrappers
{
    internal class Math
    {
        public static string ByteSizeToString(long size)
        {
            string[] strArrays = new string[] { "B", "KB", "MB", "GB", "TB" };
            int num = 0;
            while (size > 1024)
            {
                size /= 1024;
                num++;
            }
            return string.Format("{0} {1}", size.ToString(), strArrays[num]);
        }

        public static Vector2 WorldToScreen(Vector3 origin, Vector3 cameraLocation, Quaternion cameraRotation, float fov, Vector2 Screenlocation)
        {
            Matrix4x4 tempMatrix = Matrix4x4.CreateFromQuaternion(cameraRotation);

            Vector3 vAxisX, vAxisY, vAxisZ;

            vAxisX = new Vector3(tempMatrix.M11, tempMatrix.M12, tempMatrix.M13);
            vAxisY = new Vector3(tempMatrix.M21, tempMatrix.M22, tempMatrix.M23);
            vAxisZ = new Vector3(tempMatrix.M31, tempMatrix.M32, tempMatrix.M33);

            Vector3 vDelta = origin - cameraLocation;
            Vector3 vTransformed = new(Vector3.Dot(vDelta, vAxisY), Vector3.Dot(vDelta, vAxisZ), Vector3.Dot(vDelta, vAxisX));

            if (vTransformed.Z < 1f) vTransformed.Z = 1f;

            float ScreenCenterX = Screenlocation.X / 2;
            float ScreenCenterY = Screenlocation.Y / 2;

            float X = ScreenCenterX + vTransformed.X * (ScreenCenterX / (float)System.Math.Tan(fov * (float)System.Math.PI / 360f)) / vTransformed.Z;
            float Y = ScreenCenterY - vTransformed.Y * (ScreenCenterX / (float)System.Math.Tan(fov * (float)System.Math.PI / 360f)) / vTransformed.Z;

            return new Vector2(X, Y);
        }

        public static Color ColorByGuid(Guid guid)
        {
            Color color = Color.FromArgb(guid.GetHashCode());

            const int brightnessOffset = 20;
            const int saturationOffset = 15;

            int red = System.Math.Min(color.R + brightnessOffset, 255);
            int green = System.Math.Min(color.G + brightnessOffset, 255);
            int blue = System.Math.Min(color.B + brightnessOffset, 255);

            int maxColor = System.Math.Max(red, System.Math.Max(green, blue));
            int minColor = System.Math.Min(red, System.Math.Min(green, blue));

            if (maxColor != minColor)
            {
                double saturation = (maxColor - minColor) / (double)maxColor;
                saturation *= 255;
                saturation = System.Math.Max(saturation - saturationOffset, 0);
                saturation = System.Math.Min(saturation, 255);

                red = (int)((red - minColor) / (double)(maxColor - minColor) * saturation + minColor);
                green = (int)((green - minColor) / (double)(maxColor - minColor) * saturation + minColor);
                blue = (int)((blue - minColor) / (double)(maxColor - minColor) * saturation + minColor);
            }

            return Color.FromArgb(255, red, green, blue);
        }

        public static double GetRotationAngle(double x, double y, double z, double w)
        {
            ToEulerAngles(x, y, z, w, out double pitch, out double yaw, out double roll);

            double rotation = yaw * 180 / System.Math.PI;

            rotation %= 360;
            if (rotation < 0) rotation += 360;

            return rotation;
        }

        private static void ToEulerAngles(double x, double y, double z, double w, out double pitch, out double yaw, out double roll)
        {
            double sinr_cosp = 2 * (w * x + y * z);
            double cosr_cosp = 1 - 2 * (x * x + y * y);
            roll = System.Math.Atan2(sinr_cosp, cosr_cosp);

            double sinp = 2 * (w * y - z * x);
            if (System.Math.Abs(sinp) >= 1)  pitch = System.Math.PI / 2 * System.Math.Sign(sinp);
            else pitch = System.Math.Asin(sinp);

            double siny_cosp = 2 * (w * z + x * y);
            double cosy_cosp = 1 - 2 * (y * y + z * z);
            yaw = System.Math.Atan2(siny_cosp, cosy_cosp);
        }

        public static string GetDirection(double rotation)
        {
            rotation %= 360;
            if (rotation < 0) rotation += 360;

            string[] directions = { "E", "S/E", "S", "S/W", "W", "N/W", "N", "N/E" };
            double[] angleRanges = { 22.5, 67.5, 112.5, 157.5, 202.5, 247.5, 292.5, 337.5 };

            for (int i = 0; i < directions.Length; i++)
            {
                if (rotation < angleRanges[i]) return directions[i];
            }

            return directions[0];
        }
    }
}
