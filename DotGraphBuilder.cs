using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace FluentApi.Graph;


public class DotGraphBuilder :
    INodeBuilder,
    IEdgeBuilder
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

    public INodeBuilder AddNode(string nodeName)
    {
        throw new NotImplementedException();
    }

    public IElement With(INodeBuilder builder)
    {
        throw new NotImplementedException();
    }

    public IEdgeBuilder AddEdge(string sourceNode, string destinationNode)
    {
        throw new NotImplementedException();
    }

    public IElement With(IEdgeBuilder builder)
    {
        throw new NotImplementedException();
    }
}

public interface IBuilder
{
    public string Build();
}

public interface IElement : INodeBuilder, IEdgeBuilder
{
    
}

public interface INodeBuilder : IBuilder
{
    public INodeBuilder AddNode(string nodeName);

    public IElement With(INodeBuilder  builder);
}

public interface IEdgeBuilder : IBuilder
{
    public IEdgeBuilder AddEdge(string sourceNode, string destinationNode);

    public IElement With(IEdgeBuilder builder);
}