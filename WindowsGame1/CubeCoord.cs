using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1
{
    class CubeCoord
    {
        private Int32 m_X;
        private Int32 m_Y;
        private Int32 m_Z;
        private static Int32[,] WAY_VECTORS = { { 1, 0, -1 }, { 1, -1, 0 }, { 0, -1, 1 }, { -1, 0, 1 }, { -1, 1, 0 }, { 0, 1, -1 } };
        public CubeCoord(Int32 x, Int32 y, Int32 z)
        {
            SetXYZ(x, y, z);

        }

        public void Shift(Int32 Way, Int32 Value)
        {
            m_X += Value * WAY_VECTORS[Way, 0];
            m_Y += Value * WAY_VECTORS[Way, 1];
            m_Z += Value * WAY_VECTORS[Way, 2];
        }

        public Int32 X
        {
            get { return m_X; }
        }

        public Int32 Y
        {
            get { return m_Y; }
        }

        public Int32 Z
        {
            get { return m_Z; }
        }

        public void SetXY(Int32 x, Int32 y)
        {
            m_X = x;
            m_Y = y;
            m_Z = -x -y;
        }

        public void SetXZ(Int32 x, Int32 z)
        {
            m_X = x;
            m_Z = z;
            m_Y = -x -z;
        }

        public void SetYZ(Int32 y, Int32 z)
        {
            m_Z = z;
            m_Y = y;
            m_Z = -y -z;
        }

        public void SetXYZ(Int32 x, Int32 y, Int32 z)
        {
            if (x + y + z == 0) { m_X = x; m_Y = y; m_Z = z; }
            if (x + y < -z) SetXY(x, y);
            if (x + z < -y) SetXZ(x, z);
            if (z + y < -x) SetYZ(y, z);
        }
    }
}
