using System.Collections.Generic;
using System.Drawing;

namespace MangaTL.Core.Algorithms
{
    public static class RectangleFinder
    {
        //https://e-maxx.ru/algo/maximum_zero_submatrix
        public static Rectangle FindRectangle(bool[,] matrix)
        {
            var n = matrix.GetLength(0);
            var m = matrix.GetLength(1);

            var square = 0;
            var resRect = new Rectangle(0, 0, 0, 0);

            var d = new int[m];
            var d1 = new int[m];
            var d2 = new int[m];
            for (var k = 0; k < m; k++)
                d[k] = -1;

            var st = new Stack<int>();
            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < m; j++)
                    if (!matrix[i, j])
                        d[j] = i;
                st.Clear();
                for (var j = 0; j < m; ++j)
                {
                    while (st.Count != 0 && d[st.Peek()] <= d[j]) st.Pop();
                    d1[j] = st.Count == 0 ? -1 : st.Peek();
                    st.Push(j);
                }

                st.Clear();
                for (var j = m - 1; j >= 0; --j)
                {
                    while (st.Count != 0 && d[st.Peek()] <= d[j]) st.Pop();
                    d2[j] = st.Count == 0 ? m : st.Peek();
                    st.Push(j);
                }

                for (var j = 0; j < m; ++j)
                    if (square < (i - d[j]) * (d2[j] - d1[j] - 1))
                    {
                        square = (i - d[j]) * (d2[j] - d1[j] - 1);
                        resRect = new Rectangle(d1[j] + 1, d[j]+1, d2[j] - d1[j] - 1, i - d[j]);
                    }
            }

            return resRect;
        }
    }
}