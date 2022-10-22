using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace graph
{
    class FileInfo
    {
        public int A { get; }
        public int B { get; }
        public List<List<int>> Edge { get; } = new List<List<int>>();

        public FileInfo(string Name)
        {
            int i = 0;
            string Str;
            using StreamReader f = new StreamReader(Name, System.Text.Encoding.Default);
            while ((Str = f.ReadLine()) != null)
            {
                List<int> Temp = Str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(i => int.Parse(i)).ToList();
                if (i == 0)
                {
                    A = Temp[0];
                    B = Temp[1];
                    i = -1; ;
                }
                else
                    Edge.Add(Temp);
            }
        }
    }

    class Vertex
    {
        public int A { get; }
        public List<int> Edge { get; }

        public static void Print(List<int> m)
        {
            for (int k = 0; k < m.Count; k++) { Console.WriteLine(m[k]); }
        }

        public Vertex(int A, List<List<int>> edge)
        {
            this.A = A;
            List<int> temp = new List<int>();
            for (int i = 0; i < edge.Count; i++)
            {
                for (int j = 0; j < edge[i].Count; j++)
                {
                    if ((A == edge[i][j]))
                        if (j == 0) temp.Add(edge[i][j + 1]);
                        else if (j == 1) temp.Add(edge[i][j - 1]);
                        else break;
                }
            }
            int n = temp.Distinct().ToList().Count;
            Edge = new List<int>(n);
            Edge = temp.Distinct().ToList();
        }
    }

    class Graph
    {
        public int A { get; }
        public int B { get; }
        public List<Vertex> Vertex { get; }

        public Graph(FileInfo m)
        {
            A = m.A;
            B = m.B;
            _ = new List<Vertex>();
            Vertex = DefinitionVertex(m.Edge).DistinctBy(x => x.A).ToList().OrderBy(u => u.A).ToList();
        }

        private List<Vertex> DefinitionVertex(List<List<int>> edge)
        {
            List<Vertex> temp = new List<Vertex>();
            for (int i = 0; i < edge.Count; i++)
                for (int j = 0; j < edge[i].Count; j++)
                    temp.Add(new Vertex(edge[i][j], edge));
            return temp;
        }

        private bool FindVertex(int A)
        {
            for (int i = 0; i < Vertex.Count; i++)
                if (A == Vertex[i].A) { return true; }
                else continue;
            return false;
        }

        private List<int> FindEdge(int A, List<Vertex> vertex)
        {
            for (int i = 0; i < vertex.Count; i++)
            {
                if (A == vertex[i].A) return vertex[i].Edge;
                else continue;
            }
            return null;
        }

        // todo collection is about many items, so you must use plural name form of variable name. "edge" -> "edges", "vertex" -> "vertices"
        private bool FindRoute(int A, int B, List<int> Prev, List<int> edge)
        {
            Prev.Add(A);
            if (A != B)
            {
                for (int i = 0; i < edge.Count; i++)
                    if (B == edge[i]) return true;
                    else continue;

                for (int i = 0; i < edge.Count; i++)
                    if (!Prev.Contains(edge[i]) && (B != edge[i])) return FindRoute(edge[i], B, Prev, FindEdge(edge[i], Vertex));
                    else if (Prev.Contains(edge[i])) continue;
                    else return false;
            }
            return true;
        }

        public bool Route()
        {
            if (FindVertex(A) && FindVertex(B))
            {
                List<int> Prev = new List<int>();
                return FindRoute(A, B, Prev, FindEdge(A, Vertex));
            }
            else return false;
        }

        public static void Print(List<Vertex> Vertex)
        {
            for (int i = 0; i < Vertex.Count; i++)
            {
                for (int j = 0; j < Vertex[i].Edge.Count; j++)
                    Console.WriteLine(Vertex[i].Edge[j]);
                Console.WriteLine();
            }
        }
    }

    class Program
    {
        static void Main()
        {
            // todo it's better to use relative path's instead of absolute. In future it's may be helpful : Path.Combine(), Directory.GetCurrentDirectory(), AppDomain.CurrentDomain.BaseDirectory
            Graph Graph = new Graph(new FileInfo(@"C:\Users\Alex\Documents\learning\graph\f.txt"));
            if (Graph.Route()) Console.WriteLine("route built");
            else Console.WriteLine("route not built");
        }
    }
}
