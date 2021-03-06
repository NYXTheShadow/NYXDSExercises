using System.Text;

namespace Trees
{
    public class Graph
    {
        private class Node
        {
            private string _label;

            public Node(string label)
            {
                this._label = label;
            }

            public override string ToString()
            {
                return this._label;
            }
        }

        private Dictionary<string, Node> _nodes;

        private Dictionary<Node, List<Node>> _aList;

        public Graph()
        {
            this._nodes = new Dictionary<string, Node>();
            this._aList = new Dictionary<Node, List<Node>>();
        }

        public void AddNode(string label)
        {
            if (label == null) return;
            if (this._nodes.ContainsKey(label)) throw new InvalidOperationException("Node Exists.");
            var newNode = new Node(label);
            this._nodes.Add(label, newNode);
            this._aList.Add(newNode, new List<Node>());
        }

        public void RemoveNode(string label)
        {
            if (label == null) return;
            if (!this._nodes.ContainsKey(label)) throw new ArgumentException();
            foreach (var nodeKey in this._aList.Keys) this._aList[nodeKey].Remove(nodeKey);
            this._aList.Remove(this._nodes[label]);
            this._nodes.Remove(label);
        }

        public void AddEdge(string from, string to)
        {
            if (!this._nodes.ContainsKey(from) || !this._nodes.ContainsKey(to)) throw new ArgumentException();
            this._aList[this._nodes[from]].Add(this._nodes[to]);
        }

        public void RemoveEdge(string from, string to)
        {
            if (!this._nodes.ContainsKey(from) || !this._nodes.ContainsKey(to)) throw new ArgumentException();
            this._aList[this._nodes[from]].Remove(this._nodes[to]);
        }

        public void TraverseDepthFirstIterative(string label)
        {
            if (!this._nodes.ContainsKey(label)) throw new ArgumentException("Node not found !");
            var set = new HashSet<Node>();
            TraverseDepthFirstIterative(this._nodes[label], set);
        }

        private void TraverseDepthFirstIterative(Node root, HashSet<Node> visited)
        {
            var stack = new Stack<Node>();
            stack.Push(root);
            while (stack.Count != 0)
            {
                var buffer = stack.Pop();
                if (visited.Contains(buffer)) continue;
                System.Console.WriteLine(buffer);
                visited.Add(buffer);
                foreach (var node in this._aList[buffer])
                {
                    if (!visited.Contains(node)) stack.Push(node);
                }
            }
        }
        public void TraverseDepthFirst(string label)
        {
            if (!this._nodes.ContainsKey(label)) throw new ArgumentException("Node not found!");
            TraverseDepthFirst(this._nodes[label], new HashSet<Node>());
        }

        private void TraverseDepthFirst(Node root, HashSet<Node> visited)
        {
            System.Console.WriteLine(root);
            visited.Add(root);
            foreach (var node in this._aList[root])
            {
                if (!visited.Contains(node)) TraverseDepthFirst(node, visited);
            }
        }

        public void TraverseBreadthFirst(string label)
        {
            if (!this._nodes.ContainsKey(label)) throw new ArgumentException("Node not found !");
            var set = new HashSet<Node>();
            TraverseBreadthFirst(this._nodes[label], set);
        }

        private void TraverseBreadthFirst(Node root, HashSet<Node> visited)
        {
            var q = new Queue<Node>();
            q.Enqueue(root);
            while (q.Count != 0)
            {
                var buffer = q.Dequeue();
                if (visited.Contains(buffer)) continue;
                System.Console.WriteLine(buffer);
                visited.Add(buffer);
                foreach (var node in this._aList[buffer])
                {
                    if (!visited.Contains(node)) q.Enqueue(node);
                }
            }
        }

        public List<string> TopologicalSort()
        {
            var stack = new Stack<Node>();
            var set = new HashSet<Node>();
            foreach (var node in this._nodes.Values)
                TopologicalSort(node, set, stack);
            var result = new List<string>();
            while (stack.Count != 0) result.Add(stack.Pop().ToString());
            return result;
        }

        private void TopologicalSort(Node root, HashSet<Node> visited, Stack<Node> results)
        {
            if (visited.Contains(root)) return;
            foreach (var node in this._aList[root])
            {
                TopologicalSort(node, visited, results);
            }

            if (!visited.Contains(root))
            {
                results.Push(root);
                visited.Add(root);
            }
        }

        public bool HasCycle()
        {
            var all = new HashSet<Node>();
            foreach (var node in this._nodes.Values) all.Add(node);
            var visiting = new HashSet<Node>();
            var visited = new HashSet<Node>();
            foreach (var node in this._nodes.Values)
                if (HasCycle(node, all, visiting, visited)) return true;
            return false;
        }

        private bool HasCycle(Node root, HashSet<Node> all, HashSet<Node> visiting, HashSet<Node> visited)
        {
            if (root == null) return false;
            all.Remove(root);
            visiting.Add(root);
            foreach (var node in this._aList[root])
                if (visiting.Contains(node)) return true;
                else
                    return HasCycle(node, all, visiting, visited);
            visiting.Remove(root);
            visited.Add(root);
            return false;
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            foreach (var nodeKey in this._aList.Keys)
            {
                result.Append($"{nodeKey} is connected with : ["
                    + string.Join(", ", this._aList[nodeKey])
                    + "]" + '\n');
            }
            return result.ToString();
        }
    }
}