using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

//This class is a port of Ladislav Kavan's code, the original of which can be found here: http://isg.cs.tcd.ie/projects/DualQuaternions/
//Note: This is not general purpose dual quaternion code. It performs special processing that makes linear blending easier. 
//      See the section called "The Final Algorithm" in http://isg.cs.tcd.ie/kavanl/sdq.pdf for more information.
namespace Myko.Xna.SkinnedModel
{
    internal class DualQuaternion
    {
        public Quaternion Ordinary;
        public Quaternion Dual;

        /// <summary>
        /// Converts a rotation and translation into a DualQuaternion.
        /// </summary>
        /// <param name="q0">Unit rotation quaternion.</param>
        /// <param name="t">Translation vector</param>
        /// <returns>A special dual quaternion that can be linearly blended with other dual quaternions with minimal error.</returns>
        public static DualQuaternion QuatTrans2UDQ(Quaternion q0, Vector3 t)
        {
            DualQuaternion dq = new DualQuaternion();
            // non-dual part (just copy q0):
            dq.Ordinary = q0;
            // dual part:            
            dq.Dual.W = -0.5f * (t.X * q0.X + t.Y * q0.Y + t.Z * q0.Z);
            dq.Dual.X = 0.5f * (t.X * q0.W + t.Y * q0.Z - t.Z * q0.Y);
            dq.Dual.Y = 0.5f * (-t.X * q0.Z + t.Y * q0.W + t.Z * q0.X);
            dq.Dual.Z = 0.5f * (t.X * q0.Y - t.Y * q0.X + t.Z * q0.W);
            return dq;
        }

        /// <summary>
        /// Converts a DualQuaternion created using QuatTrans2UDQ back into a matrix. This code is currently not being used by the program.
        /// </summary>
        /// <param name="dq">A DualQuaternion created with QuatTrans2UDQ()</param>
        /// <returns>The matrix represented by dq.</returns>
        public static Matrix UDQToMatrix(DualQuaternion dq)
        {
	        Matrix M;
	        float len2 = Quaternion.Dot(dq.Ordinary, dq.Ordinary);
            float w = dq.Ordinary.W, x = dq.Ordinary.X, y = dq.Ordinary.Y, z = dq.Ordinary.Z;
            float t0 = dq.Dual.W, t1 = dq.Dual.X, t2 = dq.Dual.Y, t3 = dq.Dual.Z;
				
	        M.M11 = w*w + x*x - y*y - z*z;
            M.M21 = 2 * x * y - 2 * w * z;
            M.M31 = 2 * x * z + 2 * w * y;
            M.M12 = 2 * x * y + 2 * w * z;
            M.M22 = w * w + y * y - x * x - z * z;
            M.M32 = 2 * y * z - 2 * w * x;
            M.M13 = 2 * x * z - 2 * w * y;
            M.M23 = 2 * y * z + 2 * w * x;
            M.M33 = w * w + z * z - x * x - y * y;

            M.M41 = -2 * t0 * x + 2 * w * t1 - 2 * t2 * z + 2 * y * t3;
            M.M42 = -2 * t0 * y + 2 * t1 * z - 2 * x * t3 + 2 * w * t2;
            M.M43 = -2 * t0 * z + 2 * x * t2 + 2 * w * t3 - 2 * t1 * y;

            M.M14 = 0;
            M.M24 = 0;
            M.M34 = 0;
            M.M44 = len2;
	
	        M /= len2;
	
	        return M;	
        }
    }
}
