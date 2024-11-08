using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using FluentApi.Graph;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace FluentApi.Graph;

public class DotGraphBuilder : INodeBuilder, IEdgeBuilder
{
    private readonly Graph graph;

    private DotGraphBuilder(Graph graph) =>
        this.graph = graph;

    public static IDotGraphBuilder DirectedGraph(string graphName) =>
        new DotGraphBuilder(new Graph(graphName, true, true));

    public static IDotGraphBuilder UndirectedGraph(string graphName) =>
        new DotGraphBuilder(new Graph(graphName, false, true));

    public INodeBuilder AddNode(string name)
    {
        graph.AddNode(name);
        return this;
    }

    public IEdgeBuilder AddEdge(string from, string to)
    {
        graph.AddEdge(from, to);
        return this;
    }

    public string Build() => graph.ToDotFormat();

    public IDotGraphBuilder With(Action<NodeAttributes> addAttributes)
    {
        addAttributes(new NodeAttributes(graph.Nodes.Last()));
        return this;
    }

    public IDotGraphBuilder With(Action<EdgeAttributes> addAttributes)
    {
        addAttributes(new EdgeAttributes(graph.Edges.Last()));
        return this;
    }
} 

public interface INodeBuilder : IDotGraphBuilder
{
    public IDotGraphBuilder With(Action<NodeAttributes> nodeAttributes);
}

public interface IEdgeBuilder : IDotGraphBuilder
{
    public IDotGraphBuilder With(Action<EdgeAttributes>  edgeAttributes);
}

public interface IDotGraphBuilder
{
    public INodeBuilder AddNode(string nodeName);

    public IEdgeBuilder AddEdge(string sourceNode, string destinationNode);

    public string Build();
}

public abstract class BaseAttributes<TAtr> where TAtr: BaseAttributes<TAtr>
{
    private readonly Dictionary<string, string> _attributes;

    protected BaseAttributes(Dictionary<string, string> attributes)
    {
        _attributes = attributes;
    }

    public TAtr Label(string value) =>
        AddSomeAttribute(MethodBase.GetCurrentMethod(), value);

    public TAtr FontSize(int value) =>
        AddSomeAttribute(MethodBase.GetCurrentMethod(), value.ToString());

    public TAtr Color(string value) =>
        AddSomeAttribute(MethodBase.GetCurrentMethod(), value.ToString());

    protected TAtr AddSomeAttribute(MethodBase attributeKey, string addValue)
    {
        _attributes.Add(attributeKey.Name.ToLower(), addValue);

        return (TAtr) this;
    }
}

public enum NodeShape
{
    Box,
    Ellipse
}

public class NodeAttributes : BaseAttributes<NodeAttributes>
{
    public NodeAttributes(GraphNode node) : base(node.Attributes) { }

    public NodeAttributes Shape(NodeShape shape) =>
        AddSomeAttribute(MethodBase.GetCurrentMethod(), shape.ToString());
}

public class EdgeAttributes : BaseAttributes<EdgeAttributes>
{
    public EdgeAttributes(GraphEdge node) : base(node.Attributes) { }

    public EdgeAttributes Weight(double weightValue) =>
        AddSomeAttribute(MethodBase.GetCurrentMethod(), weightValue.ToString());
}