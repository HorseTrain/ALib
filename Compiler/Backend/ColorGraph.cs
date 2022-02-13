using Compiler.Intermediate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler.Backend
{
    public class ColorGraphNode
    {
        public int CurrentColor                         { get; set; } = -1;
        public HashSet<ColorGraphNode> ConnectedNodes   { get; set; }

        public ColorGraphNode()
        {
            ConnectedNodes = new HashSet<ColorGraphNode>();
        }

        public void ConnectNode(ColorGraphNode Node)
        {
            if ((ConnectedNodes.Contains(Node) && Node.ConnectedNodes.Contains(this)) || Node == this)
            {
                return;
            }

            ConnectedNodes.Add(Node);
            Node.ConnectNode(this);
        }

        public void DisconnectNode(ColorGraphNode Node)
        {
            if (!(ConnectedNodes.Contains(Node) && Node.ConnectedNodes.Contains(this)) || Node == this)
                return;

            ConnectedNodes.Remove(Node);
            Node.ConnectedNodes.Remove(this);
        }

        public void Delete()
        {
            ColorGraphNode[] AllNodes = ConnectedNodes.ToArray();

            foreach (ColorGraphNode node in AllNodes)
            {
                DisconnectNode(node);
            }
        }
    }

    public class ColorGraph
    {
        List<ColorGraphNode> Nodes  { get; set; }

        public ColorGraph()
        {
            Nodes = new List<ColorGraphNode>();
        }

        public ColorGraphNode CreateNode()
        {
            ColorGraphNode Out = new ColorGraphNode();

            Nodes.Add(Out);

            return Out;
        }

        public void RemoveNode(ColorGraphNode Node)
        {
            Nodes.Remove(Node);

            Node.Delete();
        }

        public int ColorTheGraph()
        {
            foreach (ColorGraphNode node in Nodes)
            {
                node.CurrentColor = -1;
            }

            int k = 0;

            foreach (ColorGraphNode node in Nodes)
            {
                HashSet<int> Taken = new HashSet<int>();

                foreach (ColorGraphNode Child in node.ConnectedNodes)
                {
                    if (Child.CurrentColor != -1)
                    {
                        Taken.Add(Child.CurrentColor);
                    }
                }

                int Check = 0;

                while (true)
                {
                    if (Check > k)
                        k = Check;

                    if (!Taken.Contains(Check))
                        break;

                    Check++;
                }

                node.CurrentColor = Check;
            }

            return k;
        }
    }
}
