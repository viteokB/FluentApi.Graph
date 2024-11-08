using System.Globalization;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using FluentApi.Graph;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace FluentApi.Graph;

public class DotGraphBuilder : INodeBuilder, IEdgeBuilder
{
    private readonly Graph _graph;

    private DotGraphBuilder(Graph graph) =>
        this._graph = graph;

    public static IDotGraphBuilder DirectedGraph(string graphName) =>
        new DotGraphBuilder(new Graph(graphName, true, true));

    public static IDotGraphBuilder UndirectedGraph(string graphName) =>
        new DotGraphBuilder(new Graph(graphName, false, true));

    public INodeBuilder AddNode(string name)
    {
        _graph.AddNode(name);
        return this;
    }

    public IEdgeBuilder AddEdge(string from, string to)
    {
        _graph.AddEdge(from, to);
        return this;
    }

    public string Build() => _graph.ToDotFormat();

    public IDotGraphBuilder With(Action<NodeAttributes> addAttributes)
    {
        addAttributes(new NodeAttributes(_graph.Nodes.Last()));
        return this;
    }

    public IDotGraphBuilder With(Action<EdgeAttributes> addAttributes)
    {
        addAttributes(new EdgeAttributes(_graph.Edges.Last()));
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

public abstract class BaseAttributes<TAttribute> where TAttribute : BaseAttributes<TAttribute>
{
    private readonly Dictionary<string, string> _attributes;

    protected BaseAttributes(Dictionary<string, string> attributes)
    {
        _attributes = attributes;
    }

    public TAttribute Label(string value) =>
        AddSomeAttribute(MethodBase.GetCurrentMethod(), value);

    public TAttribute FontSize(int value) =>
        AddSomeAttribute(MethodBase.GetCurrentMethod(), value.ToString());

    public TAttribute Color(string value) =>
        AddSomeAttribute(MethodBase.GetCurrentMethod(), value.ToString());

    //Используем имя метода для добавления атрибута
    protected TAttribute AddSomeAttribute(MethodBase attributeKey, string addValue)
    {
        _attributes.Add(attributeKey.Name.ToLower(), addValue);

        return (TAttribute) this;
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
        AddSomeAttribute(MethodBase.GetCurrentMethod(), shape.ToString().ToLower());
}

public class EdgeAttributes : BaseAttributes<EdgeAttributes>
{
    public EdgeAttributes(GraphEdge node) : base(node.Attributes) { }

    public EdgeAttributes Weight(double weightValue) =>
        AddSomeAttribute(MethodBase.GetCurrentMethod(), weightValue.ToString(CultureInfo.InvariantCulture));
}