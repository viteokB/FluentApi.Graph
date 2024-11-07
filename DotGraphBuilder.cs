using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace FluentApi.Graph;


public class DotGraphBuilder :
    INodeBuilder,
    IEdgeBuilder,
    IBuilder
{
    private Graph _graph;

    private DotGraphBuilder(string graphName, bool directed, bool strict)
    {
        _graph = new Graph(graphName, directed, strict);
    }

    public static DotGraphBuilder DirectedGraph(string graphName)
    {
        return new DotGraphBuilder(graphName, true, false);
    }

    public static DotGraphBuilder UndirectedGraph(string graphName)
    {
        return new DotGraphBuilder(graphName, true, false);
    }


    public string Build()
    {
        throw new NotImplementedException();
    }

    public INodeBuilder With(Action<INodeBuilder> action)
    {
        throw new NotImplementedException();
    }

    public INodeBuilder AddNode(string nodeName)
    {
        throw new NotImplementedException();
    }

    public IEdgeBuilder With(Action<IEdgeBuilder> action)
    {
        throw new NotImplementedException();
    }

    public IEdgeBuilder AddEdge(string sourceNode, string destinationNode)
    {
        throw new NotImplementedException();
    }
}

public interface IBuilder
{
    public string Build();
}

public interface INodeBuilder : IBuilder, IWith<INodeBuilder>
{
    public INodeBuilder AddNode(string nodeName);
}

public interface IEdgeBuilder : IBuilder, IWith<IEdgeBuilder>
{
    public IEdgeBuilder AddEdge(string sourceNode, string destinationNode);
}

public interface IWith<out T> 
{
    public T With(Action<T> action);
}